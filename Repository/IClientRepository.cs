using IMS_WebApplication.Models;

namespace IMS_WebApplication.Repository
{
    public interface IClientRepository
    {
        public bool InsertClient(Client client);
        public bool UpdateClient(Client client);
        public IEnumerable<Client> GetAllClients();
        public Client GetClientById(int clientId);
        public IEnumerable<Client> GetClientsByAgeAndGender(int age, string gender);
        public IEnumerable<ClientInvestment> GetInvestmentsByClientID(int clientId);
        public IEnumerable<Client> GetClientInfo() ; 
        public void SaveClient();
        public bool CreateUser(string userName, string email, string Password, string confirmPasssword);
        //public bool CreateUser(Login user);
        public bool Login(string userName, string password);
        public bool DeleteClient(int clientId);
    }
}
