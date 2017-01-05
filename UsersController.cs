using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Controllers
{
    public class UsersController : Controller
    {
        private DBContext db = new DBContext();

        // GET: NewsFeed
        public ActionResult Index()
        {
            try
            {
                string userID = User.Identity.GetUserId();
                User user = db.Users.SqlQuery("dbo.User_Select @p0", userID).SingleOrDefault();
                Image profilePicture = Image.GetProfileImages(userID, FileType.ProfilePicture);
                CreatePostViewModel createPostViewModel = new CreatePostViewModel(user, profilePicture.Path);
                return View(createPostViewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home", null);
            }

        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.SqlQuery("dbo.User_Select @p0", id).SingleOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,FirstName,LastName,Permission,DateJoined,ConnectionID,Connected")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.SqlQuery("dbo.User_Insert @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7",
                                    user.UserID, user.FirstName, user.LastName, user.Permission,
                                    user.DateJoined, user.ConnectionID, user.Connected);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/ProfilePage
        public ActionResult ProfilePage(string userID)
        {
            try
            {
                User user = db.Users.SqlQuery("dbo.User_Select @p0", userID).SingleOrDefault();
                UserProfile userProfile = db.UserProfiles.SqlQuery("dbo.UserProfile_Select @p0", userID).SingleOrDefault();
                Image profilePicture = Image.GetProfileImages(userID, FileType.ProfilePicture);
                Image coverPhoto = Image.GetProfileImages(userID, FileType.CoverPhoto);
                string currentUserID = User.Identity.GetUserId();
                Relationship relationship = db.Relationships.SqlQuery("dbo.Check_Friends @p0, @p1", currentUserID, userID).FirstOrDefault();
                if (relationship == null)
                {
                    relationship = new Relationship();
                    relationship.Status = RelationshipStatus.NoRelationship;
                }
                ProfilePageViewModel profilePageViewModel = new ProfilePageViewModel(user, userProfile, profilePicture, coverPhoto, currentUserID, relationship.Status, relationship.ID, userProfile.Private);


                //ProfilePageViewModel profilePageViewModel = new ProfilePageViewModel();
                //if (relationship != null)
                //{
                //    profilePageViewModel = new ProfilePageViewModel(user, userProfile, profilePicture, coverPhoto, currentUserID, relationship.Status, userProfile.Private);
                //} else
                //{
                //    profilePageViewModel = new ProfilePageViewModel(user, userProfile, profilePicture, coverPhoto, currentUserID, RelationshipStatus.NoRelationship, userProfile.Private);
                //}

                return View(profilePageViewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home", null);
                throw;
            }

        }

        // GET: Users/Edit/5
        public async Task<ActionResult> EditProfile()
        {
            string userID = User.Identity.GetUserId();

            if (userID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = await db.Users.SqlQuery("dbo.User_Select @p0", userID).SingleOrDefaultAsync();
            UserProfile userProfile = await db.UserProfiles.SqlQuery("dbo.UserProfile_Select @p0", userID).SingleOrDefaultAsync();
            PlaceViewModel homeTown = (userProfile.HomeTown != null) ? await PlaceViewModel.GetPlaceObject(userProfile.HomeTown) : new PlaceViewModel();
            List<PlaceViewModel> pastLocal = new List<PlaceViewModel>();
            if (userProfile.PastLocal != null)
            {
                string[] pastLocalStringArray = userProfile.PastLocal.Split(',');
                for (int i = 0; i < 10; i++)
                {
                    pastLocal.Add((i < pastLocalStringArray.Length && pastLocalStringArray[i] != "") ? await PlaceViewModel.GetPlaceObject(pastLocalStringArray[i]) : new PlaceViewModel());
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    pastLocal.Add(new PlaceViewModel());
                }
            }
            List<PlaceViewModel> favoritePlace = new List<PlaceViewModel>();
            if (userProfile.FavoritePlace != null)
            {
                string[] favoritePlaceStringArray = userProfile.FavoritePlace.Split(',');
                for (int i = 0; i < 5; i++)
                {
                    favoritePlace.Add((i < favoritePlaceStringArray.Length && favoritePlaceStringArray[i] != "") ? await PlaceViewModel.GetPlaceObject(favoritePlaceStringArray[i]) : new PlaceViewModel());
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    favoritePlace.Add(new PlaceViewModel());
                }
            }
            PlaceViewModel lastTraveled = (userProfile.LastTraveled != null) ? await PlaceViewModel.GetPlaceObject(userProfile.LastTraveled) : new PlaceViewModel();
            EditProfileViewModel editProfileViewModel = new EditProfileViewModel(user, userProfile, homeTown, pastLocal, favoritePlace, lastTraveled, userProfile.Private);

            if (user == null || userProfile == null || editProfileViewModel == null)
            {
                return HttpNotFound();
            }

            return View(editProfileViewModel);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile([Bind(Include = "FirstName,LastName,About,HomeTown,HomeTownID,HomeTownName,HomeTownLat,HomeTownLng,PastLocal0,PastLocal0ID,PastLocal0Name,PastLocal0Lat,PastLocal0Lng,PastLocal1,PastLocal1ID,PastLocal1Name,PastLocal1Lat,PastLocal1Lng,PastLocal2,PastLocal2ID,PastLocal2Name,PastLocal2Lat,PastLocal2Lng,PastLocal3,PastLocal3ID,PastLocal3Name,PastLocal3Lat,PastLocal3Lng,PastLocal4,PastLocal4ID,PastLocal4Name,PastLocal4Lat,PastLocal4Lng,PastLocal5,PastLocal5ID,PastLocal5Name,PastLocal5Lat,PastLocal5Lng,PastLocal6,PastLocal6ID,PastLocal6Name,PastLocal6Lat,PastLocal6Lng,PastLocal7,PastLocal7ID,PastLocal7Name,PastLocal7Lat,PastLocal7Lng,PastLocal8,PastLocal8ID,PastLocal8Name,PastLocal8Lat,PastLocal8Lng,PastLocal9,PastLocal9ID,PastLocal9Name,PastLocal9Lat,PastLocal9Lng,FavoritePlace0,FavoritePlace0ID,FavoritePlace0Name,FavoritePlace0Lat,FavoritePlace0Lng,FavoritePlace1,FavoritePlace1ID,FavoritePlace1Name,FavoritePlace1Lat,FavoritePlace1Lng,FavoritePlace2,FavoritePlace2ID,FavoritePlace2Name,FavoritePlace2Lat,FavoritePlace2Lng,FavoritePlace3,FavoritePlace3ID,FavoritePlace3Name,FavoritePlace3Lat,FavoritePlace3Lng,FavoritePlace4,FavoritePlace4ID,FavoritePlace4Name,FavoritePlace4Lat,FavoritePlace4Lng,LastTraveled,LastTraveledID,LastTraveledName,LastTraveledLat,LastTraveledLng,DOB,Images,PrivateProfile")] EditProfileViewModel editProfileViewModel)
        {
            if (ModelState.IsValid)
            {
                // GET: userID
                string userID = User.Identity.GetUserId();

                // Find the current user and apply changes made to the User and UserProfile
                User user = db.Users.SqlQuery("dbo.User_Select @p0", userID).SingleOrDefault();
                user.FirstName = editProfileViewModel.FirstName;
                user.LastName = editProfileViewModel.LastName;

                UserProfile userProfile = db.UserProfiles.SqlQuery("dbo.UserProfile_Select @p0", userID).SingleOrDefault();
                userProfile.About = editProfileViewModel.About;
                userProfile.DOB = DateTime.Parse(editProfileViewModel.DOB);
                userProfile.HomeTown = editProfileViewModel.HomeTownID;
                string[] PastLocalArray = { editProfileViewModel.PastLocal0ID, editProfileViewModel.PastLocal1ID, editProfileViewModel.PastLocal2ID,
                    editProfileViewModel.PastLocal3ID, editProfileViewModel.PastLocal4ID, editProfileViewModel.PastLocal5ID, editProfileViewModel.PastLocal6ID,
                    editProfileViewModel.PastLocal7ID, editProfileViewModel.PastLocal8ID, editProfileViewModel.PastLocal9ID};
                userProfile.PastLocal = string.Join(",", PastLocalArray);

                userProfile.LastTraveled = editProfileViewModel.LastTraveledID;
                string[] FavoritePlaceArray = {editProfileViewModel.FavoritePlace0ID, editProfileViewModel.FavoritePlace1ID, editProfileViewModel.FavoritePlace2ID,
                    editProfileViewModel.FavoritePlace3ID, editProfileViewModel.FavoritePlace4ID };
                userProfile.FavoritePlace = string.Join(",", FavoritePlaceArray);
                userProfile.Private = editProfileViewModel.PrivateProfile;

                // Save any images or files that were uploaded
                if (editProfileViewModel.Images != null && editProfileViewModel.Images.First() != null)
                {
                    foreach (var item in editProfileViewModel.Images)
                    {
                        Image.SavePhotos(user, item);
                    }
                }
                List<Place> placeList = new List<Place>() {
                    new Place { PlaceID = editProfileViewModel.HomeTownID, Name = editProfileViewModel.HomeTownName, Lat = editProfileViewModel.HomeTownLat, Lng = editProfileViewModel.HomeTownLng },
                    new Place { PlaceID = editProfileViewModel.PastLocal0ID, Name = editProfileViewModel.PastLocal0Name, Lat = editProfileViewModel.PastLocal0Lat, Lng = editProfileViewModel.PastLocal0Lng},
                    new Place { PlaceID = editProfileViewModel.PastLocal1ID, Name = editProfileViewModel.PastLocal1Name, Lat = editProfileViewModel.PastLocal1Lat, Lng = editProfileViewModel.PastLocal1Lng},
                    new Place { PlaceID = editProfileViewModel.PastLocal2ID, Name = editProfileViewModel.PastLocal2Name, Lat = editProfileViewModel.PastLocal2Lat, Lng = editProfileViewModel.PastLocal2Lng},
                    new Place { PlaceID = editProfileViewModel.PastLocal3ID, Name = editProfileViewModel.PastLocal3Name, Lat = editProfileViewModel.PastLocal3Lat, Lng = editProfileViewModel.PastLocal3Lng},
                    new Place { PlaceID = editProfileViewModel.PastLocal4ID, Name = editProfileViewModel.PastLocal4Name, Lat = editProfileViewModel.PastLocal4Lat, Lng = editProfileViewModel.PastLocal4Lng},
                    new Place { PlaceID = editProfileViewModel.PastLocal5ID, Name = editProfileViewModel.PastLocal5Name, Lat = editProfileViewModel.PastLocal5Lat, Lng = editProfileViewModel.PastLocal5Lng},
                    new Place { PlaceID = editProfileViewModel.PastLocal6ID, Name = editProfileViewModel.PastLocal6Name, Lat = editProfileViewModel.PastLocal6Lat, Lng = editProfileViewModel.PastLocal6Lng},
                    new Place { PlaceID = editProfileViewModel.PastLocal7ID, Name = editProfileViewModel.PastLocal7Name, Lat = editProfileViewModel.PastLocal7Lat, Lng = editProfileViewModel.PastLocal7Lng},
                    new Place { PlaceID = editProfileViewModel.PastLocal8ID, Name = editProfileViewModel.PastLocal8Name, Lat = editProfileViewModel.PastLocal8Lat, Lng = editProfileViewModel.PastLocal8Lng},
                    new Place { PlaceID = editProfileViewModel.PastLocal9ID, Name = editProfileViewModel.PastLocal9Name, Lat = editProfileViewModel.PastLocal9Lat, Lng = editProfileViewModel.PastLocal9Lng},
                    new Place { PlaceID = editProfileViewModel.FavoritePlace0ID, Name = editProfileViewModel.FavoritePlace0Name, Lat = editProfileViewModel.FavoritePlace0Lat, Lng = editProfileViewModel.FavoritePlace0Lng },
                    new Place { PlaceID = editProfileViewModel.FavoritePlace1ID, Name = editProfileViewModel.FavoritePlace1Name, Lat = editProfileViewModel.FavoritePlace1Lat, Lng = editProfileViewModel.FavoritePlace1Lng },
                    new Place { PlaceID = editProfileViewModel.FavoritePlace2ID, Name = editProfileViewModel.FavoritePlace2Name, Lat = editProfileViewModel.FavoritePlace2Lat, Lng = editProfileViewModel.FavoritePlace2Lng },
                    new Place { PlaceID = editProfileViewModel.FavoritePlace3ID, Name = editProfileViewModel.FavoritePlace3Name, Lat = editProfileViewModel.FavoritePlace3Lat, Lng = editProfileViewModel.FavoritePlace3Lng },
                    new Place { PlaceID = editProfileViewModel.FavoritePlace4ID, Name = editProfileViewModel.FavoritePlace4Name, Lat = editProfileViewModel.FavoritePlace4Lat, Lng = editProfileViewModel.FavoritePlace4Lng },
                    new Place { PlaceID = editProfileViewModel.LastTraveledID, Name = editProfileViewModel.LastTraveledName, Lat = editProfileViewModel.LastTraveledLat, Lng = editProfileViewModel.LastTraveledLng }
                };

                Place[] places = placeList.ToArray();

                for (int i = 0; i < places.Length; i++)
                {
                    Place place = db.Places.SqlQuery("dbo.Place_Select @p0", places[i].PlaceID).SingleOrDefault();

                    if (place == null)
                    {
                        place = new Place(places[i].PlaceID, places[i].Name, places[i].Lat, places[i].Lng);
                        try
                        {
                            db.Database.ExecuteSqlCommand("dbo.Place_Insert @p0, @p1, @p2, @p3", place.PlaceID, place.Name, place.Lat, place.Lng);
                        }
                        catch { }
                    }

                }

                db.Entry(user).State = EntityState.Modified;
                db.Entry(userProfile).State = EntityState.Modified;
                db.SaveChanges();
            }

            //return View(editProfileViewModel);
            return await EditProfile();

        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.SqlQuery("dbo.User_Select @p0", id).SingleOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            db.Users.SqlQuery("dbo.User_Delete @p0", id).SingleOrDefault();
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public PartialViewResult _Reviews(string userID)
        {
            string currentUserID = User.Identity.GetUserId();
            ICollection<Review> reviewsList = db.Reviews.SqlQuery("dbo.Review_Search @p0", userID).ToList();
            List<ReviewViewModel> formattedReviewList = new List<ReviewViewModel>();
            using (WebClient wc = new WebClient())
            {
                foreach (Review review in reviewsList)
                {
                    string url = String.Format("https://maps.googleapis.com/maps/api/place/details/json?&placeid={0}&key=AIzaSyBI5B2snURiIE8VkeuNYL2Es3ZZf8veRf4", review.PlaceID);
                    string json = wc.DownloadString(url);
                    PlaceObject jsonObject = JsonConvert.DeserializeObject<PlaceObject>(json);

                    ReviewViewModel formattedReview = new ReviewViewModel(review, jsonObject);
                    formattedReviewList.Add(formattedReview);
                }

                return PartialView("ProfilePage/_Reviews", formattedReviewList.OrderByDescending(i => i.ReviewID));

            }

        }

        public PartialViewResult _Posts(string userID)
        {
            string currentUserID = User.Identity.GetUserId();
            ICollection<Post> postsList = db.Posts.SqlQuery("dbo.Post_Search @p0", userID).ToList();
            List<NewsFeedViewModel> formattedPostList = new List<NewsFeedViewModel>();

            foreach (Post post in postsList)
            {
                User user = db.Users.SqlQuery("dbo.User_Select @p0", userID).SingleOrDefault();
                Image profilePicture = Image.GetProfileImages(userID, FileType.ProfilePicture);
                Image postedPicture = (post.PhotoID != null) ? db.Images.SqlQuery("dbo.Photo_Select @p0", post.PhotoID).SingleOrDefault() : new Image();
                NewsFeedViewModel formattedPost = new NewsFeedViewModel(user, profilePicture, postedPicture, post);
                formattedPostList.Add(formattedPost);
            }

            return PartialView("ProfilePage/_Posts", formattedPostList.OrderByDescending(i => i.PostID));
        }

        public PartialViewResult _Friends(string userID)
        {
            List<RelationshipViewModel> friends = RelationshipViewModel.GetRelationshipList(userID, RelationshipStatus.Friends);
            return PartialView("ProfilePage/_Friends", friends);
        }

        public PartialViewResult _Photos(string userID)
        {
            ICollection<Image> images = db.Images.SqlQuery("dbo.File_Search @p0", userID).ToList();
            ImageViewModel imageViewModel = new ImageViewModel(userID, images);

            return PartialView("ProfilePage/_Photos", imageViewModel);
        }

        public PartialViewResult _About(string userID)
        {
            User user = db.Users.SqlQuery("dbo.User_Select @p0", userID).SingleOrDefault();
            UserProfile userProfile = db.UserProfiles.SqlQuery("dbo.UserProfile_Select @p0", userID).SingleOrDefault();
            Image profilePicture = Image.GetProfileImages(userID, FileType.AboutPicture);
            AboutViewModel aboutViewModel = new AboutViewModel(user, userProfile, profilePicture);

            return PartialView("ProfilePage/_About", aboutViewModel);
        }

        public PartialViewResult _Comment(string postID)
        {
            NewsFeedViewModel newsFeedViewModel = new NewsFeedViewModel();
            return PartialView("_Comment", newsFeedViewModel);

        }
        public PartialViewResult _PhotoLightBox(string userID)
        {
            ICollection<Image> images = db.Images.SqlQuery("dbo.File_Search @p0", userID).ToList();
            ImageViewModel imageViewModel = new ImageViewModel(userID, images);

            return PartialView("ProfilePage/_PhotoLightBox", imageViewModel);

        }
        public PartialViewResult _WallpaperLightBox(string userID)
        {
            ICollection<Image> images = db.Images.SqlQuery("dbo.File_Search @p0", userID).ToList();
            ImageViewModel imageViewModel = new ImageViewModel(userID, images);

            return PartialView("ProfilePage/_WallpaperLightBox", imageViewModel);

        }
        public PartialViewResult _SuggestedFriends()
        {
            string userID = User.Identity.GetUserId();
            List<RelationshipViewModel> suggestedFriends = RelationshipViewModel.GetNullRelationshipList(userID);
            return PartialView("_SuggestedFriends", suggestedFriends);

        }
    }
}
