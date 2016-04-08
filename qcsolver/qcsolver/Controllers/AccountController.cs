using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using qcsolver.Models;
using System.Web.Security;
using System.Data.Entity;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace qcsolver.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private UserManager<Person> _userManager;
        private onsightdbEntities db = new onsightdbEntities();

        public AccountController()
        {

        }

        //
        // GET: /Account/Index
        public ActionResult Index()
        {
            var people = db.People.Include(c => c.Country1).Include(c => c.Province1);

            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];

                if (Request["company"] != null && (user.PersonType.type == "master" || (user.PersonType.type == "admin" && user.company.ToString() == Request["company"].ToString())))
                {
                    var company = Request["company"].ToString();
                    if (Request["type"] != null && (Request["type"].ToString() == "admin" || Request["type"].ToString() == "supervisor" || Request["type"].ToString() == "contractor" || Request["type"].ToString() == "subcontractor"))
                    {
                        var type = Request["type"].ToString();
                        ViewBag.type = type;
                        people = people.Where(c => c.PersonType.type == type).Where(c => c.company.ToString() == company);
                    }
                    else
                    {
                        people = people.Where(c => c.company.ToString() == company);
                    }
                }
                else if (Request["constructionSite"] != null && (user.PersonType.type == "master" || ((user.PersonType.type == "admin" || user.PersonType.type == "supervisor") && user.company == db.ConstructionSites.Where(x => x.constructionSiteId.ToString() == Request["constructionSite"].ToString()).First().company)))
                {
                    var constructionSite = Request["constructionSite"].ToString();
                    if (Request["onsite"] != null)
                    {
                        people = people.Where(c => c.Timestamps.Where(x => x.timeOut == null && x.constructionSite.ToString() == constructionSite).Count() != 0).Where(c => c.AssignedWorkers.Where(x => x.constructionSite.ToString() == constructionSite).Count() != 0);
                    }
                    else if (Request["type"] != null && (Request["type"].ToString() == "supervisor" || Request["type"].ToString() == "contractor" || Request["type"].ToString() == "subcontractor"))
                    {
                        var type = Request["type"].ToString();
                        people = people.Where(c => c.AssignedWorkers.Where(x => x.constructionSite.ToString() == constructionSite).Count() != 0).Where(c => c.PersonType.type == type);
                    }
                    else
                    {
                        people = people.Where(c => c.AssignedWorkers.Where(x => x.constructionSite.ToString() == constructionSite).Count() != 0);
                    }
                }
                else if (Request["type"] != null && user.PersonType.type == "contractor" && Request["type"].ToString() == "subcontractor")
                {
                    var type = Request["type"].ToString();
                    people = people.Where(c => c.AssignedSubContractors1.Where(x => x.contractor == user.personId).Count() != 0);
                }
                else if (user.PersonType.type == "master")
                {
                    if (Request["online"] != null)
                    {
                        people = people.Where(c => c.online == true);
                    }
                    else if (Request["type"] != null)
                    {
                        var type = Request["type"].ToString();
                        people = people.Where(c => c.PersonType.type == type);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View(people.ToList());
        }

        //
        // GET: /Account/Profile
        [AllowAnonymous]
        public ActionResult Profile()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["person"] != null)
                {
                    var personId = Request["person"].ToString();
                    var person = db.People.Where(c => c.personId.ToString() == personId).First();
                    if (person != null && (user.PersonType.type == "master" || person.PersonType.personTypeId > user.PersonType.personTypeId))
                    {
                        ViewBag.person = person;
                        return View(person);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var person = db.People.Where(c => c.personId == user.personId).First();
                    ViewBag.person = person;
                    return View(person);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public UserManager<Person> UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager<Person>>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public bool ValidatePassword(string email, string password)
        {
            bool isValid = false;
            if (string.IsNullOrEmpty(email) || password == null)
            {
                throw new ArgumentNullException();
            }

            var row = db.People.FirstOrDefault(i => i.email == email);
            if (row == null)
            {
                throw new HttpRequestValidationException("No user found with that email");
            }
            else
            {
                if (row.password != password)
                {
                    throw new HttpRequestValidationException("Incorrect Password");
                }
                else
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Session["user"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var passwordValid = ValidatePassword(model.Email, model.Password);
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                if (passwordValid)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, true);
                    Person person = new Person();
                    person = db.People.Where(i => i.email == model.Email).FirstOrDefault();
                    person.online = true;
                    Session["user"] = person;

                    // now the user is authenticated
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("password", "Invalid password");
                }
            }
            return View(model);
        }

        //
        // GET: /Account/Logoff
        [AllowAnonymous]
        public ActionResult Logoff(string returnUrl)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Remove("user");
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Session["user"] != null)
            {
                if(((Person)Session["user"]).PersonType.type != "subcontractor" && ((Person)Session["user"]).PersonType.type != "contractor")
                {
                    ViewBag.type = new SelectList(db.PersonTypes, "personTypeId", "type");
                    ViewBag.company = new SelectList(db.Companies, "companyId", "companyName");
                    ViewBag.country = new SelectList(db.Countries, "countryId", "countryName");
                    ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Person model, HttpPostedFileBase profilePic, HttpPostedFileBase contract)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var folder = Server.MapPath("~/Content/Images/Users/" + model.email);
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    folder = Server.MapPath("~/Content/Images/Users/" + model.email + "/Certificates");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    folder = Server.MapPath("~/Content/Images/Users/" + model.email + "/Licenses");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    folder = Server.MapPath("~/Content/Images/Users/" + model.email + "/Profile");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    folder = Server.MapPath("~/Content/Images/Users/" + model.email + "/Contract");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    if (profilePic != null)
                    {
                        //If filename is too long, shorten it
                        var fileName = Path.GetFileName(profilePic.FileName);
                        if (fileName.Length > 60)
                        {
                            fileName = fileName.Substring(0, 55) + fileName.Substring(fileName.Length - 5, 5);
                        }

                        var path = Path.Combine(Server.MapPath("~/Content/Images/Users/" + model.email + "/Profile"), fileName);
                        profilePic.SaveAs(path);
                        model.pictureLocation = fileName;
                    }
                    else
                    {
                        model.pictureLocation = "~/Content/Images/BuildFrame.jpg";
                    }
                    //Saves image files, then saves their path in database
                    if (contract != null)
                    {
                        var fileName = Path.GetFileName(contract.FileName);
                        if (fileName.Length > 60)
                        {
                            fileName = fileName.Substring(0, 55) + fileName.Substring(fileName.Length - 5, 5);
                        }
                        var path = Path.Combine(Server.MapPath("~/Content/Images/Users/" + model.email + "/Contract"), fileName);
                        contract.SaveAs(path);
                        model.contractLocation = fileName;
                    }

                    db.People.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Account");
                }
                ViewBag.type = new SelectList(db.PersonTypes, "personTypeId", "type");
                ViewBag.company = new SelectList(db.Companies, "companyId", "companyName");
                ViewBag.country = new SelectList(db.Countries, "countryId", "countryName");
                ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName");
                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", 
                                                validationError.PropertyName, 
                                                validationError.ErrorMessage);
                    }
                }
            }
            return View(model);
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult AssignSubContractor()
        {
            if (Session["user"] != null)
            {
                if (((Person)Session["user"]).PersonType.type != "subcontractor")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignSubContractor(AssignedSubContractor assigned)
        {
            if (ModelState.IsValid)
            {
                if (Session["user"] != null)
                {
                    if (((Person)Session["user"]).PersonType.type != "subcontractor")
                    {
                        if (assigned.Person.AssignedWorkers.constructionSite == assigned.Person1.AssignedWorkers.constructionSite && assigned.Person.PersonType.type == "contractor" && assigned.Person1.PersonType.type == "subContractor" && (((Person)Session["user"]).PersonType.type == "master" || (((Person)Session["user"]).PersonType.type == "admin" && ((Person)Session["user"]).company == assigned.Person.company) || (((Person)Session["user"]).PersonType.type == "supervisor" && ((Person)Session["user"]).AssignedWorkers.constructionSite == assigned.Person.AssignedWorkers.constructionSite) || (((Person)Session["user"]).PersonType.type == "contractor" && ((Person)Session["user"]).personId == assigned.contractor)))
                        {
                            db.AssignedSubContractors.Add(assigned);
                            db.SaveChanges();
                            return RedirectToAction("Index", "Account");
                        }
                        else
                        {
                            return View(assigned);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            return View(assigned);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult ViewContract()
        {
            
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["person"] != null)
                {
                    var personId = Request["person"].ToString();
                    var person = db.People.Where(c => c.personId.ToString() == personId).First();
                    if (person != null && (user.PersonType.type == "master" || person.PersonType.personTypeId > user.PersonType.personTypeId))
                    {
                        Response.ContentType = "Application/pdf";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + person.contractLocation);
                        Response.TransmitFile(Server.MapPath("~/Content/Images/Users/" + person.email + "/Contract/" + person.contractLocation));
                        Response.End();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var person = db.People.Where(c => c.personId == user.personId).First();
                    Response.ContentType = "Application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + person.contractLocation);
                    Response.TransmitFile(Server.MapPath("~/Content/Images/Users/" + person.email + "/Contract/" + person.contractLocation));
                    Response.End();
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Edit()
        {
            if (Session["user"] != null)
            {
                ViewBag.type = new SelectList(db.PersonTypes, "personTypeId", "type");
                ViewBag.company = new SelectList(db.Companies, "companyId", "companyName");
                ViewBag.country = new SelectList(db.Countries, "countryId", "countryName");
                ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName");
                Person user = (Person)Session["user"];
                if (Request["person"] != null)
                {
                    var personId = Request["person"].ToString();
                    var person = db.People.Where(c => c.personId.ToString() == personId).First();
                    if (person != null && (user.PersonType.type == "master" || person.PersonType.personTypeId > user.PersonType.personTypeId))
                    {
                        ViewBag.person = person;
                        return View(person);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var person = db.People.Where(c => c.personId == user.personId).First();
                    ViewBag.person = person;
                    return View(person);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Person person, HttpPostedFileBase profilePic, HttpPostedFileBase contract)
        {
            if (ModelState.IsValid)
            {
                if (profilePic != null)
                {
                    //If filename is too long, shorten it
                    var fileName = Path.GetFileName(profilePic.FileName);
                    if (fileName.Length > 60)
                    {
                        fileName = fileName.Substring(0, 55) + fileName.Substring(fileName.Length - 5, 5);
                    }
                    System.IO.DirectoryInfo myDirInfo = new DirectoryInfo("~/Content/Images/Users/" + person.email + "/Profile");

                    foreach (FileInfo file in myDirInfo.GetFiles())
                    {
                        file.Delete();
                    }
                    var path = Path.Combine(Server.MapPath("~/Content/Images/Users/" + person.email + "/Profile"), fileName);
                    profilePic.SaveAs(path);
                    person.pictureLocation = fileName;
                }

                //Saves image files, then saves their path in database
                if (contract != null)
                {
                    var fileName = Path.GetFileName(contract.FileName);
                    if (fileName.Length > 60)
                    {
                        fileName = fileName.Substring(0, 55) + fileName.Substring(fileName.Length - 5, 5);
                    }

                    System.IO.DirectoryInfo myDirInfo = new DirectoryInfo("~/Content/Images/Users/" + person.email + "/Contract");

                    foreach (FileInfo file in myDirInfo.GetFiles())
                    {
                        file.Delete();
                    }
                    var path = Path.Combine(Server.MapPath("~/Content/Images/Users/" + person.email + "/Contract"), fileName);
                    contract.SaveAs(path);
                    person.contractLocation = fileName;
                }

                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            return View(person);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Delete()
        {
            if (Session["user"] != null)
            {
                Person user = (Person)Session["user"];
                if (Request["person"] != null)
                {
                    var personId = Request["person"].ToString();
                    var person = db.People.Where(c => c.personId.ToString() == personId).First();
                    if (person != null && (user.PersonType.type == "master" || person.PersonType.personTypeId > user.PersonType.personTypeId))
                    {
                        ViewBag.type = new SelectList(db.PersonTypes, "personTypeId", "type");
                        ViewBag.company = new SelectList(db.Companies, "companyId", "companyName");
                        ViewBag.country = new SelectList(db.Countries, "countryId", "countryName");
                        ViewBag.province = new SelectList(db.Provinces, "provinceId", "provinceName");
                        ViewBag.person = person;
                        return View(person);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
            db.SaveChanges();

            System.IO.DirectoryInfo myDirInfo = new DirectoryInfo("~/Content/Images/Users/" + person.email);

            foreach (FileInfo file in myDirInfo.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in myDirInfo.GetDirectories())
            {
                dir.Delete(true);
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        /*
        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
        */
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}