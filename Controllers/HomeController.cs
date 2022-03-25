using my.DbContext;
using my.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace my.Controllers
{
    public class HomeController : Controller
    {
        MissedClassEntities1 Db = new MissedClassEntities1();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
          public ActionResult Index(UserModel userobj)
        {

            var userRes = Db.Users.Where(a => a.Email == userobj.Email).FirstOrDefault();

            if (userRes == null)
            {
                TempData["Invaid"] = "Email Not Found Or Invaid User";
            }
            else
            {
                if (userRes.Email == userobj.Email && userRes.Password == userobj.Password)
                {
                    FormsAuthentication.SetAuthCookie(userRes.Email, false);
                    Session["UserName"] = userRes.Name;
                    Session["UserEmail"] = userRes.Email;
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    TempData["Wrong"] = "Wrong Email Or Password";
                    return View();
                }
            }
            return View();


        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
            return View();
        }
        [Authorize]
         public ActionResult Dashboard()
        {
            return View();
        }
        [Authorize]
         public ActionResult StudentTable()
        {
            var data = Db.tbl_Employee.ToList();

            List<EmployeeModel> empmodel = new List<EmployeeModel>();

            foreach (var item in data)
            {
                empmodel.Add(new EmployeeModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email
                });
            }
            return View(empmodel);
        }

        public ActionResult AddDetails()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddDetails(EmployeeModel e)
        {
            tbl_Employee objnew = new tbl_Employee();
            objnew.Id = e.Id;
            objnew.Name = e.Name;
            objnew.Email = e.Email;

            if (e.Id == 0)
            {
                Db.tbl_Employee.Add(objnew);
                Db.SaveChanges();
            }
            else
            {
                Db.Entry(objnew).State = System.Data.Entity.EntityState.Modified;
                Db.SaveChanges();
            }
          
            return RedirectToAction("StudentTable");
            //return View();
        }

        public ActionResult Delete(int id)
        {
            var Deleteitem = Db.tbl_Employee.Where(x => x.Id == id).First();
            Db.tbl_Employee.Remove(Deleteitem);
            Db.SaveChanges();
            return RedirectToAction("StudentTable");
            //return View();
        }

        public ActionResult Edit(int id)
        {
            EmployeeModel objemp = new EmployeeModel();
            var edititem = Db.tbl_Employee.Where(x => x.Id == id).First();
            objemp.Id = edititem.Id;
            objemp.Name = edititem.Name;
            objemp.Email = edititem.Email;
            return View("AddDetails", objemp);
        }
    }
}