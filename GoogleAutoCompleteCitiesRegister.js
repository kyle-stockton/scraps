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

    // Get ID of the chosen place. This value will be saved into the Place Table as PlaceID.
    document.getElementById("PlaceID").value = place.place_id;
    document.getElementById("PlaceName").value = place.name;
    document.getElementById("Lat").value = place.geometry.location.lat();
    document.getElementById("Lng").value = place.geometry.location.lng();

    document.getElementById("register-btn").removeAttribute("disabled");
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

//document.getElementById("Password").addEventListener("click", function () {
//    var el = this.nextElementSibling;
//    el.style.display = "block";
//});