using Domin.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infarstuructre.ViewModel
{
    public class HotelViewModel
    {

        public Hotel hotel { get; set; }
        public IFormFile? file { get; set; }
        public string? ImageFileName { get; set; }
    }
}
