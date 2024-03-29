using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Entity
{
  public  class VwUser
    {
        public required string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "Requiredmessage")]
        public required string Name { get; set; }
        public string? ImageUser { get; set; }
        public bool ActiveUser { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "Requiredmessage")]
        public required string Role { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "Requiredmessage")]
        public required string Email { get; set; }
    }
}
