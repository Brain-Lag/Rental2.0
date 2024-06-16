using System.ComponentModel.DataAnnotations;

namespace Rental.Models
{
    public class LeaseContract
    {
        [Key] public int ContractNumber { get; set; }
        public int Code { get; set; }
        public int OwnerID { get; set; }
        public int TenantID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime enddate { get; set; }
        public decimal Deposit { get; set; }
    }
}