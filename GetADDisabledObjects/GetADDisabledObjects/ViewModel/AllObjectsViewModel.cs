using GetADDisabledObjects.Helpers;
using GetADDisabledObjects.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GetADDisabledObjects.ViewModel
{
    public class AllObjectsViewModel
    {
        public AllObjects AllDisabledObjects { get; set; }
        private NavigationViewModel _navigationViewModel { get; set; }

        public MyICommand ExitCommand { get; set; }
        public MyICommand DeleteSelectedObjectsCommand { get; set; }
        public AwaitableDelegateCommand GetDisabledObjectsCommand { get; set; }

        public AllObjectsViewModel(NavigationViewModel navigationViewModel)
        {
            _navigationViewModel = navigationViewModel;
            ExitCommand = new MyICommand(onExitCommand, canExitCommand);
            GetDisabledObjectsCommand = new AwaitableDelegateCommand(onGetDisabledObjectsCommand, canGetDisabledObjectsCommand);
            DeleteSelectedObjectsCommand = new MyICommand(onDeleteSelectedObjectsCommand, canDeleteSelectedObjectsCommand);
            initAllDisabledObjects();
        }

        public void initAllDisabledObjects()
        {
            AllObjects tempObjects = new AllObjects();

            AllDisabledObjects = tempObjects;
        }

        #region Commands Code
        public async Task onGetDisabledObjectsCommand()
        {
            AllDisabledObjects.MainButtonText = "Fetching Disabled Objects...";
            AllDisabledObjects.FetchingObjectsGifVisibility = Visibility.Visible;
            AllObjects tempAO = new AllObjects();

            tempAO = await ADDataHandler.GetDisabledObjects();

            AllDisabledObjects.DisabledComputers.Clear();
            AllDisabledObjects.DisabledUsers.Clear();

            AllDisabledObjects.DisabledComputers = tempAO.DisabledComputers;
            AllDisabledObjects.DisabledUsers = tempAO.DisabledUsers;

            AllDisabledObjects.FetchingObjectsGifVisibility = Visibility.Hidden;
            AllDisabledObjects.MainButtonText = "Get Disabled Users and Computers";
        }
        public bool canGetDisabledObjectsCommand()
        {
            return true;
        }


        public void onDeleteSelectedObjectsCommand(object parameter)
        {
            AllObjects ObjectsToRemove = new AllObjects();
            var selectedComps = AllDisabledObjects.DisabledComputers.Where(x => x.IsSelected).ToArray<ComputerObject>();
            var selectedUsers = AllDisabledObjects.DisabledUsers.Where(x => x.IsSelected).ToArray<UserObject>();
            ObjectsToRemove.DisabledComputers = new ObservableCollection<ComputerObject>(selectedComps);
            ObjectsToRemove.DisabledUsers = new ObservableCollection<UserObject>(selectedUsers);

            _navigationViewModel.SelectedViewModel = new SelectedObjectRemovalViewModel(_navigationViewModel, ObjectsToRemove);
        }
        public bool canDeleteSelectedObjectsCommand()
        {
            return true;
        }


        public void onExitCommand(object parameter)
        {
            System.Windows.Application.Current.Shutdown();
        }
        public bool canExitCommand()
        {
            return true;
        }
        #endregion
    }
}
