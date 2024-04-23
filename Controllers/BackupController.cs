using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class BackupController : Controller
    {
        private readonly ILogger<BackupController> _logger;
        ApplicationContext db;
        Object[] tables;
        //Dictionary<string, DbSet> atables;
        public BackupController(ILogger<BackupController> logger, ApplicationContext context)
        {
            db = context;
            _logger = logger;
            tables = new Object[] { db.Individuals, db.Legals, db.Clients, db.Employees, db.Subcontractors, db.Projects, db.Edits };
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


        public IActionResult ClInd()
        {
            return PartialView();
        }

        public IActionResult ClLeg()
        {
            return PartialView();
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
        public async Task<IActionResult> Index(ViewModels viewmod, string? Id, bool? ipFilter)
        {
            viewmod.clientsList = await db.Clients.ToListAsync();
            TempData["message"] = TempData["message"] is null ? "Добавление записи" : TempData["message"];
            TempData["action"] = TempData["action"] is null ? "add" : TempData["action"];
            if (!String.IsNullOrEmpty(Id) && (string?)TempData["action"] == "add")
            {
                return RedirectToAction("Test1", "Home", new { Id = "" });
            }
            else if (!String.IsNullOrEmpty(Id) && TempData["action"] != null)
            {
                viewmod.client = db.Clients.Where(i => i.Id == Convert.ToByte(Id)).First();
                TempData["Id"] = Id;
                //TempData["pers"] = $"{viewcl.client.Name} {viewcl.client.Surname}";
            }
            else { viewmod.client = new Client(); }
            viewmod.clientsList = await db.Clients.ToListAsync();
            viewmod.Target = "Index";
            //switch (ipFilter)
            //{
            //    case null:
            //        viewcl.clientsList = await db.Clients.ToListAsync();
            //        break;
            //    default:
            //        TempData["filtered"] = ipFilter is true ? "Показаны записи физ. лиц, зарегистрированных как ИП" : "Показаны записи физ. лиц, не зарегистрированных как ИП";
            //        viewcl.clientsList = await db.Clients.Where(i => i.Orders == ipFilter).ToListAsync();
            //        break;
            //}
            //TempData["message"] = TempData["message"] is null ? "Добавление записи" : TempData["message"];
            //TempData["action"] = TempData["action"] is null ? "add" : TempData["action"];
            return View(viewmod);
        }

        public async Task<IActionResult> Client(ViewModels viewmod, string? Id, bool? ipFilter)
        {
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
                //TempData["pers"] = $"{viewcl.client.Name} {viewcl.client.Surname}";
            }
            else { viewmod.client = new Client(); }
            viewmod.clientsList = await db.Clients.ToListAsync();
            viewmod.Target = "Client";
            //switch (ipFilter)
            //{
            //    case null:
            //        viewcl.clientsList = await db.Clients.ToListAsync();
            //        break;
            //    default:
            //        TempData["filtered"] = ipFilter is true ? "Показаны записи физ. лиц, зарегистрированных как ИП" : "Показаны записи физ. лиц, не зарегистрированных как ИП";
            //        viewcl.clientsList = await db.Clients.Where(i => i.Orders == ipFilter).ToListAsync();
            //        break;
            //}
            //TempData["message"] = TempData["message"] is null ? "Добавление записи" : TempData["message"];
            //TempData["action"] = TempData["action"] is null ? "add" : TempData["action"];
            return View(viewmod);
        }





        [HttpPost]
        public IActionResult Edit(IFormCollection form)
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

        public async Task<bool> CheckIds(byte? iid, byte? lid)
        {
            var opt1 = await db.Individuals.FirstOrDefaultAsync(i => i.Id == iid);
            var opt2 = await db.Legals.FirstOrDefaultAsync(l => l.Id == lid);
            if (opt1 == null && opt2 == null)
            {
                //TempData["message"] = "Запись не была добавлена.\nУказан недействительный UID.";
                return true;
            }
            return false;
        }

        //public async Task<DbSet> GetTable

        //public async Task<IActionResult> ProceedDb(ViewModels models, string input, DbSet<Indiv>? tI, DbSet<Legal>? tL, DbSet<Client>? tC, DbSet<Employee>? tE, DbSet<Subcon>? tS, DbSet<Project>? tP, DbSet<Edit>? tEd)
        //{
        //    CheckNotNy
        //    tL.GetType() idk = 
        //    table = 
        //    try
        //    {
        //        switch (input)
        //        {
        //            case "Отмена":
        //                return RedirectToAction(TempData["target"].ToString());
        //            case "Изменить":
        //                tarDb.Update();
        //                db.Clients.Update(viewcl.client);
        //                await db.SaveChangesAsync();
        //                TempData["message"] = $"Запись для {String.Format("CLT-{0:d3}", viewcl.client.Id)} успешно изменена.";
        //                return RedirectToAction(TempData["target"].ToString());
        //            case "Удалить":
        //                db.Clients.Remove(await db.Clients.FirstOrDefaultAsync(i => i.Id == Convert.ToByte(TempData["Id"])));
        //                await db.SaveChangesAsync();
        //                TempData["message"] = $"Запись для {String.Format("CLT-{0:d3}", TempData["uid"])} успешно удалена.";
        //                return RedirectToAction(TempData["target"].ToString());
        //            case "Добавить":
        //                if (await db.Clients.FirstOrDefaultAsync(i => i.Id == viewcl.client.Id) != null)
        //                {
        //                    TempData["message"] = $"Запись для {String.Format("CLT-{0:d3}", viewcl.client.Id)} не была добавлена.\nОбнаружено наложение на существующую запись.";
        //                    return RedirectToAction(TempData["target"].ToString());
        //                }
        //                db.Clients.Add(viewcl.client);
        //                await db.SaveChangesAsync();
        //                TempData["message"] = $"Запись для {String.Format("CLT-{0:d3}", viewcl.client.Id)} успешно добавлена.";
        //                return RedirectToAction(TempData["target"].ToString());
        //        }
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        if (e.InnerException.HResult == -2146232060)
        //        {
        //            TempData["message"] = $"Возникла ошибка при добавлении записи.\nНевозможно добавить две записи с идентичным UID в базу \"Клиенты\".";
        //            return RedirectToAction("Index");
        //        }
        //        TempData["message"] = $"Возникла ошибка при обращении к Базе Данных\n{e}";
        //        return RedirectToAction("Index");
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        TempData["message"] = $"Запись для {String.Format("CLT-{0:d3}", viewcl.client.Id)} не была добавлена.\nФорма была заполнена некорректно."; ;
        //        return View(viewcl.client);
        //    }
        //    return
        //}


        [HttpPost]
        public async Task<IActionResult> ClientForm(ViewModels viewmod, string finput)
        {
            if (finput == "Отмена") { return RedirectToAction(viewmod.Target); }
            var pref = GetPref(viewmod.Target);
            //pref = "CLT-";
            bool errIds = false;
            switch (viewmod.Target)
            {
                case "Individ":
                    viewmod.indiv.Id = finput == "Изменить" ? Convert.ToByte(TempData["Id"]) : viewmod.indiv.Id;
                    break;
                case "Legal":
                    viewmod.legal.Id = finput == "Изменить" ? Convert.ToByte(TempData["Id"]) : viewmod.legal.Id;
                    break;
                case "Client":
                    viewmod.client.Id = finput == "Изменить" ? Convert.ToByte(TempData["Id"]) : viewmod.client.Id;
                    var clType = viewmod.client.UID.Split('-')[0];
                    var clId = Convert.ToByte(viewmod.client.UID.Split('-')[1]);
                    viewmod.client.IndivId = clType == "IND" ? clId : null;
                    viewmod.client.LegalId = clType == "LEG" ? clId : null;
                    viewmod.client.UID = String.Format("{0}-{1:d3}", clType, clId);
                    errIds = await CheckIds(viewmod.client.IndivId, viewmod.client.LegalId);
                    break;
                case "Employee":
                    viewmod.empl.Id = finput == "Изменить" ? Convert.ToByte(TempData["Id"]) : viewmod.empl.Id;
                    break;
                case "Subcon":
                    viewmod.subc.Id = finput == "Изменить" ? Convert.ToByte(TempData["Id"]) : viewmod.subc.Id;
                    var subType = viewmod.subc.UID.Split('-')[0];
                    var subId = Convert.ToByte(viewmod.subc.UID.Split('-')[1]);
                    viewmod.subc.IndivId = subType == "IND" ? subId : null;
                    viewmod.subc.LegalId = subType == "LEG" ? subId : null;
                    viewmod.subc.UID = String.Format("{0}{1:d3}", subType, subId);
                    errIds = await CheckIds(viewmod.subc.IndivId, viewmod.subc.LegalId);
                    break;
                case "Project":
                    viewmod.proj.Id = finput == "Изменить" ? Convert.ToByte(TempData["Id"]) : viewmod.proj.Id;
                    break;
                case "Edit":
                    viewmod.edit.Id = finput == "Изменить" ? Convert.ToByte(TempData["Id"]) : viewmod.edit.Id;
                    break;
            }

            if (errIds)
            {
                TempData["message"] = "Запись не была добавлена.\nУказан недействительный UID.";
                return RedirectToAction(viewmod.Target);
            }

            try
            {
                switch (viewmod.Target)
                {
                    case "Individ":
                        switch (finput)
                        {
                            case "Изменить":
                                db.Individuals.Update(viewmod.indiv);
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.indiv.Id)} успешно изменена.";
                                break;
                            case "Удалить":
                                db.Individuals.Remove(await db.Individuals.FirstOrDefaultAsync(i => i.Id == viewmod.indiv.Id));
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.indiv.Id)} успешно удалена.";
                                break;
                            case "Добавить":
                                if (await db.Individuals.FirstOrDefaultAsync(i => i.Id == viewmod.indiv.Id) != null)
                                {
                                    TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.indiv.Id)} не была добавлена.\nОбнаружено наложение на существующую запись.";
                                    return RedirectToAction(viewmod.Target);
                                }
                                db.Individuals.Add(viewmod.indiv);
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.indiv.Id)} успешно добавлена.";
                                break;
                        }
                        break;
                    case "Legal":
                        switch (finput)
                        {
                            case "Изменить":
                                db.Legals.Update(viewmod.legal);
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.legal.Id)} успешно изменена.";
                                break;
                            case "Удалить":
                                db.Legals.Remove(await db.Legals.FirstOrDefaultAsync(i => i.Id == viewmod.legal.Id));
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.legal.Id)} успешно удалена.";
                                break;
                            case "Добавить":
                                if (await db.Legals.FirstOrDefaultAsync(i => i.Id == viewmod.legal.Id) != null)
                                {
                                    TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.legal.Id)} не была добавлена.\nОбнаружено наложение на существующую запись.";
                                    return RedirectToAction(viewmod.Target);
                                }
                                db.Legals.Add(viewmod.legal);
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.legal.Id)} успешно добавлена.";
                                break;
                        }
                        break;
                    case "Client":
                        switch (finput)
                        {
                            case "Изменить":
                                db.Clients.Update(viewmod.client);
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.client.Id)} успешно изменена.";
                                break;
                            case "Удалить":
                                db.Clients.Remove(await db.Clients.FirstOrDefaultAsync(i => i.Id == viewmod.client.Id));
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.client.Id)} успешно удалена.";
                                break;
                            case "Добавить":
                                if (await db.Clients.FirstOrDefaultAsync(i => i.Id == viewmod.client.Id) != null)
                                {
                                    TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.client.Id)} не была добавлена.\nОбнаружено наложение на существующую запись.";
                                    return RedirectToAction(viewmod.Target);
                                }
                                db.Clients.Add(viewmod.client);
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.client.Id)} успешно добавлена.";
                                break;
                        }
                        break;
                    case "Employee":
                        switch (finput)
                        {
                            case "Изменить":
                                db.Employees.Update(viewmod.empl);
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.empl.Id)} успешно изменена.";
                                break;
                            case "Удалить":
                                db.Employees.Remove(await db.Employees.FirstOrDefaultAsync(i => i.Id == viewmod.empl.Id));
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.empl.Id)} успешно удалена.";
                                break;
                            case "Добавить":
                                if (await db.Employees.FirstOrDefaultAsync(i => i.Id == viewmod.empl.Id) != null)
                                {
                                    TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.empl.Id)} не была добавлена.\nОбнаружено наложение на существующую запись.";
                                    return RedirectToAction(viewmod.Target);
                                }
                                db.Employees.Add(viewmod.empl);
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.empl.Id)} успешно добавлена.";
                                break;
                        }
                        break;
                    case "Subcon":
                        switch (finput)
                        {
                            case "Изменить":
                                db.Subcontractors.Update(viewmod.subc);
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.subc.Id)} успешно изменена.";
                                break;
                            case "Удалить":
                                db.Subcontractors.Remove(await db.Subcontractors.FirstOrDefaultAsync(i => i.Id == viewmod.subc.Id));
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.subc.Id)} успешно удалена.";
                                break;
                            case "Добавить":
                                if (await db.Subcontractors.FirstOrDefaultAsync(i => i.Id == viewmod.subc.Id) != null)
                                {
                                    TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.subc.Id)} не была добавлена.\nОбнаружено наложение на существующую запись.";
                                    return RedirectToAction(viewmod.Target);
                                }
                                db.Subcontractors.Add(viewmod.subc);
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.subc.Id)} успешно добавлена.";
                                break;
                        }
                        break;
                    case "Project":
                        switch (finput)
                        {
                            case "Изменить":
                                db.Projects.Update(viewmod.proj);
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.proj.Id)} успешно изменена.";
                                break;
                            case "Удалить":
                                db.Projects.Remove(await db.Projects.FirstOrDefaultAsync(i => i.Id == viewmod.proj.Id));
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.proj.Id)} успешно удалена.";
                                break;
                            case "Добавить":
                                if (await db.Projects.FirstOrDefaultAsync(i => i.Id == viewmod.proj.Id) != null)
                                {
                                    TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.proj.Id)} не была добавлена.\nОбнаружено наложение на существующую запись.";
                                    return RedirectToAction(viewmod.Target);
                                }
                                db.Projects.Add(viewmod.proj);
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.proj.Id)} успешно добавлена.";
                                break;
                        }
                        break;
                    case "Edit":
                        switch (finput)
                        {
                            case "Изменить":
                                db.Edits.Update(viewmod.edit);
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.edit.Id)} успешно изменена.";
                                break;
                            case "Удалить":
                                db.Edits.Remove(await db.Edits.FirstOrDefaultAsync(i => i.Id == viewmod.edit.Id));
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.edit.Id)} успешно удалена.";
                                break;
                            case "Добавить":
                                if (await db.Edits.FirstOrDefaultAsync(i => i.Id == viewmod.edit.Id) != null)
                                {
                                    TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.edit.Id)} не была добавлена.\nОбнаружено наложение на существующую запись.";
                                    return RedirectToAction(viewmod.Target);
                                }
                                db.Edits.Add(viewmod.edit);
                                TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.edit.Id)} успешно добавлена.";
                                break;
                        }
                        break;
                }
                //switch (finput)
                //{
                //    case "Изменить":
                //        db.Clients.Update(viewmod.client);
                //        TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.client.Id)} успешно изменена.";
                //        break;
                //    case "Удалить":
                //        db.Clients.Remove(await db.Clients.FirstOrDefaultAsync(i => i.Id == viewmod.client.Id));
                //        TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.client.Id)} успешно удалена.";
                //        break;
                //    case "Добавить":
                //        if (await db.Clients.FirstOrDefaultAsync(i => i.Id == viewmod.client.Id) != null)
                //        {
                //            TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.client.Id)} не была добавлена.\nОбнаружено наложение на существующую запись.";
                //            return RedirectToAction(viewmod.Target);
                //        }
                //        db.Clients.Add(viewmod.client);
                //        TempData["message"] = $"Запись для {String.Format("{0}{1:d3}", pref, viewmod.client.Id)} успешно добавлена.";
                //        break;
                //}
                await db.SaveChangesAsync();
                return RedirectToAction(viewmod.Target);
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException.HResult == -2146232060)
                {
                    TempData["message"] = $"Возникла ошибка при добавлении записи.\nНевозможно добавить две записи с идентичным UID в Базу Данных.";
                    return RedirectToAction(viewmod.Target);
                }
                TempData["message"] = $"Возникла ошибка при обращении к Базе Данных\n{e}";
                return RedirectToAction(viewmod.Target);
            }
            //if (!ModelState.IsValid)
            //{
            //    TempData["message"] = $"Запись для {String.Format("CLT-{0:d3}", viewcl.client.Id)} не была добавлена.\nФорма была заполнена некорректно."; ;
            //    return View(viewcl.client);
            //}
            ModelState.Clear();
            return RedirectToAction("Index");
        }
    }
}
