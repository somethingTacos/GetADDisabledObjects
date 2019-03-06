using System;
using System.Linq;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Collections.ObjectModel;
using GetADDisabledObjects.Model;

namespace GetADDisabledObjects.Helpers
{
    public static class ADDataHandler
    {
        //EXAMPLE TASK CODE, because lazy...

        //private RTMonitor ReturnableRTData;
        //public async Task<RTMonitor> CheckRTConnections(RTMonitor TargetsToCheck)
        //{
        //    await Task.Run(() =>
        //    {
        //        ReturnableRTData = _CheckRTConnections(TargetsToCheck);

        //    });
        //    return ReturnableRTData;
        //}


        public static AllObjects GetDisabledObjects()
        {
            AllObjects AllDisabledComputersData = new AllObjects();

            AllDisabledComputersData.DisabledComputers = GetDisabledComputers();
            AllDisabledComputersData.DisabledUsers = GetDisabledUsers();

            return AllDisabledComputersData;
        }


        #region Private Methods
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
                                Location = comp.DistinguishedName,
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

            try
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain);
                UserPrincipal user = new UserPrincipal(context);

                PrincipalSearcher searcher = new PrincipalSearcher(user);

                foreach (var obj in searcher.FindAll())
                {
                    UserPrincipal usr = obj as UserPrincipal;

                    if (usr != null)
                    {
                        if (usr.Enabled == false)
                        {
                            DisabledUsers.Add(new UserObject
                            {
                                Name = usr.Name,
                                Location = usr.DistinguishedName,
                                IsSelected = false
                            });
                        }
                    }
                }
            }
            catch (Exception) { }

            return DisabledUsers;
        }
        #endregion
    }
}
