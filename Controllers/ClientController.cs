using IMS_WebApplication.BO;
using IMS_WebApplication.Models;
using IMS_WebApplication.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IMS_WebApplication.Controllers
{
    public class ClientController : Controller
    {
        private readonly ClientBO _clientBO;
        private readonly IClientRepository _clientRepository;
        private readonly object _authService;

        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _clientBO = new ClientBO(clientRepository);
        }
        // GET: ClientController
        public IActionResult Index()
        {
            return View("Views/Client/Index.cshtml");
        }

        // GET: ClientController/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool flag = _clientBO.InsertClient(client);
                    if (flag)
                    {
                        TempData["Response"] = "Client Added Successfully";
                        TempData["ResponseClass"] = "success";
                    }
                    else
                    {
                        TempData["Response"] = "Failed to insert client Details...";
                        TempData["ResponseClass"] = "error";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Response"] = "An error occurred while inserting the client.";
                    TempData["ResponseClass"] = "error";
                }
            }

            // Regardless of the result, return the view with the client object
            return View(client);
        }


        

        //GET: ClientController/Details
        [HttpGet]
        public IActionResult Details()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(int id)
        {
            Client client = _clientBO.GetClientById(id);
            if (client == null)
            {
                ViewBag.ErrorMessage = "Client not found";
                return View();
            }
            return View("ClientDetails", client);

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return Content("Client id must be greater than zero ");
            }
            if (id < 0)
            {
                return Content($"{id} is invalid.");
            }

            try
            {
                Client client = _clientBO.GetClientById(id);
                if (client != null)
                {
                    return View(client);
                }
                TempData["errorMessage"] = $"Client Details not found with id: {id}";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Client client)
        {
            if (!ModelState.IsValid)
            {
                // If model state is not valid, return to the edit page with validation errors
                return View(client);
            }

            try
            {
                // Update the client details in the database
                bool isUpdated = _clientBO.UpdateClient(client);

                if (isUpdated)
                {
                    // If updated successfully, set success message and redirect to a success page
                    TempData["Response"] = "Client Updated Successfully";
                    TempData["ResponseClass"] = "success";                  
                    return View(client);
                }
                else
                {
                    TempData["Response"] = "Failed to update client...";
                    TempData["ResponseClass"] = "error";
                    return View(client);
                }
            }
            catch (Exception ex)
            {
                TempData["Response"] = "An error occurred while updating the client.";
                TempData["ResponseClass"] = "error";
                return View(client);
            }
        }
                
        public IActionResult ClientList()
        {
            // Retrieve the list of clients from the data source
            ICollection<Client> clients = (ICollection<Client>)_clientBO.GetAllClients();

            ViewData["Response"] = TempData["Response"];
            ViewData["ResponseClass"] = TempData["ResponseClass"];
            // Pass the list of clients to the view
            return View("Views/Client/ClientList.cshtml", clients);
        }

        [HttpGet]
        public IActionResult ClientInvestment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClientInvestment(int clientId)
        {
            var clientInvestments = _clientBO.GetInvestmentsByClientID(clientId);

            if (clientInvestments == null || !clientInvestments.Any())
            {
                ViewBag.ErrorMessage = "No investments found for the given client ID.";
                return View(new List<ClientInvestment>()); // Return an empty list to the view
            }

            return View(clientInvestments); // Pass the list to the view
        }

        [HttpGet]
        public IActionResult ClientsByAgeAndGender()
        {
            return View(new List<Client>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClientsByAgeAndGender(int age, string gender)
        {
            if (!ModelState.IsValid)
            {
                return View(new List<Client>());  // Return to view to display validation errors
            }

            var clients = _clientBO.GetClientsByAgeAndGender(age, gender);
            if (clients == null || !clients.Any())
            {
                ViewBag.ErrorMessage = "No clients found matching the criteria.";
                return View(new List<Client>());
            }
            return View(clients);
        }

        [HttpGet]
        public IActionResult GetClientInfo()
        {
            var clients = _clientBO.GetClientInfo();  // Fetch data using the business layer.
            if (clients == null || !clients.Any())
            {
                ViewBag.ErrorMessage = "No client found matching the provided criteria.";
                return View("ErrorView"); // Make sure this view exists or handle the situation appropriately.
            }
            return View(clients);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(Login model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to create a new user
                if (_clientBO.CreateUser(model.UserName, model.Email, model.Password, model.ConfirmPassword))
                {
                    // Redirect to login page after successful sign up
                    //return RedirectToAction("Login", "Client");
                    return RedirectToAction("Login", "Client");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to sign up. Username may already be taken.");
                }
            }
            return View(model);
            //return View("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                // Check if the user exists and the password is correct
                if (_clientBO.Login(model.UserName, model.Password))
                {
                    // Redirect authenticated user to dashboard
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Add model error for invalid credentials
                    ModelState.AddModelError("", "Invalid username or password.");
                    //return View(model);
                }
            }
            // If model state is invalid, return to the login view with validation errors
            return View(model);
        }


        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ResetPassword(IFormCollection collection)
        {
            string password1 = collection["password"].ToString();
            string password2 = collection["password"].ToString();

            if (password1 == password2)
            {
                HttpContext.Session.SetString("Password", password1);
                ViewBag.PasswordChanged = true;
            }
            else
            {
                ViewBag.PasswordChanged = false;
            }
            return View("Login");
        }

        public IActionResult Logout()
        {
            // Sign out the user
            HttpContext.SignOutAsync();

            // Redirect to the login page
            return View("Login", "Client");
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                // Retrieve the client from the database using the provided ID
                var client = _clientBO.GetClientById(id);
                if (client == null)
                {
                    // If the client is not found, return NotFound
                    return NotFound();
                }

                // Pass the client data to the view for display
                return View(client);
            }
            catch (Exception ex)
            {
                // Log the exception and return an error message
                TempData["Response"] = $"An error occurred while retrieving client details: {ex.Message}";
                TempData["ResponseClass"] = "error";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                // Retrieve the client from the database using the provided ID
                var client = _clientBO.GetClientById(id);
                if (client == null)
                {
                    // If the client is not found, return NotFound
                    return NotFound();
                }

                // Delete the client from the database
                _clientBO.DeleteClient(id);

                // Optionally, you can add a success message here if needed
                TempData["Response"] = "Client deleted successfully";
                TempData["ResponseClass"] = "success";

                // Redirect to the client list page after successful deletion
                return RedirectToAction("ClientList");
            }
            catch (Exception ex)
            {
                // Log the exception and return an error message
                TempData["Response"] = $"An error occurred while deleting the client: {ex.Message}";
                TempData["ResponseClass"] = "error";
                return RedirectToAction("Delete", new { id = id });
            }
        }
    }
}
