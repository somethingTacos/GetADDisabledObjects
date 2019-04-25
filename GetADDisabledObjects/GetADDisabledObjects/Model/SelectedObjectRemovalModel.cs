using System.Windows;
using PropertyChanged;

namespace GetADDisabledObjects.Model
{
    public class SelectedObjectRemovalModel { }

    [AddINotifyPropertyChangedInterface]
    public class SelectedObjects
    {
        public string MainButtonText { get; set; } = "Confirm Removal";
        public AllObjects allObjects { get; set; } = new AllObjects();

        public string OperationCompleteButtonText { get; set; } = "Cancel";
        public Visibility RemovalButtonVisibility { get; set; } = Visibility.Visible;
    }
}
