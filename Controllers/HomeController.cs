using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ApplicationContext db;
        public Person pers = new Person();

        
        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            db = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            TempData["message"] = TempData["message"] ?? "Выберете точку входа.";
            return View(new Person());
        }
        [HttpPost]
        public async Task<IActionResult> Index(Person user, string role, string? UID)
        {
            if (UID != "admin" && UID != null && (Convert.ToInt16(UID.Split('-')[1]) < 0 || Convert.ToInt16(UID.Split('-')[1]) > 255))
            {
                TempData["message"] = "Указан недействительный UID";
                return View(new Person());
            }
            byte id = 0;
            switch (role)
            {
                case "Администратор":
                    return RedirectToAction("Index", "Admin");
                case "Клиент":
                    if (UID == null) { TempData["message"] = "Не указан UID"; return View(user); }
                    id = Convert.ToByte(UID.Split('-')[1]);
                    if (!UID.StartsWith("CLT") || db.Clients.FirstOrDefault(c => c.Id == id) == null)
                    {
                        TempData["message"] = "Указан недействительный UID"; 
                        return View(user);
                    }
                    ClientModels ctemp = new ClientModels();
                    user.Id = id;
                    ctemp.User = user;
                    ctemp.Client = db.Clients.First(c => c.Id == id);
                    ctemp.Indiv = ctemp.Client.IndivId != null ? db.Individuals.Find(ctemp.Client.IndivId) : null;
                    ctemp.Legal = ctemp.Client.LegalId != null ? db.Legals.Find(ctemp.Client.LegalId) : null;
                    ctemp.ProjsList = db.Projects.Where(p => p.ClientId == id).ToList();
                    ctemp.EditsList = db.Edits.Where(e => ctemp.Client.Orders.Contains(" " + e.Id.ToString() + ",")).ToList();
                    ViewData["Title"] = $"Клиент";
                    return View("Client", ctemp);
                case "Архитектор":
                    if (UID == null) { TempData["message"] = "Не указан UID"; return View(user); }
                    id = Convert.ToByte(UID.Split('-')[1]);
                    if (!UID.StartsWith("EMP") || db.Employees.FirstOrDefault(e => e.Id == id) == null)
                    {
                        TempData["message"] = "Указан недействительный UID";
                        return View(user);
                    }
                    EmplModels etemp = new EmplModels();
                    user.Id = id;
                    etemp.User = user;
                    etemp.Employee = db.Employees.First(e => e.Id == id);
                    etemp.Indiv = db.Individuals.Find(etemp.Employee.IndivId);
                    etemp.ProjsList = db.Projects.Where(p => p.EmployeeId == id).ToList();
                    etemp.EditsList = db.Edits.Where(e => etemp.Employee.Projects.Contains(" "+e.Id.ToString()+",")).ToList();
                    ViewData["Title"] = $"Архитектор";
                    return View("Employee", etemp);
                default:
                    return View(user);
            }
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}