using ODSphereRouter.Services;
using ODSphereRouter.ViewModels;
using ODUtils.Commands;

namespace ODSphereRouter.Commands
{
    public sealed class NavigateCommand<TViewModel>(NavigationService<TViewModel> navigationService) : CommandBase where TViewModel : ViewModelBase
    {
        private readonly NavigationService<TViewModel> navigationService = navigationService;

        public override bool CanExecute(object? parameter)
        {
            return navigationService.Current is not TViewModel;
        }
        public override void Execute(object? parameter)
        {
            navigationService.Navigate();
        }
    }
}