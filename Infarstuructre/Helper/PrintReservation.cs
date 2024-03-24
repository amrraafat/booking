using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infarstuructre.Helper
{
    public class PrintReservationModel
    {
        public DateTime ReservationDateTime { get; set; }
        public DateTime StartDate { get; set; }
        public string HotelName { get; set; }
        public string PackageName { get; set; }
        public string CustomerName { get; set; }
        public string MobileNumber { get; set; }
        public int AdultNo { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal AdultTotal { get; set; }
        public int KidNo { get; set; }
        public decimal KidPrice { get; set; }
        public decimal KidTotal { get; set; }
        public decimal TotalInvoice { get; set; }
        public decimal Paid { get; set; }
        public decimal Remain { get; set; }
        public string HotelLocation { get; set; }
    }
}
