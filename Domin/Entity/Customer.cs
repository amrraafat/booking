using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Entity
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public required string CustomerName { get; set; }
        public byte Gender { get; set; }
        public required string Address { get; set; }
        public string NationalId { get; set; }
        public byte[]? NationalIdImage { get; set; }
        public string MobileNumber { get; set; }
    }
}
