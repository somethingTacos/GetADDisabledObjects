using GetADDisabledObjects.Model;
using System.Collections.ObjectModel;

namespace GetADDisabledObjects.Model
{
    public class SelectedObjectRemovalModel { }

    public class SelectedObjects
    {
        public AllObjects allObjects { get; set; } = new AllObjects();
        public ObservableCollection<string> RemovalLog = new ObservableCollection<string>();
    }
}
