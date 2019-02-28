using System.Windows;
using GetADDisabledObjects.ViewModel;
namespace GetADDisabledObjects
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewmodel = new NavigationViewModel();
            viewmodel.SelectedViewModel = new AllObjectsViewModel(viewmodel);
            this.DataContext = viewmodel;
        }
    }
}
