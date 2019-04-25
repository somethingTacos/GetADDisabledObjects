using System.Collections.ObjectModel;
using PropertyChanged;

namespace GetADDisabledObjects.Model
{
    public class AllObjectsModel { }

    [AddINotifyPropertyChangedInterface]
    public class AllObjects
    {
        public string MainButtonText { get; set; } = "Get Disabled Users and Computers";
        public bool SelectAllComps { get; set; } = false;
        public bool SelectAllUsers { get; set; } = false;
        public ObservableCollection<ComputerObject> DisabledComputers { get; set; } = new ObservableCollection<ComputerObject>();
        public ObservableCollection<UserObject> DisabledUsers { get; set; } = new ObservableCollection<UserObject>();
    }

    [AddINotifyPropertyChangedInterface]
    public class ComputerObject
    {
        public string Name { get; set; } = "";
        public string SamAccountName { get; set; } = "";
        public string Location { get; set; } = "";
        public bool IsSelected { get; set; } = false;
    }

    [AddINotifyPropertyChangedInterface]
    public class UserObject
    {
        public string Name { get; set; } = "";
        public string SamAccountName { get; set; } = "";

        public string Location { get; set; } = "";
        public bool IsSelected { get; set; } = false;
    }

    [AddINotifyPropertyChangedInterface]
    public class FailedRemovalObject
    {
        public string Name { get; set; } = "";

        public string Location { get; set; } = "";
    }
}
