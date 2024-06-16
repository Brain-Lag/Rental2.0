using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rental.Models
{
    public class Owner
    {
        [Key] public int OwnerID { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhoneNumber { get; set; }
        public string OwnerRequisites { get; set; }
        public int UserID { get; set; }
    }
}