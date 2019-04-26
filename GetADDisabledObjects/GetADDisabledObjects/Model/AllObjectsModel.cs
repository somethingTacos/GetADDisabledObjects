using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using PropertyChanged;

namespace GetADDisabledObjects.Model
{
    public class AllObjectsModel { }

    [AddINotifyPropertyChangedInterface]
    public class AllObjects : INotifyPropertyChanged
    {
        public string MainButtonText { get; set; } = "Get Disabled Users and Computers";
        public Visibility FetchingObjectsGifVisibility { get; set; } = Visibility.Hidden;

        private bool _SelectAllComps;
        public bool SelectAllComps
        {
            get { return _SelectAllComps; }
            set
            {
                _SelectAllComps = value;
                DisabledComputers.Select(x => x.IsSelected = _SelectAllComps).ToList();
                RaiseNonAutoPropertyChange("SelectAllComps");
            }
        }
        private bool _SelectAllUsers;
        public bool SelectAllUsers
        {
            get { return _SelectAllUsers; }
            set
            {
                _SelectAllUsers = value;
                DisabledUsers.Select(x => x.IsSelected = _SelectAllUsers).ToList();
                RaiseNonAutoPropertyChange("SelectAllUsers");
            }
        }
        public ObservableCollection<ComputerObject> DisabledComputers { get; set; } = new ObservableCollection<ComputerObject>();
        public ObservableCollection<UserObject> DisabledUsers { get; set; } = new ObservableCollection<UserObject>();


        //property change event handler for the non-auto properties 
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        protected void RaiseNonAutoPropertyChange(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

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
