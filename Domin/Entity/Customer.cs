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
        public string CustomerName { get; set; }
        public byte Gender { get; set; }
        public string Address { get; set; }
        public int NationalId { get; set; }
        public string NationalIdImage { get; set; }
    }
}
