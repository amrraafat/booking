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
        public string? HotelNameFL { get; set; }
        public string? HotelNameSL { get; set; }
        public byte HotelRate { get; set; }
        public byte[]? HotelImage { get; set; }
        public string HotelLocation { get; set; }
    }
}
