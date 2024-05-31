using IMS_WebApplication.BO;
using IMS_WebApplication.Models;
using IMS_WebApplication.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IMS_WebApplication.Controllers
{
    //[Route("api/[controller]")]
    [Route("api")]
    [ApiController]
    public class Client_Controller : ControllerBase
    {
        private readonly ClientBO _clientBO = null;
        private readonly IClientRepository _clientRepository = null;

        public Client_Controller(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _clientBO = new ClientBO(clientRepository);
        }

        // GET: ClientController
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            // Ideally, you return a data object or result, not a view
            return Ok("Welcome to the API!");
        }

        // GET: ClientController/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            try
            {
                var initialData = new
                {
                    client = new List<string>
                    {
                        "ClientId",
                        "FirstName",
                        "LastName",
                        "Email",
                        "City",
                        "State",
                        "PhoneNumber",
                        "PanCardNo",
                        "Gender",
                        "Age",
                        "Password"
                    },
                    InitialSetupInfo = "Initial client creation info"
                };

                return Ok(initialData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Return 500 Internal Server Error with exception message
            }
        }

        // POST: ClientController/Create
        [HttpPost("Create")]
        public IActionResult Create([FromBody] Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns 400 Bad Request with model state errors
            }

            try
            {
                bool flag = _clientBO.InsertClient(client); 
                if (flag)
                {
                    return Ok("Client Added Successfully"); // Returns 200 OK with success message
                }
                else
                {
                    return StatusCode(500, "Client Failed to add"); // Returns 500 Internal Server Error
                }
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, ex.Message); // Returns 500 Internal Server Error with exception message
            }
        }

        [HttpGet("Details/{id}")]
        public IActionResult Details(int id)
        {
            var client = _clientBO.GetClientById(id);
            if (client == null)
            {
                return NotFound(new { message = "Client not found" });
            }
            return Ok(client);
        }

        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return BadRequest("Client id must be greater than zero.");
            }
            if (id < 0)
            {
                return BadRequest($"{id} is invalid.");
            }

            try
            {
                Client client = _clientBO.GetClientById(id);
                if (client != null)
                {
                    return Ok(client);
                }
                else
                {
                    return NotFound($"Client Details not found with id: {id}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving client details: {ex.Message}");
            }
        }


        // POST: /Client/Edit/id
        [HttpPost("Edit/{id}")]
        public IActionResult Edit([FromBody] Client client)
        {
            if (client == null)
            {
                return BadRequest("Invalid client data.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool isUpdated = _clientBO.UpdateClient(client);
                if (isUpdated)
                {
                    return Ok("Client Updated Successfully");
                }
                else
                {
                    return StatusCode(500, "Failed to update client.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the client: {ex.Message}");
            }
        }

        // GET: /Client/ClientList
        [HttpGet("ClientList")]
        public IActionResult ClientList()
        {
            try
            {
                // Retrieve the list of clients from the data source
                ICollection<Client> clients = (ICollection<Client>)_clientBO.GetAllClients();

                // Return the list of clients as JSON
                if (clients != null && clients.Count > 0)
                {
                    return Ok(clients);
                }
                else
                {
                    return NotFound("No clients found.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, $"An error occurred while retrieving clients: {ex.Message}");
            }
        }


        // GET api/ClientInvestment/5
        [HttpGet("ClientInvestment/{clientId}")]
        public ActionResult<IEnumerable<ClientInvestment>> GetClientInvestments(int clientId)
        {
            var clientInvestments = _clientBO.GetInvestmentsByClientID(clientId);

            if (clientInvestments == null || !clientInvestments.Any())
            {
                return NotFound("No investments found for the given client ID.");
            }

            return Ok(clientInvestments);
        }

        [HttpGet("ClientsByAgeAndGender")]
        public ActionResult<IEnumerable<Client>> GetClientsByAgeAndGender([FromQuery] int age, [FromQuery] string gender)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var clients = _clientBO.GetClientsByAgeAndGender(age, gender);
                if (clients == null || !clients.Any())
                {
                    return NotFound($"No clients found matching age: {age} and gender: {gender}.");
                }
                return Ok(clients);
            }
            catch (Exception ex)
            {
                // Log the exception details here to help with debugging
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        //GET: api/ClientInfo
        [HttpGet("ClientInfo")]
        public ActionResult<IEnumerable<Client>> GetClientInfo()
        {
            var clients = _clientBO.GetClientInfo();  // Fetch data using the business layer.
            if (clients == null || !clients.Any())
            {
                return NotFound("No client found matching the provided criteria.");
            }
            return Ok(clients);
        }

    }
}
