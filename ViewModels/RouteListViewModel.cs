using ODSphereRouter.Models;
using ODUtils.Extensions;

namespace ODSphereRouter.ViewModels
{
    public sealed class RouteListViewModel(RouteListItem item, StarSystem starSystem) : ViewModelBase
    {
        private readonly RouteListItem item = item;
        private readonly StarSystem starSystem = starSystem;
        public string Name => item.Name;
        public StarSystem StarSystem => starSystem;
        public string PowerState => starSystem.PowerState.GetDescription();
    }
}
