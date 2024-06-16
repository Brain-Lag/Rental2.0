using Rental.Models;
using Rental;
using System.Security.Cryptography;
using System.Text;
using Rental.Models;

namespace Rental.Services;

public class AccountService
{
    private readonly AppContextdb _context;

    public AccountService(AppContextdb context)
    {
        _context = context;
    }

    public async Task<bool> Register(User user)
    {
        var existingUser = _context.User.FirstOrDefault(a => a.UserName == user.UserName);

        if (existingUser != null)
            return false;

        var account = new User
        {
            UserRegDate = DateTime.UtcNow,
            UserName = user.UserName,
            UserEmail = user.UserEmail,
            UserPassword = EncryptPassword(user.UserPassword!),
            UserRole = "user"
        };

        await _context.User.AddAsync(account);
        await _context.SaveChangesAsync();

        return true;
    }

    public (bool, string) Login(User user)
    {
        var existingUser = _context.User.FirstOrDefault(u => u.UserName == user.UserName);

        if (existingUser == null)
            return (false, string.Empty);

        var encpass = existingUser.UserPassword!;
        if (encpass != existingUser.UserPassword)
            return (false, string.Empty);

        return (true, existingUser.UserID.ToString());
    }

    public string EncryptPassword(string password)
    {
        using (SHA512 sha512 = SHA512.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha512.ComputeHash(inputBytes);
            string hash = BitConverter.ToString(hashBytes);

            return hash;
        }
    }
}