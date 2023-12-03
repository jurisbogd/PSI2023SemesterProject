using System.Security.Cryptography;
using System.Text;

namespace SEProject.Models;

public class User
{
    public Guid UserID { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }

    public User(string pass)
    {
        SetPassword(pass);
    }
    
    public void SetPassword(string password)
    {
        // Generate a random salt
        byte[] saltBytes = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }
        Salt = Convert.ToBase64String(saltBytes);

        // Combine the password and salt, then hash
        using (var sha256 = SHA256.Create())
        {
            var combinedBytes = Encoding.UTF8.GetBytes(password + Salt);
            var hashBytes = sha256.ComputeHash(combinedBytes);
            PasswordHash = Convert.ToBase64String(hashBytes);
        }
    }
    
    // Check if the provided password matches the stored hash
    public bool VerifyPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var combinedBytes = Encoding.UTF8.GetBytes(password + Salt);
            var hashBytes = sha256.ComputeHash(combinedBytes);
            var enteredPasswordHash = Convert.ToBase64String(hashBytes);

            return PasswordHash == enteredPasswordHash;
        }
    }
}