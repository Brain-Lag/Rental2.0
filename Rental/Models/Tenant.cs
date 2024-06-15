using System.ComponentModel.DataAnnotations;

namespace Rental.Models
{
    public class Tenant
    {
        [Key] public int TenantID { get; set; }
        public string TenantName { get; set; }
        public string TenantPhoneNumber { get; set; }
        public int UserID { get; set; }
    }
}
