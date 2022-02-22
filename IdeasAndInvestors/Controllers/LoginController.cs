﻿using IdeasAndInvestors.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdeasAndInvestors.Controllers
{
    public class LoginController : Controller
    {
        #region Default
        private readonly Models.IdeasAndInvestorsDbContext bkDb;
        private readonly IWebHostEnvironment henv;

        public LoginController(Models.IdeasAndInvestorsDbContext bkDB, IWebHostEnvironment henv)
        {
            bkDb = bkDB;
            this.henv = henv;
        }
        #endregion Default
        public IActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {            
            return View();
        }

        [HttpPost]
        public IActionResult Login(IFormCollection frm)
        {
            var email = Convert.ToString(frm["Email"]);
            var password = Convert.ToString(frm["Password"]);
            var rdFound = bkDb.PersonMasters.Where(usr => usr.Pemail == email && usr.Ppassword == password).FirstOrDefault();
            if (rdFound != null)
            {
                TempData["ErrMsg"] = "Login Successfull";
            }
            else
            {
                TempData["ErrMsg"] = "Invalid Email or Password";
            }
            
            return View();
        }
        [HttpGet]
        public IActionResult SignUPStartUp()
        {
            return View();

        }
        [HttpPost]
        public IActionResult SignUPStartUp(PersonMaster personMaster, IFormFile file)
        {

            string uniqueImageName = null;
            if (file != null)
            {
                string uploadimgfoldername = Path.Combine(henv.WebRootPath, "images\\StartupImage");
                uniqueImageName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string finalPath = Path.Combine(uploadimgfoldername, uniqueImageName);
                file.CopyTo(new FileStream(finalPath, FileMode.Create));
                personMaster.Pimage = "images\\StartupImage" + uniqueImageName;
            }
            
            personMaster.Pqid = 0;
            personMaster.Panswer = "NoAnswer";
            personMaster.Prollid = 1;//1 for startup
            bkDb.PersonMasters.Add(personMaster);
            bkDb.SaveChanges();
            return RedirectToAction("Login");

        }
        public IActionResult SignUPInvestor()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(IFormCollection frm)
        {
            var email = Convert.ToString(frm["Email"]);
            var password = Convert.ToString(frm["Password"]);
            var rdFound = bkDb.PersonMasters.Where(usr => usr.Pemail == email && usr.Ppassword == password).FirstOrDefault();
            if (rdFound != null)
            {
                rdFound.Ppassword= Convert.ToString(frm["CPassword"]);
                bkDb.Entry(rdFound).State = EntityState.Modified;
                bkDb.SaveChanges();
                TempData["ErrMsg"] = "Password Updated Successfully";
            }
            else
            {
                TempData["ErrMsg"] = "Invalid Password or Email";
            }
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        
    }
}
