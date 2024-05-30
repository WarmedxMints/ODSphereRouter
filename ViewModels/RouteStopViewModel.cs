using ODSphereRouter.Models;
using ODUtils.Extensions;
using ODUtils.Models;

namespace ODSphereRouter.ViewModels
{
    public class RouteStopViewModel(StarSystem starSystem, StarSystem? prevSystem = null) : ViewModelBase
    {
        private readonly StarSystem starSystem = starSystem;
        private readonly StarSystem? prevSystem = prevSystem;

        public string Name => starSystem.Name ?? "Unknown";
        public string Powers => starSystem.Powers.GetDescription();
        public string PowerState => starSystem.PowerState.GetDescription();
        public string JumpDistance => prevSystem == null ? string.Empty : $"{Distance:N2} ly";
        public double Distance => prevSystem == null ? 0 : starSystem.Pos.DistanceFrom(prevSystem.Pos);
        public Position Pos => starSystem.Pos * 2;
    }
}
