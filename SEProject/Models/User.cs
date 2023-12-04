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

    public User()
    {

    }
    public User(string username, string email, string password)
    {
        UserID = Guid.NewGuid();
        Username = username;
        Email = email;
        Salt = GenerateSalt();
        PasswordHash = SetPassword(password);
    }
    
    // Generates a random Salt
    public string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }
        Salt = Convert.ToBase64String(saltBytes);

        return Salt;
    }

    public string SetPassword(string password)
    {
        // Combine the password and salt, then hash
        using (var sha256 = SHA256.Create())
        {
            var combinedBytes = Encoding.UTF8.GetBytes(password + Salt);
            var hashBytes = sha256.ComputeHash(combinedBytes);
            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }
    }
    
    // Check if the provided password matches the stored hash
    public bool VerifyPassword(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            var combinedBytes = Encoding.UTF8.GetBytes(password + salt);
            var hashBytes = sha256.ComputeHash(combinedBytes);
            var enteredPasswordHash = Convert.ToBase64String(hashBytes);

            return PasswordHash == enteredPasswordHash;
        }
    }

    public void ToString()
    {
        Console.WriteLine("User ID: " + UserID);
        Console.WriteLine("Username: " + Username);
        Console.WriteLine("Email: " + Email);
        Console.WriteLine("Hashed Password: " + PasswordHash);
        Console.WriteLine("Generated Salt: " + Salt);
        Console.WriteLine();
    }
}