using GetADDisabledObjects.Model;
using GetADDisabledObjects.Helpers;
using System.Windows;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Threading.Tasks;
using System;

namespace GetADDisabledObjects.ViewModel
{
    public class SelectedObjectRemovalViewModel
    {
        private NavigationViewModel _navigationViewModel { get; set; }
        public SelectedObjects selectedObjects { get; set; }
        public ObservableCollection<object> RemovalDataList { get; set; } = new ObservableCollection<object>();

        public MyICommand DoneCommand { get; set; }
        public AwaitableDelegateCommand ConfirmRemovalCommand { get; set; }

        public SelectedObjectRemovalViewModel(NavigationViewModel navigationViewModel, AllObjects ObjectsToRemove)
        {
            _navigationViewModel = navigationViewModel;
            DoneCommand = new MyICommand(onDoneCommand, canDoneCommand);
            ConfirmRemovalCommand = new AwaitableDelegateCommand(onConfirmRemovalCommand, canConfirmRemovalCommand);

            initObjectsToRemove(ObjectsToRemove);
        }

        public void initObjectsToRemove(AllObjects objectsToRemove)
        {
            SelectedObjects tempSO = new SelectedObjects();
            tempSO.allObjects = objectsToRemove;

            foreach (ComputerObject comp in tempSO.allObjects.DisabledComputers)
            {
                RemovalDataList.Add(comp);
            }
            foreach (UserObject usr in tempSO.allObjects.DisabledUsers)
            {
                RemovalDataList.Add(usr);
            }

            if(tempSO.allObjects.DisabledUsers.Count == 0 && tempSO.allObjects.DisabledComputers.Count == 0)
            {
                tempSO.OperationCompleteButtonText = "Done";
            }

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

        public async Task onConfirmRemovalCommand()
        {
            Tuple<bool, AllObjects> RemovalSucceeded = await ADDataHandler.RemoveSelectedObjects(selectedObjects.allObjects);
            
            selectedObjects.RemovalButtonVisibility = Visibility.Hidden;
            selectedObjects.OperationCompleteButtonText = "Done";
            RemovalDataList.Clear();

            if (!RemovalSucceeded.Item1)
            {
                foreach(ComputerObject comp in RemovalSucceeded.Item2.DisabledComputers)
                {
                    FailedRemovalObject failedComp = new FailedRemovalObject { Name = comp.Name, Location = comp.Location };
                    RemovalDataList.Add(failedComp);
                }
                foreach (UserObject usr in RemovalSucceeded.Item2.DisabledUsers)
                {
                    FailedRemovalObject failedUser = new FailedRemovalObject { Name = usr.Name, Location = usr.Location };
                    RemovalDataList.Add(failedUser);
                }

                MessageBox.Show("Some Objects Could not be removed.\n\nThey are being listed now.", "Something went wrong  :(", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool canConfirmRemovalCommand()
        {
            return selectedObjects.allObjects.DisabledComputers.Count > 0 || selectedObjects.allObjects.DisabledUsers.Count > 0;
        }
        #endregion
    }
}
