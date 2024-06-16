using System.ComponentModel.DataAnnotations;

namespace Rental.Models
{
    public class Review
    {
        [Key] public int ReviewID { get; set; }
        public int Code { get; set; }
        public float Rating { get; set; }
        public string ReviewText { get; set; }
        public DateOnly ReviewDate { get; set; }
        public int TenantID { get; set; }
    }
}