using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext ac = new ApplicationDbContext();
        private static DBContext dc = new DBContext();

        // GET: Admin
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // Tanvir(23/12/16): Creating Paged List

            ViewBag.CurrentSort = sortOrder;
            var Users = from u in dc.Users
                        select u;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                Users = Users.Where(u => u.LastName.ToLower().Contains(searchString.ToLower()) || u.FirstName.ToLower().Contains(searchString.ToLower()));

            }

            int pageSize = 50;
            int pageNumber = (page ?? 1);

            // End of PagedList


            // Create ViewModel to run list function
            AdminUserViewModel vm = new AdminUserViewModel();
            // Assemble lists to be passed to list function
            List<AdminUserViewModel> viewModels = new List<AdminUserViewModel>();
            List<ApplicationUser> applicationUsers = ac.Users.ToList();
            List<User> users = Users.ToList();
            List<UserProfile> userProfiles = dc.UserProfiles.ToList();
            List<Place> places = dc.Places.ToList();
            List<Review> reviews = dc.Reviews.ToList();
            List<Post> posts = dc.Posts.ToList();
            //pass data to list function
            vm.AdminUserList(viewModels,
             users,
             userProfiles,            
             places,
             applicationUsers,
             posts,
             reviews
             );
            // for each viewModel, convert Id hash store in Role to name of role
            foreach(AdminUserViewModel viewModel in viewModels)
            {
                viewModel.Role = ac.Roles.Where(a => a.Id == viewModel.Role).Select(a => a.Name).FirstOrDefault();
            }

            return View(viewModels.ToPagedList(pageNumber,pageSize));
            

        }

        // GET: Admin/Details/5
        public ActionResult Details(string Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // assemble AdminUserViewModel for passed Id
            ApplicationUser appUser = ac.Users.Where(a => a.Id == Id).FirstOrDefault();
            User person = dc.Users.Where(a => a.UserID == Id).FirstOrDefault();
            UserProfile profile = dc.UserProfiles.Where(a => a.UserID == Id).FirstOrDefault();
            string rid = appUser.Roles.Where(a => a.UserId == Id).Select(a => a.RoleId).FirstOrDefault();
            string role = ac.Roles.Where(a => a.Id == rid).Select(a => a.Name).FirstOrDefault();

            string favPlace = (profile.FavoritePlace ?? "BLAH").Split(',')[0];
            string place = dc.Places.Where(a => a.PlaceID == favPlace).Select(a => a.Name).FirstOrDefault() ?? "N/A";
            string homeTown = dc.Places.Where(a => a.PlaceID == profile.HomeTown).Select(a => a.Name).FirstOrDefault();
            int postFlag = dc.Posts.Count(a => a.UserID == Id && a.Flag > 0);
            int reviewFlag = dc.Reviews.Count(a => a.UserID == Id && a.Flag > 0);
            bool lockOutEnabled = appUser.LockoutEnabled;


            AdminUserViewModel vm = new AdminUserViewModel(appUser, person, profile, role, place,homeTown, postFlag, reviewFlag);

            return View(vm);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                ac.Users.Add(applicationUser);
                ac.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(string Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // assemble AdminUserViewModel for passed Id
            ApplicationUser appUser = ac.Users.Where(a => a.Id == Id).FirstOrDefault();
            User person = dc.Users.Where(a => a.UserID == Id).FirstOrDefault();
            UserProfile profile = dc.UserProfiles.Where(a => a.UserID == Id).FirstOrDefault();

            // gets RoleId from user data and converts to role name
            string roleId = appUser.Roles.Where(a => a.UserId == Id).Select(a => a.RoleId).FirstOrDefault();
            string roleName = ac.Roles.Where(a => a.Id == roleId).Select(a => a.Name).FirstOrDefault();

            // gets first entry from favorite places - Linq does not like Split
            string favPlace = (profile.FavoritePlace ?? "BLAH").Split(',')[0];
            string place = dc.Places.Where(a => a.PlaceID == favPlace).Select(a => a.Name).FirstOrDefault() ?? "N/A";
            string homeTown = dc.Places.Where(a => a.PlaceID == profile.HomeTown).Select(a => a.Name).FirstOrDefault() ?? "N/A";

            // counts number of flagged submissions by type
            int postFlag = dc.Posts.Count(a => a.UserID == Id && a.Flag > 0);
            int reviewFlag = dc.Reviews.Count(a => a.UserID == Id && a.Flag > 0);

            bool lockOutEnabled = appUser.LockoutEnabled;

            AdminUserViewModel vm = new AdminUserViewModel(appUser, person, profile, roleName, place, homeTown, postFlag, reviewFlag);

            return View(vm);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminUserViewModel vm)
        {                 
            if (ModelState.IsValid)
            {
                // Saves relevant lockout and role data to the database
                ApplicationUser user = ac.Users.Where(a => a.Id == vm.Id).First();

                string rid = user.Roles.Where(a => a.UserId == vm.Id).Select(a => a.RoleId).FirstOrDefault();
                string role = ac.Roles.Where(a => a.Id == rid).Select(a => a.Name).FirstOrDefault() ?? "no role";
                // only remove current role and add new role if admin has changed role 
                if(role != vm.Role)
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ac));
                    var result1 = userManager.RemoveFromRole(vm.Id, role);
                    var result2 = userManager.AddToRole(vm.Id, vm.Role);
                }

                user.LockoutEnabled = vm.LockOutEnabled;
                user.LockoutEndDateUtc = vm.LockOutEndDate;
                
                // store changes in database
                ac.Entry(user).State = EntityState.Modified;
                ac.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        // GET: Admin/Delete/5
/*        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = ac.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }
*/
        // POST: Admin/Delete/5
/*        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = ac.Users.Find(id);
            ac.Users.Remove(applicationUser);
            ac.SaveChanges();
            return RedirectToAction("Index");
        }
*/
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ac.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
