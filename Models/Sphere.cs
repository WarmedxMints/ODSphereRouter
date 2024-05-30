using ODSphereRouter.DTOs;

namespace ODSphereRouter.Models
{
    public sealed class Sphere
    {
        public string ControllingSystem { get; }

        public List<string> Systems { get; }

        public Sphere(string controllingSystem, List<string> systems)
        {
            ControllingSystem = controllingSystem;
            Systems = systems;
        }

        public Sphere(SphereDTO sphereDTO)
        {
            ControllingSystem = sphereDTO.Name;
            Systems = [.. sphereDTO.Systems.Split(",")];
            Systems.Sort(StringComparer.Ordinal);
        }
    }
}
