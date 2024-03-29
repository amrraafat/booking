using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Entity
{
    public class Hotel
    {
        [Key]
        public int HotelId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "Requiredmessage")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "MaxLength")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "MinLength")]
        public string? HotelNameFL { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "Requiredmessage")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "MaxLength")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "MinLength")]
        public string? HotelNameSL { get; set; }

        [Range(1, 7, ErrorMessage = "Hotel rate must be between 1 and 7.")]
        [Required(ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "Requiredmessage")]
        public byte HotelRate { get; set; }

        
        public byte[]? HotelImage { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "Requiredmessage")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "MaxLength")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "MinLength")]
        public string HotelLocation { get; set; }
    }
}
