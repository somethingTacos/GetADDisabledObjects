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

            PrincipalContext context = new PrincipalContext(ContextType.Domain);
            ComputerPrincipal computer = new ComputerPrincipal(context);

            PrincipalSearcher searcher = new PrincipalSearcher(computer);

            foreach(var obj in searcher.FindAll())
            {
                ComputerPrincipal comp = obj as ComputerPrincipal;

                if(comp != null)
                {
                    if(comp.Enabled == false)
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

            return DisabledComps;
        }

        private static ObservableCollection<UserObject> GetDisabledUsers()
        {


            return new ObservableCollection<UserObject>();
        }
        #endregion
    }
}
