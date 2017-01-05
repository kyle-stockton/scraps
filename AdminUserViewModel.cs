using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace ViewModels
{
    public class AdminUserViewModel
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        // list of possible user roles
        private readonly List<string> _roles = new List<string> { "User", "PowerUser", "Admin" };
        // creates list of SelectListItems for Edit view role dropdown
        public IEnumerable<SelectListItem> Roles
        {
            get { return new SelectList(_roles); }
        }
        // Property Values

        // From AspNetUsers table
        [Required]
        [Display(Name = "ID")]
        public string Id { get; set; }

        [Required]
        public bool LockOutEnabled { get; set; }

        [Required]
        public DateTime? LockOutEndDate { get; set; }

        [Required]
        public string Role { get; set; }


        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        // from User table
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        // from UserProfile table
        [Required]
        public DateTime DateJoined { get; set; }

        [Display(Name = "Days as Member")]
        public int DaysAsMember { get; set; }

        [Display(Name = "HomeTown")]
        public string HomeTownID { get; set; }

        [Display(Name = "Favorite Place")]
        public string FavoritePlaceID { get; set; }

        // from Place Table
        public string HomeTown { get; set; }
        public string HomeTownName { get; set; }
        public string FavoritePlace { get; set; }
        public string FavoritePlaceName { get; set; }

        // from Post table
        [Display(Name = "Flagged Posts")]
        public int PostFlag { get; set; }

        // from Review table
        [Display(Name = "Flagged Reviews")]
        public int ReviewFlag { get; set; }

        //Constructors
        public AdminUserViewModel() { } // Empty constructor

        //Populate a single view model when Id is known
        public AdminUserViewModel(
            ApplicationUser applicationUser,
            User user,
            UserProfile userProfile,
            string role,
            string place,
            string homeTown,
            int postFlag,
            int reviewFlag
            )
        {
            Id = applicationUser.Id;
            Email = applicationUser.Email;
            Role = role; //AdminController interprets user's role Id and passes role name
            FirstName = user.FirstName;
            LastName = user.LastName;
            // get difference between date joined and convert number of days to int            
            TimeSpan t = DateTime.Now - user.DateJoined;
            DaysAsMember = Convert.ToInt32(t.TotalDays);
            // Admin Controller get Place Ids from user Profile, place names from Place table, passes names to constructor
            FavoritePlaceName = place;
            HomeTownName = homeTown;
            // AdminController calculates number of flagged submissions by type, passes count as int
            PostFlag = postFlag;
            ReviewFlag = reviewFlag;

            LockOutEnabled = applicationUser.LockoutEnabled;
            LockOutEndDate = applicationUser.LockoutEndDateUtc ?? DateTime.UtcNow;
        }

        // populates list of AdminUserViewModels
        public void AdminUserList(
            List<AdminUserViewModel> adminUser,
            List<User> user,
            List<UserProfile> userProfile,
            List<Place> place,
            List<ApplicationUser> netUser,
            List<Post> post,
            List<Review> review)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            foreach (User person in user)
            {
                AdminUserViewModel vm = new AdminUserViewModel();
                vm.Id = netUser.Where(a => a.Id == person.UserID).Select(a => a.Id).FirstOrDefault();
                vm.Email = netUser.Where(a => a.Id == person.UserID).Select(a => a.Email).FirstOrDefault();
                vm.LockOutEnabled = netUser.Where(a => a.Id == person.UserID).Select(a => a.LockoutEnabled).FirstOrDefault();
                vm.LockOutEndDate = netUser.Where(a => a.Id == person.UserID).Select(a => a.LockoutEndDateUtc).FirstOrDefault() ?? DateTime.UtcNow;

                try
                {
                    // gets user's Role ID, passes to AdminController for lookup in AspNetRoles table
                    vm.Role = netUser.Where(a => a.Id == person.UserID).FirstOrDefault().Roles.Where(a => a.UserId == vm.Id).Select(a => a.RoleId).FirstOrDefault();
                }
                catch
                {
                    UserManager.AddToRole(vm.Id, "User");
                    vm.Role = netUser.Where(a => a.Id == person.UserID).FirstOrDefault().Roles.Where(a => a.UserId == vm.Id).Select(a => a.RoleId).FirstOrDefault();
                }
                vm.FirstName = person.FirstName;
                vm.LastName = person.LastName;
                
                //calculates difference in days between date joined and today (not UTC)
                TimeSpan t = DateTime.Now - person.DateJoined;
                vm.DaysAsMember = Convert.ToInt32(t.TotalDays);

                vm.HomeTownID = userProfile.Where(a => a.UserID == vm.Id).Select(a => a.HomeTown).FirstOrDefault();
                vm.HomeTownName = place.Where(a => a.PlaceID == vm.HomeTownID).Select(a => a.Name).FirstOrDefault();
                string favPlaces = userProfile.Where(a => a.UserID == vm.Id).Select(a => a.FavoritePlace).FirstOrDefault();
                //Checks to see if user has a favorite place.
                if (favPlaces==null)
                {
                    vm.FavoritePlaceName = "N/A";
                }
                else
                {
                    vm.FavoritePlaceID = favPlaces.Split(',')[0];
                    vm.FavoritePlaceName = place.Where(a => a.PlaceID == vm.FavoritePlaceID).Select(a => a.Name).FirstOrDefault();
                }

                // Counts number of user's flagged submmissions by type
                vm.PostFlag = post.Count(a => (a.UserID == vm.Id) && (a.Flag > 0));
                vm.ReviewFlag = review.Count(a => (a.UserID == vm.Id) && (a.Flag > 0));

                // add to running list
                adminUser.Add(vm);
            }
        }
    }
}