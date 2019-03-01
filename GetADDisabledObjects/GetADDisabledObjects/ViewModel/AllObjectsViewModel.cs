using GetADDisabledObjects.Model;


namespace GetADDisabledObjects.ViewModel
{
    public class AllObjectsViewModel
    {
        public AllObjects AllDisabledObjects { get; set; }
        private NavigationViewModel _navigationViewModel { get; set; }

        public AllObjectsViewModel(NavigationViewModel navigationViewModel)
        {
            _navigationViewModel = navigationViewModel;
            initAllDisabledObjects();
        }

        public void initAllDisabledObjects()
        {
            AllObjects tempObjects = new AllObjects();

            AllDisabledObjects = tempObjects;
        }
    }
}
