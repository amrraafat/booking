using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Entity
{
    public class Package
    {
        [Key]
        public int PackageId { get; set; }
        public required string  PackageName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set;}
        public decimal AdultPrice { get; set; }
        public decimal KidPrice { get; set; }
        public int HotelId { get; set; }
    }
}
