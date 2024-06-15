using Rental.Models;
using System;

namespace Rental.Services
{
    public class PropertyService
    {
        private readonly AppContextdb _context;

        public PropertyService(AppContextdb appContextdb)
        {
            _context = appContextdb;
        }
        public async Task<bool> AddPropertyAsync(RealEstate property)
        {
            if (property == null) return false;

            await _context.RealEstate.AddAsync(property);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddToFavoritesAsync(Favorite favorite)
        {
            if (favorite == null) return false;
            await _context.Favorite.AddAsync(favorite);
            await _context.SaveChangesAsync();
            return true ;
        }

        public async Task<bool> ClearFavoritesAsync(int userId)
        {
            var existingUser = await _context.User.FindAsync(userId);
            if(existingUser == null) return false;

            var userFavorites = _context.Favorite.Where(f => f.UserID == userId);
            _context.Favorite.RemoveRange(userFavorites);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditAsync(RealEstate property)
        {
            if (property == null) return false;

            _context.RealEstate.Update(property);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
