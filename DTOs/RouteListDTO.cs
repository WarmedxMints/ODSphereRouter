using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODSphereRouter.DTOs
{
    public sealed class RouteListDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Address { get; set; }
        public required string System { get; set; }    
    }
}