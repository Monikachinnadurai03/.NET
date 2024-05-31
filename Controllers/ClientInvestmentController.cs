using IMS_WebApplication.BO;
using IMS_WebApplication.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS_WebApplication.Controllers
{
    public class ClientInvestmentController : Controller
    {
        private readonly ClientBO _clientBO = null;
        private readonly IClientRepository _clientRepository = null;

        public ClientInvestmentController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _clientBO = new ClientBO(clientRepository);
        }
        // GET: ClientInvestmentController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ClientInvestmentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClientInvestmentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClientInvestmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClientInvestmentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClientInvestmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClientInvestmentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClientInvestmentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


    }
}
