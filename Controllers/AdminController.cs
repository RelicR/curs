using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        ApplicationContext db;

        public AdminController(ILogger<AdminController> logger, ApplicationContext context)
        {
            db = context;
            _logger = logger;
        }

        public string GetPref(string? point)
        {
            var pref = "";
            switch (point)
            {
                case "Individ":
                    pref = "IND-";
                    break;
                case "Legal":
                    pref = "LEG-";
                    break;
                case "Client":
                    pref = "CLT-";
                    break;
                case "Employee":
                    pref = "EMP-";
                    break;
                case "Subcon":
                    pref = "SUB-";
                    break;
                default:
                    break;
            }
            return pref;
        }

        public async Task<bool[]> CheckIds(byte? iid, byte? lid)
        {
            bool[] err = new bool[2] { false, false };
            var ind = await db.Individuals.FirstOrDefaultAsync(i => i.Id == iid);
            var leg = await db.Legals.FirstOrDefaultAsync(l => l.Id == lid);
            if (ind == null && leg == null)
            {
                err[0] = true;
            }
            return err;
        }
        public async Task<bool[]> CheckIds(byte cid, byte eid, byte sid)
        {
            bool[] err = new bool[2] { false, false };
            var clt = await db.Clients.FirstOrDefaultAsync(c => c.Id == cid);
            var emp = await db.Employees.FirstOrDefaultAsync(e => e.Id == eid);
            var sub = await db.Subcontractors.FirstOrDefaultAsync(s => s.Id == sid);
            if (clt == null || emp == null || sub == null)
            {
                err[1] = true;
            }
            return err;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Администратор";
            return View();
        }

        public async Task<IActionResult> Individ(ViewModels viewmod, string? Id, bool? filter, string? surfilter, string? namefilter, string? midfilter)
        {
            ViewData["Title"] = "Физ. лица";
            viewmod.indivsList = await db.Individuals.ToListAsync();
            if (filter != null)
            {
                viewmod.indivsList = viewmod.indivsList.Where(i => i.Type == filter).ToList();
                TempData["filtered"] = filter is true ? "Показаны записи физ. лиц, зарегистрированных как ИП" : "Показаны записи физ. лиц, не зарегистрированных как ИП";
            }
            if (surfilter != null || namefilter != null || midfilter != null)
            {
                viewmod.indivsList = surfilter != null ? viewmod.indivsList.Where(i => i.Surname == surfilter).ToList() : viewmod.indivsList;
                viewmod.indivsList = namefilter != null ? viewmod.indivsList.Where(i => i.Name == namefilter).ToList() : viewmod.indivsList;
                viewmod.indivsList = midfilter != null ? viewmod.indivsList.Where(i => i.Midname == midfilter).ToList() : viewmod.indivsList;
            }
            TempData["message"] = TempData["message"] is null ? "Добавление записи" : TempData["message"];
            TempData["action"] = TempData["action"] is null ? "add" : TempData["action"];
            if (!String.IsNullOrEmpty(Id) && (string?)TempData["action"] == "add")
            {
                return RedirectToAction("Individ", "Admin", new { Id = "" });
            }
            else if (!String.IsNullOrEmpty(Id) && TempData["action"] != null)
            {
                viewmod.indiv = db.Individuals.Where(i => i.Id == Convert.ToByte(Id)).First();
                TempData["Id"] = Id;
            }
            else { viewmod.indiv = new Indiv(); }
            viewmod.Target = "Individ";
            return View(viewmod);
        }

        public async Task<IActionResult> Legal(ViewModels viewmod, string? Id, bool? filter)
        {
            ViewData["Title"] = "Юр. лица";
            viewmod.legalsList = await db.Legals.ToListAsync();
            if (filter != null)
            {
                viewmod.legalsList = viewmod.legalsList.Where(i => i.Type == filter).ToList();
                TempData["filtered"] = filter is true ? "Показаны записи коммерческих организаций" : "Показаны записи некоммерческих";
            }
            TempData["message"] = TempData["message"] is null ? "Добавление записи" : TempData["message"];
            TempData["action"] = TempData["action"] is null ? "add" : TempData["action"];
            if (!String.IsNullOrEmpty(Id) && (string?)TempData["action"] == "add")
            {
                return RedirectToAction("Legal", "Admin", new { Id = "" });
            }
            else if (!String.IsNullOrEmpty(Id) && TempData["action"] != null)
            {
                viewmod.legal = db.Legals.Where(i => i.Id == Convert.ToByte(Id)).First();
                TempData["Id"] = Id;
            }
            else { viewmod.legal = new Legal(); }
            viewmod.Target = "Legal";
            return View(viewmod);
        }

        

        public async Task<IActionResult> Client(ViewModels viewmod, string? Id, bool? ipFilter)
        {
            ViewData["Title"] = "Клиенты";
            var idk = db.Edits.FromSqlRaw("SELECT * FROM [Edits]");
            viewmod.clientsList = await db.Clients.ToListAsync();
            
            TempData["message"] = TempData["message"] is null ? "Добавление записи" : TempData["message"];
            TempData["action"] = TempData["action"] is null ? "add" : TempData["action"];
            if (!String.IsNullOrEmpty(Id) && (string?)TempData["action"] == "add")
            {
                return RedirectToAction("Client", "Admin", new { Id = "" });
            }
            else if (!String.IsNullOrEmpty(Id) && TempData["action"] != null)
            {
                viewmod.client = db.Clients.Where(i => i.Id == Convert.ToByte(Id)).First();
                TempData["Id"] = Id;
            }
            else { viewmod.client = new Client(); }
            viewmod.Target = "Client";
            return View(viewmod);
        }

        

        public async Task<IActionResult> Employee(ViewModels viewmod, string? Id, bool? ipFilter)
        {
            ViewData["Title"] = "Архитекторы";
            viewmod.emplsList = await db.Employees.ToListAsync();

            TempData["message"] = TempData["message"] is null ? "Добавление записи" : TempData["message"];
            TempData["action"] = TempData["action"] is null ? "add" : TempData["action"];
            if (!String.IsNullOrEmpty(Id) && (string?)TempData["action"] == "add")
            {
                return RedirectToAction("Employee", "Admin", new { Id = "" });
            }
            else if (!String.IsNullOrEmpty(Id) && TempData["action"] != null)
            {
                viewmod.empl = db.Employees.Where(i => i.Id == Convert.ToByte(Id)).First();
                TempData["Id"] = Id;
            }
            else { viewmod.empl = new Employee(); }
            viewmod.Target = "Employee";
            return View(viewmod);
        }

        public async Task<IActionResult> Subcon(ViewModels viewmod, string? Id, bool? ipFilter)
        {
            ViewData["Title"] = "Подрядчики";
            viewmod.subcsList = await db.Subcontractors.ToListAsync();

            TempData["message"] = TempData["message"] is null ? "Добавление записи" : TempData["message"];
            TempData["action"] = TempData["action"] is null ? "add" : TempData["action"];
            if (!String.IsNullOrEmpty(Id) && (string?)TempData["action"] == "add")
            {
                return RedirectToAction("Subcon", "Admin", new { Id = "" });
            }
            else if (!String.IsNullOrEmpty(Id) && TempData["action"] != null)
            {
                viewmod.subc = db.Subcontractors.Where(i => i.Id == Convert.ToByte(Id)).First();
                TempData["Id"] = Id;
            }
            else { viewmod.subc = new Subcon(); }
            viewmod.Target = "Subcon";
            return View(viewmod);
        }

        public async Task<IActionResult> Project(ViewModels viewmod, string? Id, DateTime? dFiltS, DateTime? dFiltE, double? bFiltS, double? bFiltE)
        {
            ViewData["Title"] = "Проекты";
            viewmod.projsList = await db.Projects.ToListAsync();
            if (dFiltS != null && dFiltE != null && dFiltS > dFiltE) { TempData["filtered"] += "Неверно задан фильтр даты. "; }
            else if (dFiltS != null || dFiltE != null)
            {
                dFiltS = dFiltS == null ? DateTime.MinValue : dFiltS;
                dFiltE = dFiltE == null ? DateTime.MaxValue : dFiltE;
                viewmod.projsList = viewmod.projsList.Where(p => p.StartDate >= dFiltS).Where(p => p.StartDate <= dFiltE).ToList();
                TempData["filtered"] += String.Format("Применён фильтр даты: с {0:dd.MM.yyyy} по {1:dd.MM.yyyy} ", dFiltS, dFiltE);
            }
            if (bFiltS != null && bFiltE != null && bFiltS > bFiltE) { TempData["filtered"] += "Неверно задан фильтр бюджета. "; }
            else if (bFiltS != null || bFiltE != null)
            {
                bFiltS = bFiltS == null ? 0 : bFiltS;
                bFiltE = bFiltE == null ? double.MaxValue : bFiltE;
                viewmod.projsList = viewmod.projsList.Where(p => p.Budget >= bFiltS).Where(p => p.Budget <= bFiltE).ToList();
                TempData["filtered"] += String.Format("Применён фильтр бюджета: от {0} до {1} ", bFiltS, bFiltE);
            }

            TempData["message"] = TempData["message"] is null ? "Добавление записи" : TempData["message"];
            TempData["action"] = TempData["action"] is null ? "add" : TempData["action"];
            if (!String.IsNullOrEmpty(Id) && (string?)TempData["action"] == "add")
            {
                return RedirectToAction("Project", "Admin", new { Id = "" });
            }
            else if (!String.IsNullOrEmpty(Id) && TempData["action"] != null)
            {
                viewmod.proj = db.Projects.Where(i => i.Id == Convert.ToByte(Id)).First();
                TempData["Id"] = Id;
            }
            else { viewmod.proj = new Project(); }
            viewmod.Target = "Project";
            return View(viewmod);
        }

        public async Task<IActionResult> Edit(ViewModels viewmod, string? Id, bool? ipFilter)
        {
            ViewData["Title"] = "Правки к проектам";
            viewmod.editsList = await db.Edits.ToListAsync();

            TempData["message"] = TempData["message"] is null ? "Добавление записи" : TempData["message"];
            TempData["action"] = TempData["action"] is null ? "add" : TempData["action"];
            if (!String.IsNullOrEmpty(Id) && (string?)TempData["action"] == "add")
            {
                return RedirectToAction("Edit", "Admin", new { Id = "" });
            }
            else if (!String.IsNullOrEmpty(Id) && TempData["action"] != null)
            {
                viewmod.edit = db.Edits.Where(i => i.Id == Convert.ToByte(Id)).First();
                viewmod.edit.EditDate = DateTime.Now;
                TempData["Id"] = Id;
            }
            else { viewmod.edit = new Edit(); }
            viewmod.Target = "Edit";
            return View(viewmod);
        }

        [HttpPost]
        public IActionResult ItemAction(IFormCollection form)
        {
            var btn = form.Keys.First().Split('-');
            var pref = GetPref(btn[1]);
            switch (btn[0])
            {
                case "e":
                    TempData["message"] = String.Format("Изменение записи для {0}{1:d3}", pref, Convert.ToByte(btn[2]));
                    TempData["action"] = "edit";
                    return RedirectToAction(btn[1], "Admin", new { Id = btn[2] });
                case "d":
                    TempData["message"] = String.Format("Подтвердите удаление записи для {0}{1:d3}", pref, Convert.ToByte(btn[2]));
                    TempData["action"] = "remove";
                    return RedirectToAction(btn[1], "Admin", new { Id = btn[2] });
                default:
                    return RedirectToAction("Index", "Admin");
            }
        }


        public async Task TreatRecord<T>(ViewModels viewmod, byte id, string action, string? pref) where T : class
        {
            T? tempRec = null;
            byte? Id = null;
            foreach (PropertyInfo propertyInfo in viewmod.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(T))
                {
                    tempRec = (T?)propertyInfo.GetValue(viewmod, null);
                    Id = Convert.ToByte(tempRec.GetType().GetProperty("Id").GetValue(tempRec, null));
                    break;
                }
            }

            if (tempRec == null) { TempData["message"] = "Возникла ошибка при обработке записи.\nПередана некорректная запись."; RedirectToAction(viewmod.Target); return; }
            if (id != Id) { TempData["message"] = "Отмена операции. Обнаружено несоответствие Id."; RedirectToAction(viewmod.Target); return; }
            switch (action)
            {
                case "Изменить":
                    db.Update<T>(tempRec);
                    TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, id)} успешно изменена.";
                    break;
                case "Удалить":
                    db.Remove<T>(db.Find<T>(id));
                    TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, id)} успешно удалена.";
                    break;
                case "Добавить":
                    if (db.Find<T>(id) != null)
                    {
                        TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, id)} не была добавлена.\nОбнаружено наложение на существующую запись.";
                        RedirectToAction(viewmod.Target);
                        break;
                    }
                    db.Add<T>(tempRec);
                    TempData["message"] = $"Запись успешно добавлена.";
                    break;
            }
            await db.SaveChangesAsync();
            if (typeof(T) == typeof(Project))
            {
                var tempQuery = @"[BDTesting].[dbo].[UpdateProjectMembers] {0}, {1}, {2}, {3}";
                List<SqlParameter> procParams = new List<SqlParameter>() {
                        new SqlParameter("@projId", Convert.ToByte(tempRec.GetType().GetProperty("Id").GetValue(tempRec, null))),
                        new SqlParameter("@clientId", viewmod.proj.ClientId),
                        new SqlParameter("@archId", viewmod.proj.EmployeeId),
                        new SqlParameter("@action", action)
                };
                db.Database.ExecuteSqlRaw(tempQuery, procParams);
            }
        }

        public async Task TreatEdit(ViewModels viewmod, string action)
        {
            if (viewmod.edit.Id == 0 || db.Projects.Find(viewmod.edit.Id) == null) { TempData["message"] = "Указан неверный Id Проекта."; RedirectToAction(viewmod.Target); return; }
            switch (action)
            {
                case "Удалить":
                    var tempProj = db.Projects.Find(viewmod.edit.Id);
                    tempProj.IsEdit = false;
                    db.Projects.Update(tempProj);
                    db.Edits.Remove(await db.Edits.FirstOrDefaultAsync(i => i.Id == viewmod.edit.Id));
                    TempData["message"] = $"Запись для {String.Format("{0:d3}", viewmod.edit.Id)} успешно удалена.";
                    break;
                default:
                    List<SqlParameter> procParams = new List<SqlParameter>() {
                        new SqlParameter("@projId", viewmod.edit.Id),
                        new SqlParameter("@editDesc", viewmod.edit.Description),
                        new SqlParameter("@editDate", viewmod.edit.EditDate.ToString("s")),
                        new SqlParameter("@editReason", viewmod.edit.Reason),
                        new SqlParameter{ ParameterName="@editBudget", SqlDbType=System.Data.SqlDbType.Money, Value=Convert.ToDecimal(viewmod.edit.EditedBudget) }
                    };
                    string tempQuery = "UpdateProjectEdits ";
                    foreach (var item in procParams)
                    {
                        if (item.SqlDbType.ToString() == "NVarChar") { tempQuery += $"{item.ParameterName}='{item.SqlValue}', "; }
                        else { tempQuery += $"{item.ParameterName}={(item.SqlValue == null ? "NULL" : item.SqlValue)}, "; }
                    }
                    db.Database.ExecuteSqlRaw(tempQuery.TrimEnd(' ').TrimEnd(','));
                    TempData["message"] = action == "Изменить" ? $"Запись для {String.Format("{0:d3}", viewmod.edit.Id)} успешно изменена." : "Запись успешно добавлена";
                    break;
            }
            await db.SaveChangesAsync();
        }

        [HttpPost]
        public async Task<IActionResult> AdminForm(ViewModels viewmod, string finput)
        {
            if (finput == "Отмена") { return RedirectToAction(viewmod.Target); }
            byte recId = 0;
            try { recId = finput == "Добавить" ? (byte)0 : Convert.ToByte(TempData["Id"]); }
            catch(Exception e) { TempData["message"] = "Передан неверный Id";  return RedirectToAction(viewmod.Target); }
            var pref = GetPref(viewmod.Target);
            bool invalidIds = false;
            if (finput != "Удалить")
            {
                switch (viewmod.Target)
                {
                    case "Individ":
                        viewmod.indiv.Id = recId;
                        break;

                    case "Legal":
                        viewmod.legal.Id = recId;
                        break;

                    case "Client":
                        viewmod.client.Id = recId;
                        var clType = viewmod.client.UID.Split('-')[0];
                        var clId = Convert.ToByte(viewmod.client.UID.Split('-')[1]);
                        viewmod.client.IndivId = clType == "IND" ? clId : null;
                        viewmod.client.LegalId = clType == "LEG" ? clId : null;
                        viewmod.client.UID = String.Format("{0}-{1:d3}", clType, clId);
                        invalidIds = (await CheckIds(viewmod.client.IndivId, viewmod.client.LegalId))[0];
                        break;

                    case "Employee":
                        viewmod.empl.Id = recId;
                        viewmod.empl.IndivId = Convert.ToByte(viewmod.empl.UID.Split('-')[1]);
                        viewmod.empl.UID = String.Format("{0}-{1:d3}", "IND", viewmod.empl.IndivId);
                        if (!viewmod.empl.Vac) { viewmod.empl.VacStart = viewmod.empl.VacEnd = null; }
                        else if (viewmod.empl.VacStart > viewmod.empl.VacEnd) { TempData["message"] = "Неверно указаны даты отпуска"; return RedirectToAction(viewmod.Target, "Admin"); }
                        invalidIds = (await CheckIds(viewmod.empl.IndivId, null))[0];
                        break;

                    case "Subcon":
                        viewmod.subc.Id = recId;
                        var subType = viewmod.subc.UID.Split('-')[0];
                        var subId = Convert.ToByte(viewmod.subc.UID.Split('-')[1]);
                        viewmod.subc.IndivId = subType == "IND" ? subId : null;
                        viewmod.subc.LegalId = subType == "LEG" ? subId : null;
                        viewmod.subc.UID = String.Format("{0}-{1:d3}", subType, subId);
                        invalidIds = (await CheckIds(viewmod.subc.IndivId, viewmod.subc.LegalId))[0];
                        break;

                    case "Project":
                        viewmod.proj.Id = recId;
                        var projClt = Convert.ToByte(viewmod.proj.ClientUID.Split('-')[1]);
                        var projEmp = Convert.ToByte(viewmod.proj.ArchUID.Split('-')[1]);
                        var projSub = Convert.ToByte(viewmod.proj.SubcUID.Split('-')[1]);
                        viewmod.proj.ClientId = projClt;
                        viewmod.proj.EmployeeId = projEmp;
                        viewmod.proj.SubconId = projSub;
                        viewmod.proj.ClientUID = String.Format("{0}-{1:d3}", "CLT", projClt);
                        viewmod.proj.ArchUID = String.Format("{0}-{1:d3}", "EMP", projEmp);
                        viewmod.proj.SubcUID = String.Format("{0}-{1:d3}", "SUB", projSub);
                        invalidIds = (await CheckIds(projClt, projEmp, projSub))[1];
                        break;

                    case "Edit":
                        viewmod.edit.Id = viewmod.edit.Id;
                        Console.WriteLine(viewmod.edit.EditedBudget);
                        break;
                }
            }
            if (invalidIds)
            {
                TempData["message"] = "Запись не была добавлена.\nУказан(-ы) недействительный(-ые) UID.";
                return RedirectToAction(viewmod.Target);
            }

            try
            {
                switch (viewmod.Target)
                {
                    case "Individ":
                        await TreatRecord<Indiv>(viewmod, recId, finput, pref);
                        break;
                    case "Legal":
                        await TreatRecord<Legal>(viewmod, recId, finput, pref);
                        break;
                    case "Client":
                        await TreatRecord<Client>(viewmod, recId, finput, pref);
                        break;
                    case "Employee":
                        await TreatRecord<Employee>(viewmod, recId, finput, pref);
                        break;
                    case "Subcon":
                        await TreatRecord<Subcon>(viewmod, recId, finput, pref);
                        break;
                    case "Project":
                        await TreatRecord<Project>(viewmod, recId, finput, pref);
                        break;
                    case "Edit":
                        await TreatEdit(viewmod, finput);
                        break;
                }
                return RedirectToAction(viewmod.Target);
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException.HResult == -2146232060)
                {
                    TempData["message"] = "Возникла ошибка при добавлении записи.\nНевозможно добавить две записи с идентичным UID в Базу Данных.";
                    return RedirectToAction(viewmod.Target);
                }
                TempData["message"] = $"Возникла ошибка при обращении к Базе Данных\n{e}";
                return RedirectToAction(viewmod.Target);
            }
        }
    }
}
