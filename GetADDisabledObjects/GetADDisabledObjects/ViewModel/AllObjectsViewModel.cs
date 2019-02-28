using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetADDisabledObjects.ViewModel
{
    public class AllObjectsViewModel
    {
        private NavigationViewModel _navigationViewModel { get; set; }

        public AllObjectsViewModel(NavigationViewModel navigationViewModel)
        {
            _navigationViewModel = navigationViewModel;

        }
    }
}
