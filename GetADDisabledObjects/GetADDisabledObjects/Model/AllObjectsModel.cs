using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace GetADDisabledObjects.Model
{
    public class AllObjectsModel { }

    [AddINotifyPropertyChangedInterface]
    public class AllObjects
    {
        public ObservableCollection<ComputerObject> DisabledComputers { get; set; } = new ObservableCollection<ComputerObject>();
        public ObservableCollection<UserObject> DisabledUsers { get; set; } = new ObservableCollection<UserObject>();
    }

    [AddINotifyPropertyChangedInterface]
    public class ComputerObject
    {
        public string Name { get; set; } = "";
        public string OU { get; set; } = "";
        public bool IsSelected { get; set; } = false;
    }

    [AddINotifyPropertyChangedInterface]
    public class UserObject
    {
        public string Name { get; set; } = "";
        public string OU { get; set; } = "";
        public bool IsSelected { get; set; } = false;
    }
}
