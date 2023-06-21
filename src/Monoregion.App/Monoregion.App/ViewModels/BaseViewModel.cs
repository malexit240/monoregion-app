using System;
using Prism.Mvvm;
using Prism.Navigation;

namespace Monoregion.App.ViewModels
{
    public abstract class BaseViewModel : BindableBase, IInitialize, INavigationAware, IDestructible
    {
        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        #region -- Public properties --

        protected INavigationService NavigationService { get; private set; }

        public virtual bool ShouldInvokeDefaultBackButtonImplementation => true;

        #endregion

        #region -- IInitialize implementation --

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        #endregion

        #region -- INavigationAware implementation --

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        #endregion

        #region -- IDestructible implementation --

        public virtual void Destroy()
        {
        }

        #endregion
    }
}
