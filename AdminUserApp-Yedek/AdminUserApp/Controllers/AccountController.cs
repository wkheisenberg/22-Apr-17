using AdminUserApp.DAL;
using AdminUserApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AdminUserApp.Controllers
{
    public class AccountController : Controller
    {
        TempContext db = new TempContext();
        public static string mySessionId;

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Session.SessionID == mySessionId)
            {
                User x = (User)Session["User"];

                if (x.rol.RoleId == 1)
                {
                    return RedirectToAction("AdminDashboard", "Dashboard");
                }
                else if (x.rol.RoleId == 2)
                {
                    return RedirectToAction("UserDashboard", "Dashboard");
                }
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Login login)
        {
            if (ModelState.IsValid)
            {
                int LoginType = GetRoleID_By_UserName(login.username);
                var obj = db.kullar.Where(k => k.UserName == login.username && k.Password == login.password).FirstOrDefault();

                if (LoginType == 0 && obj == null)
                {
                    return View("Error");
                }
                else if (LoginType == 0 && obj != null)
                {
                    ViewData["obj"] = "0";
                    return View("Error");
                }
                else if (LoginType == 1 && obj != null)
                {
                    Session["User"] = obj;
                    mySessionId = Session.SessionID;
                    return RedirectToAction("AdminDashBoard", "DashBoard");
                }
                else if (LoginType == 1 && obj == null)
                {
                    ViewData["obj"] = "1";
                    return View("Error");
                }
                else if (LoginType == 2 && obj != null)
                {
                    Session["User"] = obj;
                    mySessionId = Session.SessionID;
                    return RedirectToAction("UserDashBoard", "DashBoard");
                }
                else if (LoginType == 2 && obj == null)
                {
                    ViewData["obj"] = "2";
                    return View("Error");
                }
            }
            return View(login);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            mySessionId = "";
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Register()
        {
            //daha sonra burada admin kontrollerini yap
            return View();
        }

        [HttpPost]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                db.kullar.Add(model);
                db.SaveChanges();
                return RedirectToAction("AdminDashboard", "Dashboard");
            }
            return View("Error");
        }

        public ActionResult ListUsers()
        {
            var kullar = db.kullar.ToList();
            return View(kullar);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.kullar.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.kullar.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User usr)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListUsers");
            }
            return View(usr);
        }

        public ActionResult Delete(int sid)
        {
            User usr = db.kullar.Find(sid);
            db.kullar.Remove(usr);
            db.SaveChanges();
            return RedirectToAction("ListUsers");
        }
        [HttpGet]
        public ActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateRole(Role model)
        {
            if (ModelState.IsValid)
            {
                db.roller.Add(model);
                db.SaveChanges();
                return RedirectToAction("AdminDashboard", "Dashboard");
            }
            return View("Error");
        }

        public ActionResult AssignRole()
        {
            var kullarAndRoller = db.kullar.Include(k => k.rol);
            return View(kullarAndRoller);
        }

        public ActionResult RolAta(int id)
        {
            User myuser = db.kullar.Find(id);
            var dropdownDataList = db.roller.ToList();
            var dropdownOptions = dropdownDataList.Select(m => m.RoleName);
            ViewBag.DropdownListOptions = new SelectList(dropdownOptions);
            return View(myuser);
        }
        [HttpPost]
        public ActionResult RolAta(User myuser)
        {
            //User user = new User();
            //Role myrole = new Role();

            //user.FullName = myuser.FullName;
            //user.Email = myuser.Email;
            //string str = myuser.rol.RoleName;

            //int rolId = db.roller.Where(r => r.RoleName == myuser.rol.RoleName).FirstOrDefault().RoleId;
            //myrole.RoleId = rolId;
            //myuser.rol.RoleId = rolId;

            //db.Entry(myuser).State = EntityState.Modified;
            //db.SaveChanges();
            return RedirectToAction("AssignRole");
        }



        int GetRoleID_By_UserName(string strName)
        {
            int rID = 0;
            try
            {
                rID = db.kullar.Where(u => u.UserName == strName).FirstOrDefault().rol.RoleId;
            }
            catch
            {
                return rID;
            }

            return rID;
        }
    }
}