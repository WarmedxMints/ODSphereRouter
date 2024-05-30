using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODSphereRouter.DTOs
{
    public sealed class StarSystemDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Address { get; set; }
        public string Name { get; set; } = string.Empty;
        public long Population { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public int Powers { get; set; }
        public int PowerState { get; set; }

        public bool CheckSystemUpdated(StarSystemDTO other)
        {
            if (Address != other.Address)
                return false;

            return Powers != other.Powers || PowerState != other.PowerState;
        }

        public StarSystemDTO() { }

        public StarSystemDTO(StarSystemDTO matched, StarSystemDTO newItem)
        {
            Powers = newItem.Powers;
            PowerState = newItem.PowerState;
        }
    }
}
