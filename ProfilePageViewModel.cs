using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class ProfilePageViewModel
    {
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateJoined { get; set; }
        public string ProfilePicturePath { get; set; }
        public string ProfilePictureName { get; set; }
        public string CoverPhotoPath { get; set; }
        public string CoverPhotoName { get; set; }
        public string CurrentUserID { get; set; }
        public RelationshipStatus? RelationshipStatus { get; set; }
        public int RelationshipID { get; set; }
        public bool Private { get; set; }

        public ProfilePageViewModel() { }

        public ProfilePageViewModel(User user, UserProfile userProfile, Image profilePicture, Image coverPhoto, string currentUserID, RelationshipStatus status, int relationshipID, bool privateProfile)
        {
            UserID = user.UserID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            DateJoined = user.DateJoined.ToString();
            ProfilePicturePath = (profilePicture.Path != "no-image.png") ? user.UserID + "/" + profilePicture.Path : profilePicture.Path;
            ProfilePictureName = profilePicture.FileName;
            CoverPhotoName = coverPhoto.FileName;
            CoverPhotoPath = (coverPhoto.Path != "cover-image.jpg") ? user.UserID + "/" + coverPhoto.Path : coverPhoto.Path;
            CurrentUserID = currentUserID;
            RelationshipStatus = status;
            RelationshipID = relationshipID;
            Private = privateProfile;
        }

        public ProfilePageViewModel(User user, UserProfile userProfile, Image profilePicture, Image coverPhoto, string currentUserID)
        {
            UserID = user.UserID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            DateJoined = user.DateJoined.ToString();
            ProfilePicturePath = (profilePicture.Path != "no-image.png") ? user.UserID + "/" + profilePicture.Path : profilePicture.Path;
            
            ProfilePictureName = profilePicture.FileName;

            CoverPhotoName = coverPhoto.FileName;
            CoverPhotoPath = (coverPhoto.Path != "cover-image.jpg") ? user.UserID + "/" + coverPhoto.Path : coverPhoto.Path;
            CurrentUserID = currentUserID;
        }
    }

    public class AboutViewModel
    {
        public string ProfilePicturePath { get; set; }
        public string ProfilePictureName { get; set; }
        //public string AboutPicturePath { get; set; }
        //public string AboutPictureName { get; set; }
        public PlaceViewModel HomeTown { get; set; }
        public List<PlaceViewModel> PastLocals = new List<PlaceViewModel>();
        public PlaceViewModel LastTraveled { get; set; }
        public List<PlaceViewModel> FavoritePlaces = new List<PlaceViewModel>();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }
        public string MemberSince { get; set; }
        public int Age;
        public string Birthday;
        public string About;

        public AboutViewModel() { }

        public AboutViewModel(User user, UserProfile userProfile, Image profilePicture)
        {
            ProfilePictureName = profilePicture.FileName;
            ProfilePicturePath = (profilePicture.Path != "no-image.png") ? user.UserID + "/" + profilePicture.Path : profilePicture.Path;
            HomeTown = (userProfile.HomeTown != null) ? PlaceViewModel.GetPlaceObjectt(userProfile.HomeTown) : new PlaceViewModel();
            if (userProfile.PastLocal != null)
            {
                string[] userProfilePastLocals = userProfile.PastLocal.Split(',');
                foreach (string pl in userProfilePastLocals)
                {
                    if (pl != "") PastLocals.Add(PlaceViewModel.GetPlaceObjectt(pl));
                }
            }
            else
            {
                PastLocals.Add(new PlaceViewModel());
            }
            LastTraveled = (userProfile.LastTraveled != null) ? PlaceViewModel.GetPlaceObjectt(userProfile.LastTraveled) : new PlaceViewModel();
            if (userProfile.FavoritePlace != null)
            {
                string[] userProfileFavoritePlaces = userProfile.FavoritePlace.Split(',');
                foreach (string fp in userProfileFavoritePlaces)
                {
                    if (fp != "") FavoritePlaces.Add(PlaceViewModel.GetPlaceObjectt(fp));
                }
            }
            FirstName = user.FirstName;
            LastName = user.LastName;
            DOB = userProfile.DOB;
            DateTime now = DateTime.Today;
            Age = (now.Year - DOB.Year);
            if (now < DOB.AddYears(Age)) Age--;
            About = userProfile.About;
            Birthday = String.Format("{0:MMMM d}",DOB);
            MemberSince = String.Format("{0:M/d/yyyy}",user.DateJoined);
        }
    }

    public class ImageViewModel
    {
        public string UserID { get; set; }
        public ICollection<Image> Images { get; set; }


        // Constructors
        public ImageViewModel() { }

        public ImageViewModel(string userID, ICollection<Image> images)
        {
            UserID = userID;
            Images = images;
        }
    }


}