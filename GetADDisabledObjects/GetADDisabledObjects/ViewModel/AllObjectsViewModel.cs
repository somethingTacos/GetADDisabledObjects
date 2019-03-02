using GetADDisabledObjects.Helpers;
using GetADDisabledObjects.Model;

namespace GetADDisabledObjects.ViewModel
{
    public class AllObjectsViewModel
    {
        public AllObjects AllDisabledObjects { get; set; }
        private NavigationViewModel _navigationViewModel { get; set; }

        public MyICommand ExitCommand { get; set; }
        public MyICommand GetDisabledObjectsCommand { get; set; }
        public MyICommand DeleteSelectedObjectsCommand { get; set; }

        public AllObjectsViewModel(NavigationViewModel navigationViewModel)
        {
            _navigationViewModel = navigationViewModel;
            ExitCommand = new MyICommand(onExitCommand, canExitCommand);
            GetDisabledObjectsCommand = new MyICommand(onGetDisabledObjectsCommand, canGetDisabledObjectsCommand);
            DeleteSelectedObjectsCommand = new MyICommand(onDeleteSelectedObjectsCommand, canDeleteSelectedObjectsCommand);
            initAllDisabledObjects();
        }

        public void initAllDisabledObjects()
        {
            AllObjects tempObjects = new AllObjects();

            AllDisabledObjects = tempObjects;
        }

        #region Commands Code
        public void onGetDisabledObjectsCommand(object parameter)
        {
            AllDisabledObjects = ADDataHandler.GetDisabledObjects();
        }
        public bool canGetDisabledObjectsCommand()
        {
            return true;
        }


        public void onDeleteSelectedObjectsCommand(object parameter)
        {
            System.Windows.MessageBox.Show("Not setup yet  :(");
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
