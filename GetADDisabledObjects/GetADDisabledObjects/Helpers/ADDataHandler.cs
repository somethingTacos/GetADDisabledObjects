using System;
using System.Linq;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using System.Collections.ObjectModel;
using GetADDisabledObjects.Model;

namespace GetADDisabledObjects.Helpers
{
    public static class ADDataHandler
    {
        private static AllObjects ReturnableAllObjects;
        public async static Task<AllObjects> GetDisabledObjects()
        {
            await Task.Run(() =>
            {
                ReturnableAllObjects = _GetDisabledObjects();
            });

            return ReturnableAllObjects;
        }

        private static AllObjects _GetDisabledObjects()
        {
            AllObjects AllDisabledObjectsData = new AllObjects();

            AllDisabledObjectsData.DisabledComputers = GetDisabledComputers();
            AllDisabledObjectsData.DisabledUsers = GetDisabledUsers();

            return AllDisabledObjectsData;
        }

        private static Tuple<bool, AllObjects> AllObjectsRemoved;
        public async static Task<Tuple<bool,AllObjects>> RemoveSelectedObjects(AllObjects ObjectsToRemove)
        {
            await Task.Run(() =>
            {
                AllObjectsRemoved = _RemoveSelectedObjects(ObjectsToRemove);
            });

            return AllObjectsRemoved;
        }

        private static Tuple<bool, AllObjects> _RemoveSelectedObjects(AllObjects ObjectsToRemove)
        {
            bool OperationSucceeded = false;
            ObservableCollection<ComputerObject> CompErrors = RemoveComps(ObjectsToRemove.DisabledComputers);
            ObservableCollection<UserObject> UserErrors = RemoveUsers(ObjectsToRemove.DisabledUsers);
            AllObjects ReturnData = new AllObjects { DisabledComputers = CompErrors, DisabledUsers = UserErrors };

            if(ReturnData.DisabledComputers.Count() == 0 && ReturnData.DisabledUsers.Count() == 0)
            {
                OperationSucceeded = true;
            }


            return new Tuple<bool, AllObjects>(OperationSucceeded, ReturnData);
        }

        

        private static string GetCanonicalName(string DistinguisedName) //I couldn't figure out how to use the directoryentry refreshcache, so I just did this... which, probably works fine...
        {
            string cn = "";
            ObservableCollection<string> reversedCN = new ObservableCollection<string>();
            ObservableCollection<string> dcA = new ObservableCollection<string>();
            string[] dn = DistinguisedName.Split(',');

            foreach (string n in dn)
            {
                string[] tempS = n.Split('=');

                if (tempS[0] != "CN" && tempS[0] != "DC")
                {
                    reversedCN.Add(tempS[1]);
                }

                if (tempS[0] == "DC")
                {
                    dcA.Add(tempS[1]);
                }
            }

            cn = String.Join(".", dcA);
            cn += "/";
            cn += String.Join("/", reversedCN.Reverse());

            if(!cn.EndsWith("/"))
            {
                cn += "/";
            }

            return cn;
        }

        #region Private Methods
        private static ObservableCollection<ComputerObject> RemoveComps(ObservableCollection<ComputerObject> CompsToRemove)
        {
            ObservableCollection<ComputerObject> CompErrorList = new ObservableCollection<ComputerObject>();
            try
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain);
                foreach (ComputerObject comp in CompsToRemove)
                {
                    ComputerPrincipal computer = ComputerPrincipal.FindByIdentity(context, comp.SamAccountName);

                    if (computer != null)
                    {
                        computer.Delete();
                    }
                    else
                    {
                        CompErrorList.Add(comp);
                    }

                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

            return CompErrorList;
        }
        private static ObservableCollection<UserObject> RemoveUsers(ObservableCollection<UserObject> UsersToRemove)
        {
            ObservableCollection<UserObject> UserErrorList = new ObservableCollection<UserObject>();
            try
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain);
                foreach (UserObject usr in UsersToRemove)
                {
                    UserPrincipal user = UserPrincipal.FindByIdentity(context, usr.SamAccountName);
                    if (user != null)
                    {
                        user.Delete();
                    }
                    else
                    {
                        UserErrorList.Add(usr);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

            return UserErrorList;
        }

        private static ObservableCollection<ComputerObject> GetDisabledComputers()
        {
            ObservableCollection<ComputerObject> DisabledComps = new ObservableCollection<ComputerObject>();

            try
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain);
                ComputerPrincipal computer = new ComputerPrincipal(context);

                PrincipalSearcher searcher = new PrincipalSearcher(computer);

                foreach (var obj in searcher.FindAll())
                {
                    ComputerPrincipal comp = obj as ComputerPrincipal;

                    if (comp != null)
                    {
                        if (comp.Enabled == false)
                        {
                            DisabledComps.Add(new ComputerObject
                            {
                                Name = comp.Name,
                                SamAccountName = comp.SamAccountName,
                                Location = GetCanonicalName(comp.DistinguishedName),
                                IsSelected = false
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: {ex.Message}\n\nIs host in a domain?", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

            return DisabledComps;
        }

        private static ObservableCollection<UserObject> GetDisabledUsers()
        {
            ObservableCollection<UserObject> DisabledUsers = new ObservableCollection<UserObject>();
            string[] BuiltInAccounts = { "Guest", "krbtgt", "DefaultAccount" }; // I should make it so you can add to an 'Ignore' list... I'll consider it...

            try
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain);
                UserPrincipal user = new UserPrincipal(context);

                PrincipalSearcher searcher = new PrincipalSearcher(user);

                foreach (var obj in searcher.FindAll())
                {
                    UserPrincipal usr = obj as UserPrincipal;

                    if (usr != null && BuiltInAccounts.Where(x => x == usr.Name).Count() < 1)
                    {
                        if (usr.Enabled == false)
                        {
                            DisabledUsers.Add(new UserObject
                            {
                                Name = usr.Name,
                                SamAccountName = usr.SamAccountName,
                                Location = GetCanonicalName(usr.DistinguishedName),
                                IsSelected = false
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: {ex.Message}\n\nIs host in a domain?", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

            return DisabledUsers;
        }
        #endregion
    }
}
