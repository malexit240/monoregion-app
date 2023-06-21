using Prism.Mvvm;
using Xamarin.Forms;

namespace Monoregion.App.Views
{
    public class BaseContentPage : ContentPage
    {
        public BaseContentPage()
        {
            ViewModelLocator.SetAutowireViewModel(this, true);
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}

