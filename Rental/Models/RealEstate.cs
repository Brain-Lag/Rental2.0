using System.ComponentModel.DataAnnotations;

namespace Rental.Models
{
    public class RealEstate
    {
        [Key] public int Code { get; set; }
        public string? Type { get; set; }
        public string? Address { get; set; }
        public double Square { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int OwnerID { get; set; }
        public decimal Pledge { get; set; }
    }
}
