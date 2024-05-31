using IMS_WebApplication.Models;
using IMS_WebApplication.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Cryptography;
using System.Text;

namespace IMS_WebApplication.BO
{
    public class ClientBO
    {
        private readonly IClientRepository _clientRepository;

        public ClientBO(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public bool InsertClient(Client client)
        {
            bool flag = false;
            try
            {
                flag = _clientRepository.InsertClient(client);

            }
            catch (Exception)
            {                
                return flag = false;
            }
            return flag;
        }

        public IEnumerable<Client> GetAllClients()
        {
            return _clientRepository.GetAllClients();
        }
        public Client GetClientById(int clientId)
        {
            return _clientRepository.GetClientById(clientId);
        }

        public bool UpdateClient(Client client)
        {
            try
            {
                _clientRepository.UpdateClient(client);
                return true;

            }
            catch (DbUpdateException ex)
            {
                return false; // Return false indicating update failure
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
        public IEnumerable<ClientInvestment> GetInvestmentsByClientID(int clientId)
        {
            return _clientRepository.GetInvestmentsByClientID(clientId);           
        }

        public IEnumerable<Client> GetClientsByAgeAndGender(int age, string gender)
        {
            return _clientRepository.GetClientsByAgeAndGender(age, gender);
        }

        public IEnumerable<Client> GetClientInfo()
        {
            return _clientRepository.GetClientInfo();
        }

        //public bool CreateUser(string userName, string email, string password)
        //{
        //    // Generate a salt
        //    byte[] salt = GenerateSalt();

        //    // Hash the password with the salt
        //    byte[] passwordHash = HashPassword(password, salt);

        //    // Attempt to create a new user
        //    return _clientRepository.CreateUser(userName, email, password);
        //}

        //private byte[] HashPassword(string password, byte[] salt)
        //{
        //    // Hash the password with the salt using a secure hashing algorithm
        //    using (var sha256 = SHA256.Create())
        //    {
        //        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        //        byte[] combinedBytes = passwordBytes.Concat(salt).ToArray();
        //        return sha256.ComputeHash(combinedBytes);
        //    }
        //}

        //private byte[] GenerateSalt()
        //{
        //    // Generate a random salt
        //    byte[] salt = new byte[16];
        //    using (var rng = new RNGCryptoServiceProvider())
        //    {
        //        rng.GetBytes(salt);
        //    }
        //    return salt;
        //}

        public bool CreateUser(string username, string email, string Password, string confirmPassword)
        {
            if (Password != confirmPassword)
            {
                return false; // Passwords do not match
            }
            return _clientRepository.CreateUser(username, email, Password, confirmPassword);
        }
        public bool Login(string username, string password)
        {
            return _clientRepository.Login(username, password);
        }

        //public bool Login(string userName, string password)
        //{
        //    // Retrieve the user from the database based on the provided username
        //    var user = _clientRepository.GetUserByUsername(userName);

        //    // Check if the user exists
        //    if (user != null)
        //    {
        //        // Compare the entered password with the password stored in the database
        //        if (user.Password == password)
        //        {
        //            // Passwords match, authentication successful
        //            return true;
        //        }
        //    }

        //    // Either the user doesn't exist or the password is incorrect
        //    return false;
        //}

        public bool DeleteClient(int id)
        {
            try
            {
                return _clientRepository.DeleteClient(id);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Failed to delete client", ex);
            }
            
        }
    }
}
