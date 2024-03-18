using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Entity
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }
        public int EmployeeId { get; set; }
        public int HotelId { get; set; }
        public int PackageId { get; set; }
        public int CustomerId { get; set; }
        public int AdultNo { get; set; }
        public int KidNo { get; set; }
        public DateTime? ReservationDateTime { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Paid { get; set; }
        public decimal Remain { get; set; }
        public DateTime? LastModify { get; set; } 
        public string? UserName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string DeleteReason { get; set; }
    }
}
