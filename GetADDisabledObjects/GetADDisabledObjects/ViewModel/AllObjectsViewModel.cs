using GetADDisabledObjects.Helpers;
using GetADDisabledObjects.Model;
using System.Threading.Tasks;

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
            AllObjects tempAO = new AllObjects();

            tempAO = ADDataHandler.GetDisabledObjects();

            AllDisabledObjects.DisabledComputers.Clear();
            AllDisabledObjects.DisabledUsers.Clear();

            AllDisabledObjects.DisabledComputers = tempAO.DisabledComputers;
            AllDisabledObjects.DisabledUsers = tempAO.DisabledUsers;
        }
        public bool canGetDisabledObjectsCommand()
        {
            return true;
        }


        public void onDeleteSelectedObjectsCommand(object parameter)
        {
            _navigationViewModel.SelectedViewModel = new SelectedObjectRemovalViewModel(_navigationViewModel, AllDisabledObjects);
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
