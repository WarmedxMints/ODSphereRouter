using ODSphereRouter.Models;
using ODUtils.Extensions;

namespace ODSphereRouter.ViewModels
{
    public sealed class SphereSystemViewModel(StarSystem system) : ViewModelBase
    {
        public StarSystem StarSystem { get; } = system;
        public string Name => StarSystem.Name;
        public string Powers => StarSystem.Powers.GetDescription();
        public string PowerState => StarSystem.PowerState.GetDescription();
        public string Population => StarSystem.Population.FormatNumber();
    }
}