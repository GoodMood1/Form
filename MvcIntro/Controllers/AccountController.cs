using MvcIntro.Models.Entities;
using MvcIntro.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Web.Mvc;

namespace MvcIntro.Controllers
{
    public class AccountController : Controller
    {
        // GET: User Account
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        //Show register form
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserRegisterModel userRegisterModel)
        {
            //Thick controller ideology
            if (ModelState.IsValid)
            {
                using (var db = new Entities())
                {
                    var searchResults = db.Users.Where(u => u.UserName == userRegisterModel.Name);

                    if (searchResults.Any())
                    {
                        ViewBag.Error = "This user is already registered.";

                        return View(userRegisterModel);
                    }
                    //if there's no such user
                    else
                    {
                        SHA512 hasher = SHA512.Create();

                        byte[] passwordHash = hasher.ComputeHash(Encoding.UTF8.GetBytes(userRegisterModel.Password));

                        User userToAdd = new User()
                        {
                            UserName = userRegisterModel.Name,
                            PasswordHash = passwordHash
                        };

                        try
                        {
                            db.Users.Add(userToAdd);
                            db.SaveChanges();

                            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

                            string userData = jsSerializer.Serialize(new { UserID = userToAdd.UserID });

                            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                version: 1,
                                name: userToAdd.UserName,
                                issueDate: DateTime.Now,
                                expiration: DateTime.Now.AddMinutes(30),
                                isPersistent: true,
                                userData: userData
                                );

                            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));

                            authCookie.Expires = DateTime.Now.AddMinutes(30);

                            Response.Cookies.Add(authCookie);

                            return RedirectToAction("Index");
                        }
                        catch { }
                    }

                }

            }

            return View(userRegisterModel);
        }









        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginModel userLoginModel)
        {
            //Thick controller ideology
            if (ModelState.IsValid)
            {
                using (var db = new Entities())
                {
                    SHA512 hasher = SHA512.Create();
                    var searchResults = db.Users.Where(u => u.UserName == userLoginModel.Name && u.PasswordHash == hasher.ComputeHash(Encoding.UTF8.GetBytes(userLoginModel.Password)));

                    if (searchResults.Any())
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Error = "User not found";

                        return View(userLoginModel);
                    }

                }

            }

            return View(userLoginModel);
        }
    }
}