using IMS_WebApplication.BO;
using IMS_WebApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Cryptography;
using System.Text;
using Login = IMS_WebApplication.Models.Login;

namespace IMS_WebApplication.Repository
{
    public class ClientRepository : IClientRepository
    {
           
        //The variable is going to hold the instance of the EfRefContext
        private readonly EfRef1Context _context;

        //Initializing the EfRefContext instance which it received as an argument
        public ClientRepository(EfRef1Context context)
        {
            _context = context;
        }

        public ClientRepository()
        {
            _context = new EfRef1Context();
        }
        public bool DeleteClient(int clientId)
        {
            var client = _context.Clients.Find(clientId);
            if (client == null)
                return false;

            _context.Clients.Remove(client);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Client> GetAllClients()
        {
            return _context.Clients.ToList();
        }

        public Client GetClientById(int clientId)
        {
            return _context.Clients.Find(clientId);
        }

        public bool InsertClient(Client client)
        {
            bool flag = false;
            try
            {
                _context.Clients.Add(client);
                SaveClient();
                return flag = true; // Return true if insertion succeeds
            }
            catch (Exception)
            {
                return flag = false; // Return false if an exception occurs during insertion
            }
        }

        public void SaveClient()
        {
            _context.SaveChanges();
        }

        public bool UpdateClient(Client client)
        {
            try
            {
                var existingClient = _context.Clients.Find(client.ClientId);
                if (existingClient != null)
                {
                    // Update the existing client properties
                    existingClient.ClientId = client.ClientId;
                    existingClient.FirstName = client.FirstName;
                    existingClient.LastName = client.LastName;
                    existingClient.Email = client.Email;
                    existingClient.PhoneNumber = client.PhoneNumber;
                    existingClient.City = client.City;
                    existingClient.State = client.State;
                    existingClient.PanCardNo = client.PanCardNo;
                    existingClient.Gender = client.Gender;
                    existingClient.Age = client.Age;

                    _context.Update(existingClient);
                    int rowsAffected = _context.SaveChanges(); // Save changes to the database
                    if (rowsAffected > 0)
                    {
                        return true; // Return true indicating successful update
                    } 
                    else
                    {
                        return false; // Return false indicating client ID not found
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update client.", ex);
            }
        }

        public IEnumerable<Client> GetClientsByAgeAndGender(int age, string gender)
        {
            return _context.Clients.Where(c => c.Age >= age && c.Gender == gender).ToList();
        }

        public IEnumerable<ClientInvestment> GetInvestmentsByClientID(int clientId)
        {
            var clientInvestments = _context.Clients
                            .Where(c => c.ClientId == clientId)
                            .SelectMany(c => c.ClientInvestments) // This navigates from Client to their Investments
                            .ToList();
            return (IEnumerable<ClientInvestment>)clientInvestments;
        }

        public IEnumerable<Client> GetClientInfo()
        {
            var clients = _context.Clients.Where(c => c.State == "TamilNadu" || c.City == "Chennai").ToList();
            return clients;
        }

        //public void CreateUser(string userName, string email, string Password, string confirmPasssword)
        //{
        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    // Generate a random salt
        //    byte[] PasswordSalt = GenerateSalt();

        //    // Hash the password using the salt
        //    byte[] hashedPassword = HashPasswordWithSalt(user.Password, PasswordSalt);

        //    // Convert byte arrays to base64 strings for storage
        //    string base64HashedPassword = Convert.ToBase64String(hashedPassword);
        //    string base64Salt = Convert.ToBase64String(PasswordSalt);

        //    // Store the hashed password and salt in the database
        //    user.Password = base64HashedPassword;
        //    user.PasswordSalt = base64Salt;

        //    _context.Add(user);
        //    _context.SaveChanges();
        //}

        //private byte[] GenerateSalt()
        //{
        //    byte[] salt = new byte[32];
        //    using (var rng = RandomNumberGenerator.Create())
        //    {
        //        rng.GetBytes(salt);
        //    }
        //    return salt;
        //}

        //private byte[] HashPasswordWithSalt(string password, byte[] salt)
        //{
        //    using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
        //    {
        //        return pbkdf2.GetBytes(32); // 256 bits
        //    }
        //}


        public bool CreateUser(string userName, string email, string Password, string confirmPassword)
        {
            // Generate a random salt
            byte[] salt = GenerateSalt();

            // Hash the password with the salt
            byte[] passwordHash = HashPassword(Password, salt);

            // Check if the username already exists
            var existingUser = _context.Logins.FirstOrDefault(u => u.UserName == userName);
            if (existingUser != null)
            {
                return false; // User already exists
            }

            // Create a new user entity
            var newUser = new Login
            {
                UserName = userName,
                Email = email,
                Password = Password,
                ConfirmPassword = confirmPassword,
                PasswordHash = passwordHash,
                PasswordSalt = salt

            };

            // Add the new user to the database
            _context.Logins.Add(newUser);
            _context.SaveChanges();

            return true; // User created successfully

        }

        private byte[] HashPassword(string Password, byte[] salt)
        {
            // Hash the password with the salt using a secure hashing algorithm
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes((Password));
                byte[] combinedBytes = passwordBytes.Concat(salt).ToArray();
                return sha256.ComputeHash(combinedBytes);
            }
        }

        private byte[] GenerateSalt()
        {
            // Generate a random salt
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public bool Login(string userName, string password)
        {
            var user = _context.Logins.FirstOrDefault(u => u.UserName == userName);

            if (user == null)
            {
                // User not found
                return false;
            }

            // Verify the password
            return VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
        }
        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            // Compute the hash of the provided password using the stored salt
            byte[] computedHash = HashPassword(password, storedSalt);
            // Compare the computed hash with the stored hash
            return storedHash.SequenceEqual(computedHash);
        }

       

        //public bool Login(string userName, string password)
        //{
        //    return _context.Logins.Any(u => u.UserName == userName && u.Password == password); // Note: You should compare hashed passwords
        //}

    }
}

