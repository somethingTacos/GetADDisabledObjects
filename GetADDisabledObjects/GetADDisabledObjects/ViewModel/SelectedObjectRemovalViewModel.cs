using GetADDisabledObjects.Model;
using GetADDisabledObjects.Helpers;

namespace GetADDisabledObjects.ViewModel
{
    public class SelectedObjectRemovalViewModel
    {
        private NavigationViewModel _navigationViewModel { get; set; }
        public SelectedObjects selectedObjects { get; set; }

        public MyICommand DoneCommand { get; set; }

        public SelectedObjectRemovalViewModel(NavigationViewModel navigationViewModel, AllObjects ObjectsToRemove)
        {
            _navigationViewModel = navigationViewModel;
            DoneCommand = new MyICommand(onDoneCommand, canDoneCommand);

            initObjectsToRemove(ObjectsToRemove);
        }

        public void initObjectsToRemove(AllObjects objectsToRemove)
        {
            SelectedObjects tempSO = new SelectedObjects();
            tempSO.allObjects = objectsToRemove;

            selectedObjects = tempSO;
        }

        #region Commands Code
        public void onDoneCommand(object parameter)
        {
            _navigationViewModel.SelectedViewModel = new AllObjectsViewModel(_navigationViewModel);
        }
        public bool canDoneCommand()
        {
            return true;
        }
        #endregion
    }
}
