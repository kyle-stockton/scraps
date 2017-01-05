// Google Autocomplete for Geolocations only. Used for registration and editprofile

var placeSearch, autocomplete;

function initAutocomplete() {
    // Create the autocomplete object, restricting the search to geographical
    // location types.
    autocomplete = new google.maps.places.Autocomplete(
        document.getElementById('autocomplete'),
        { types: ['(cities)'] }); // For geolocations only - Excludes business, etc.
    // When the user selects an address from the dropdown, populate the address fields in the form.
    autocomplete.addListener('place_changed', fillInAddress);
}

function fillInAddress() {
    // Get the place details from the autocomplete object.
    var place = autocomplete.getPlace();
    var autoCompleteInput = document.getElementById('autoCompleteInput');
    var locInput = autoCompleteInput.getAttribute('locInput');

    var locationLabel = document.getElementById(locInput);
    document.getElementById('edit-submit-btn').setAttribute('disabled', 'true');
    locationLabel.style.display = "block";
    // formerly invisible fields will have to have Bootstrap classes added
    var newClasses = "col-md-8 col-md-offset-4";
    $(locationLabel).find("h4").text(place.formatted_address);

    autoCompleteInput.style.display = "none";

    switch (locInput) {
        case "homeTownInput": {
            document.getElementById("HomeTownID").value = place.place_id;
            document.getElementById("HomeTownName").value = place.name;
            document.getElementById("HomeTownLat").value = place.geometry.location.lat();
            document.getElementById("HomeTownLng").value = place.geometry.location.lng();
            break;
        }
        case "pastLocal0Input": {
            document.getElementById("PastLocal0ID").value = place.place_id;
            document.getElementById("PastLocal0Name").value = place.name;
            document.getElementById("PastLocal0Lat").value = place.geometry.location.lat();
            document.getElementById("PastLocal0Lng").value = place.geometry.location.lng();
            break;
        }
        case "pastLocal1Input": {
            document.getElementById("PastLocal1ID").value = place.place_id;
            document.getElementById("PastLocal1Name").value = place.name;
            document.getElementById("PastLocal1Lat").value = place.geometry.location.lat();
            document.getElementById("PastLocal1Lng").value = place.geometry.location.lng();
            $(locationLabel).addClass(newClasses);
            break;
        }
        case "pastLocal2Input": {
            document.getElementById("PastLocal2ID").value = place.place_id;
            document.getElementById("PastLocal2Name").value = place.name;
            document.getElementById("PastLocal2Lat").value = place.geometry.location.lat();
            document.getElementById("PastLocal2Lng").value = place.geometry.location.lng();
            $(locationLabel).addClass(newClasses);
            break;
        }
        case "pastLocal3Input": {
            document.getElementById("PastLocal3ID").value = place.place_id;
            document.getElementById("PastLocal3Name").value = place.name;
            document.getElementById("PastLocal3Lat").value = place.geometry.location.lat();
            document.getElementById("PastLocal3Lng").value = place.geometry.location.lng();
            $(locationLabel).addClass(newClasses);
            break;
        }
        case "pastLocal4Input": {
            document.getElementById("PastLocal4ID").value = place.place_id;
            document.getElementById("PastLocal4Name").value = place.name;
            document.getElementById("PastLocal4Lat").value = place.geometry.location.lat();
            document.getElementById("PastLocal4Lng").value = place.geometry.location.lng();
            $(locationLabel).addClass(newClasses);
            break;
        }
        case "pastLocal5Input": {
            document.getElementById("PastLocal5ID").value = place.place_id;
            document.getElementById("PastLocal5Name").value = place.name;
            document.getElementById("PastLocal5Lat").value = place.geometry.location.lat();
            document.getElementById("PastLocal5Lng").value = place.geometry.location.lng();
            $(locationLabel).addClass(newClasses);

            break;
        }
        case "pastLocal6Input": {
            document.getElementById("PastLocal6ID").value = place.place_id;
            document.getElementById("PastLocal6Name").value = place.name;
            document.getElementById("PastLocal6Lat").value = place.geometry.location.lat();
            document.getElementById("PastLocal6Lng").value = place.geometry.location.lng();
            $(locationLabel).addClass(newClasses);
            break;
        }
        case "pastLocal7Input": {
            document.getElementById("PastLocal7ID").value = place.place_id;
            document.getElementById("PastLocal7Name").value = place.name;
            document.getElementById("PastLocal7Lat").value = place.geometry.location.lat();
            document.getElementById("PastLocal7Lng").value = place.geometry.location.lng();
            $(locationLabel).addClass(newClasses);
            break;
        }
        case "pastLocal8Input": {
            document.getElementById("PastLocal8ID").value = place.place_id;
            document.getElementById("PastLocal8Name").value = place.name;
            document.getElementById("PastLocal8Lat").value = place.geometry.location.lat();
            document.getElementById("PastLocal8Lng").value = place.geometry.location.lng();
            $(locationLabel).addClass(newClasses);
            break;
        }
        case "pastLocal9Input": {
            document.getElementById("PastLocal9ID").value = place.place_id;
            document.getElementById("PastLocal9Name").value = place.name;
            document.getElementById("PastLocal9Lat").value = place.geometry.location.lat();
            document.getElementById("PastLocal9Lng").value = place.geometry.location.lng();
            $(locationLabel).addClass(newClasses);
            break;
        }
        case "favoritePlace0Input": {
            document.getElementById("FavoritePlace0ID").value = place.place_id;
            document.getElementById("FavoritePlace0Name").value = place.name;
            document.getElementById("FavoritePlace0Lat").value = place.geometry.location.lat();
            document.getElementById("FavoritePlace0Lng").value = place.geometry.location.lng();
            break;
        }
        case "favoritePlace1Input": {
            document.getElementById("FavoritePlace1ID").value = place.place_id;
            document.getElementById("FavoritePlace1Name").value = place.name;
            document.getElementById("FavoritePlace1Lat").value = place.geometry.location.lat();
            document.getElementById("FavoritePlace1Lng").value = place.geometry.location.lng();
            $(locationLabel).addClass(newClasses);
            break;
        }
        case "favoritePlace2Input": {
            document.getElementById("FavoritePlace2ID").value = place.place_id;
            document.getElementById("FavoritePlace2Name").value = place.name;
            document.getElementById("FavoritePlace2Lat").value = place.geometry.location.lat();
            document.getElementById("FavoritePlace2Lng").value = place.geometry.location.lng();
            $(locationLabel).addClass(newClasses);
            break;
        }
        case "favoritePlace3Input": {
            document.getElementById("FavoritePlace3ID").value = place.place_id;
            document.getElementById("FavoritePlace3Name").value = place.name;
            document.getElementById("FavoritePlace3Lat").value = place.geometry.location.lat();
            document.getElementById("FavoritePlace3Lng").value = place.geometry.location.lng();
            $(locationLabel).addClass(newClasses);
            break;
        }
        case "favoritePlace4Input": {
            document.getElementById("FavoritePlace4ID").value = place.place_id;
            document.getElementById("FavoritePlace4Name").value = place.name;
            document.getElementById("FavoritePlace4Lat").value = place.geometry.location.lat();
            document.getElementById("FavoritePlace4Lng").value = place.geometry.location.lng();
            $(locationLabel).addClass(newClasses);
            break;
        }
        case "lastTraveledInput": {
            document.getElementById("LastTraveledID").value = place.place_id;
            document.getElementById("LastTraveledName").value = place.name;
            document.getElementById("LastTraveledLat").value = place.geometry.location.lat();
            document.getElementById("LastTraveledLng").value = place.geometry.location.lng();
            break;
        }
        default:
            break;
    }

    document.getElementById('autocomplete').value = "";
    document.getElementById('edit-submit-btn').removeAttribute('disabled');
}


// Bias the autocomplete object to the user's geographical location,
// as supplied by the browser's 'navigator.geolocation' object.
function geolocate() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var geolocation = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            var circle = new google.maps.Circle({
                center: geolocation,
                radius: position.coords.accuracy
            });
            autocomplete.setBounds(circle.getBounds());
        });
    }
}