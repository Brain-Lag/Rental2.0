using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Rental.Models
{
    public class Favorite
    {
        [Key] public int FavoriteID { get; set; }
        public int Code { get; set; }
        public int UserID { get; set; }
    }
}
