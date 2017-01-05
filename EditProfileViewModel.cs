using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class EditProfileViewModel
    {
        // Property Values
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "About")]
        [DataType(DataType.MultilineText)]
        public string About { get; set; }

        [Display(Name = "Local Of")]
        public string HomeTown { get; set; }
        public string HomeTownName { get; set; }
        public string HomeTownID { get; set; }
        public double HomeTownLat { get; set; }
        public double HomeTownLng { get; set; }

        [Display(Name = "Past Local Of")]
        public string PastLocal0 { get; set; }
        public string PastLocal0ID { get; set; }
        public string PastLocal0Name { get; set; }
        public double PastLocal0Lat { get; set; }
        public double PastLocal0Lng { get; set; }

        public string PastLocal1 { get; set; }
        public string PastLocal1ID { get; set; }
        public string PastLocal1Name { get; set; }
        public double PastLocal1Lat { get; set; }
        public double PastLocal1Lng { get; set; }

        public string PastLocal2 { get; set; }
        public string PastLocal2ID { get; set; }
        public string PastLocal2Name { get; set; }
        public double PastLocal2Lat { get; set; }
        public double PastLocal2Lng { get; set; }

        public string PastLocal3 { get; set; }
        public string PastLocal3ID { get; set; }
        public string PastLocal3Name { get; set; }
        public double PastLocal3Lat { get; set; }
        public double PastLocal3Lng { get; set; }

        public string PastLocal4 { get; set; }
        public string PastLocal4ID { get; set; }
        public string PastLocal4Name { get; set; }
        public double PastLocal4Lat { get; set; }
        public double PastLocal4Lng { get; set; }

        public string PastLocal5 { get; set; }
        public string PastLocal5ID { get; set; }
        public string PastLocal5Name { get; set; }
        public double PastLocal5Lat { get; set; }
        public double PastLocal5Lng { get; set; }

        public string PastLocal6 { get; set; }
        public string PastLocal6ID { get; set; }
        public string PastLocal6Name { get; set; }
        public double PastLocal6Lat { get; set; }
        public double PastLocal6Lng { get; set; }

        public string PastLocal7 { get; set; }
        public string PastLocal7ID { get; set; }
        public string PastLocal7Name { get; set; }
        public double PastLocal7Lat { get; set; }
        public double PastLocal7Lng { get; set; }

        public string PastLocal8 { get; set; }
        public string PastLocal8ID { get; set; }
        public string PastLocal8Name { get; set; }
        public double PastLocal8Lat { get; set; }
        public double PastLocal8Lng { get; set; }

        public string PastLocal9 { get; set; }
        public string PastLocal9ID { get; set; }
        public string PastLocal9Name { get; set; }
        public double PastLocal9Lat { get; set; }
        public double PastLocal9Lng { get; set; }

        public string FirstEmptyPastLocal { get; set; }

        [Display(Name = "Favorite Place")]
        public string FavoritePlace0 { get; set; }
        public string FavoritePlace0ID { get; set; }
        public string FavoritePlace0Name { get; set; }
        public double FavoritePlace0Lat { get; set; }
        public double FavoritePlace0Lng { get; set; }

        public string FavoritePlace1 { get; set; }
        public string FavoritePlace1ID { get; set; }
        public string FavoritePlace1Name { get; set; }
        public double FavoritePlace1Lat { get; set; }
        public double FavoritePlace1Lng { get; set; }

        public string FavoritePlace2 { get; set; }
        public string FavoritePlace2ID { get; set; }
        public string FavoritePlace2Name { get; set; }
        public double FavoritePlace2Lat { get; set; }
        public double FavoritePlace2Lng { get; set; }

        public string FavoritePlace3 { get; set; }
        public string FavoritePlace3ID { get; set; }
        public string FavoritePlace3Name { get; set; }
        public double FavoritePlace3Lat { get; set; }
        public double FavoritePlace3Lng { get; set; }

        public string FavoritePlace4 { get; set; }
        public string FavoritePlace4ID { get; set; }
        public string FavoritePlace4Name { get; set; }
        public double FavoritePlace4Lat { get; set; }
        public double FavoritePlace4Lng { get; set; }

        public string FirstEmptyFavoritePlace { get; set; }

        [Display(Name = "Last Traveled")]
        public string LastTraveled { get; set; }
        public string LastTraveledID { get; set; }
        public string LastTraveledName { get; set; }
        public double LastTraveledLat { get; set; }
        public double LastTraveledLng { get; set; }

        [Display(Name = "Date of Birth")]
        public string DOB { get; set; }

        [Display(Name = "Private Profile")]
        public bool PrivateProfile { get; set; }

        [Display(Name = "Upload Photos")]
        public List<HttpPostedFileBase> Images { get; set; }

        //Constructors
        public EditProfileViewModel() {} // Empty constructor

        public EditProfileViewModel(User user, 
            UserProfile userProfile,
            PlaceViewModel homeTown, 
            List<PlaceViewModel> pastLocal, 
            List<PlaceViewModel> favoritePlace, 
            PlaceViewModel lastTraveled,
            bool privateProfile) // Value passed constructor
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            About = userProfile.About;
            HomeTown = homeTown.FormattedAddress;
            HomeTownID = homeTown.PlaceID;
            HomeTownName = homeTown.Name;
            LastTraveled = lastTraveled.FormattedAddress;
            LastTraveledID = lastTraveled.PlaceID;
            LastTraveledName = lastTraveled.Name;
            for (int i = 0; i < pastLocal.Count; i++)
            {
                if (pastLocal[i].PlaceID == null)
                {
                    FirstEmptyPastLocal = String.Format("'pastLocal{0}Input'",i);
                    break;
                }
            }
            PastLocal0 = pastLocal[0].FormattedAddress;
            PastLocal0ID = pastLocal[0].PlaceID;
            PastLocal0Name = pastLocal[0].Name;
            PastLocal1 = pastLocal[1].FormattedAddress;
            PastLocal1ID = pastLocal[1].PlaceID;
            PastLocal1Name = pastLocal[1].Name;
            PastLocal2 = pastLocal[2].FormattedAddress;
            PastLocal2ID = pastLocal[2].PlaceID;
            PastLocal2Name = pastLocal[2].Name;
            PastLocal3 = pastLocal[3].FormattedAddress;
            PastLocal3ID = pastLocal[3].PlaceID;
            PastLocal3Name = pastLocal[3].Name;
            PastLocal4 = pastLocal[4].FormattedAddress;
            PastLocal4ID = pastLocal[4].PlaceID;
            PastLocal4Name = pastLocal[4].Name;
            PastLocal5 = pastLocal[5].FormattedAddress;
            PastLocal5ID = pastLocal[5].PlaceID;
            PastLocal5Name = pastLocal[5].Name;
            PastLocal6 = pastLocal[6].FormattedAddress;
            PastLocal6ID = pastLocal[6].PlaceID;
            PastLocal6Name = pastLocal[6].Name;
            PastLocal7 = pastLocal[7].FormattedAddress;
            PastLocal7ID = pastLocal[7].PlaceID;
            PastLocal7Name = pastLocal[7].Name;
            PastLocal8 = pastLocal[8].FormattedAddress;
            PastLocal8ID = pastLocal[8].PlaceID;
            PastLocal8Name = pastLocal[8].Name;
            PastLocal9 = pastLocal[9].FormattedAddress;
            PastLocal9ID = pastLocal[9].PlaceID;
            PastLocal9Name = pastLocal[9].Name;
            for (int i = 0; i < favoritePlace.Count; i++)
            {
                if (favoritePlace[i].PlaceID == null)
                {
                    FirstEmptyFavoritePlace = String.Format("'favoritePlace{0}Input'", i);
                    break;
                }
            }
            FavoritePlace0 = favoritePlace[0].FormattedAddress;
            FavoritePlace0ID = favoritePlace[0].PlaceID;
            FavoritePlace0Name = favoritePlace[0].Name;
            FavoritePlace1 = favoritePlace[1].FormattedAddress;
            FavoritePlace1ID = favoritePlace[1].PlaceID;
            FavoritePlace1Name = favoritePlace[1].Name;
            FavoritePlace2 = favoritePlace[2].FormattedAddress;
            FavoritePlace2ID = favoritePlace[2].PlaceID;
            FavoritePlace2Name = favoritePlace[2].Name;
            FavoritePlace3 = favoritePlace[3].FormattedAddress;
            FavoritePlace3ID = favoritePlace[3].PlaceID;
            FavoritePlace3Name = favoritePlace[3].Name;
            FavoritePlace4 = favoritePlace[4].FormattedAddress;
            FavoritePlace4ID = favoritePlace[4].PlaceID;
            FavoritePlace4Name = favoritePlace[4].Name;
            DOB = userProfile.DOB.ToShortDateString();
            PrivateProfile = privateProfile;
        }
        public EditProfileViewModel(User user, UserProfile userProfile) // Value passed constructor
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            About = userProfile.About;
            HomeTownID = userProfile.HomeTown;
            DOB = userProfile.DOB.ToShortDateString();
        }
    }
}