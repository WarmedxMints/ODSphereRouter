using ODSphereRouter.DTOs;

namespace ODSphereRouter.Models
{
    public sealed class RouteListItem
    {
        public RouteListItem(long address, string name)
        {
            Address = address;
            Name = name;
        }

        public RouteListItem(RouteListDTO dto)
        {
            Address = dto.Address;
            Name = dto.System;
        }

        public long Address { get; }
        public string Name { get; }
    }
}
