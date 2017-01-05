var map;
var service;
var markers = [];
var labels = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
var labelIndex = 0;
var distanceRadioBtn = true;

function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: -33.8688, lng: 151.2195 },
        zoom: 13,
        mapTypeId: 'roadmap'
    });

    // Try HTML5 geolocation.
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            map.setCenter(pos);
        }, function () {
            handleLocationError(true, infoWindow, map.getCenter());
        });
    } else {
        // Browser doesn't support Geolocation
        handleLocationError(false, infoWindow, map.getCenter());
    }

    // Create the search box and link it to the UI element.
    var input = document.getElementById('pac-input');
    var searchBox = new google.maps.places.SearchBox(input);

    var infowindow = new google.maps.InfoWindow();
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

    // Bias the SearchBox results towards current map's viewport.
    map.addListener('bounds_changed', function () {
        searchBox.setBounds(map.getBounds());
    });


    var radios = document.getElementsByName('distance');

    var GetResults = function () {
        places = searchBox.getPlaces();
        if (places.length == 0) {
            return;
        }

        // Reset label index
        labelIndex = 0;

        // Clear out the old markers.
        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(null);
        }
        markers = [];
        service = new google.maps.places.PlacesService(map);

        var placesList = document.getElementById("places");
        placesList.innerHTML = ""; // Clear list

        // Check radio button value
        distanceRadioBtn = radios[0].value === "miles" ? radios[0].checked : radios[1].checked;

        // Call to server for local users
        $.ajax({
            type: "GET",
            data: { lat: places[0].geometry.location.lat, lng: places[0].geometry.location.lng, radius: document.getElementById("Radius").value, measurementType: distanceRadioBtn },
            url: "/Reviews/GetLocalUsers",
            error: function (html) {
                $("#local-user-list").html("<h4>Sorry about that – there aren’t any locals from that area yet!</h4>").show();
                return;
            },
            success: function (html) {
                $("#local-user-list").html(html).show();

                // Checks to make sure there was data returned.  If there was no data, do this.
                if (document.getElementsByClassName("search-list-item").length == 0) {
                    $("#local-user-list").html("<h4>Sorry about that – there aren’t any locals from that area yet!</h4>").show();
                }
            }
        });

        // Display locals list
        document.getElementById("local-user-list").style.display = "block";

        var bounds = new google.maps.LatLngBounds();

        // Get right panel element and display it
        var rightPanel = document.getElementById("right-panel");
        rightPanel.style.display = "block";

        // Create over flow list if places is longer than 7
        if (places.length > 7) {
            placesList.style.overflowY = "scroll";
        } else {
            placesList.style.overflowY = "hidden";
        }

        // For each place, get the icon, name and location.
        places.forEach(function (place) {
            if (!place.geometry) {
                console.log("Returned place contains no geometry");
                return;
            }

            var label = labels[labelIndex++];

            //Create marker
            var marker = new google.maps.Marker({
                map: map,
                position: place.geometry.location,
                label: label
            });
            markers.push(marker);

            // HTML string for infowindow content
            var content = '<h4>' + place.name + '</h4>' +
                    '<h6>' + place.formatted_address + '</h6>' +
                    '<button class="btn" onclick="ViewPlace(' + "'" + place.place_id + "'" + ')">See Details!</button>';


            // Display info window onclick: marker
            google.maps.event.addListener(marker, 'click', function () {
                infowindow.setContent(content);
                infowindow.open(map, this);
            });

            // Create list item
            var listItem = document.createElement("li");
            listItem.innerHTML = "<strong>" + label + "</strong>" + ". " + place.name;
            placesList.appendChild(listItem);

            // Display info window onclick: list item result
            listItem.addEventListener("click", function () {
                infowindow.setContent(content);
                infowindow.open(map, marker);
            });

            if (place.geometry.viewport) {
                // Only geocodes have viewport.
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
        });
        map.fitBounds(bounds);
    };

    // Listen for the event fired when the user selects a prediction and retrieve more details for that place.

    searchBox.addListener('places_changed', function (){GetResults()});

    document.getElementById("Radius").addEventListener("mouseout", function () { GetResults() });
    
    for (var i = 0 ; i < radios.length; i++) {
        radios[i].onclick = function () { GetResults() };
    }
}

// Hide results box
function HideResults(element, display) {
    element.style.display = "none";
    document.getElementById(display).style.display = "block";
    var list = document.getElementById("places");
    if (display == "chevron-down") {
        list.style.display = "none";
    } else {
        list.style.display = "block";
    }

}


//function Test() {
//    var placesList = document.getElementById("places");
//    if (placesList.childElementCount != 0) {   
//        for (var i = 0; i < places.length; i++) {
//            setInterval(GetDetails(places[i]), 2000);            
//        }
//    }
//}


//function GetDetails(place) {
//    service.getDetails({
//        placeId: place.place_id
//    }, function (place, status) {
//        console.log(status + place);
//        if (status === google.maps.places.PlacesServiceStatus.OK) {
//            var marker = new google.maps.Marker({
//                map: map,
//                position: place.geometry.location,
//                visible: true
//            });
//            marker.setMap(map);
//            markers.push(marker);
//            google.maps.event.addListener(marker, 'click', function () {
//                infowindow.setContent('<h4>' + place.name + '</h4>' +
//                    '<h6>' + place.formatted_address + '</h6>' +
//                    '<h6>' + place.formatted_phone_number + '</h6>' +
//                    '<button class="btn" onclick="ViewPlace(' + "'" + place.place_id + "'" + ')">See Reviews!</button>');
//                infowindow.open(map, this);
//            });
//        }
//    });
//}

//function SetMarker(place) {
//    service.getDetails({
//        placeId: place.place_id
//    }, function (place, status) {
//        console.log(status + place);
//        if (status === google.maps.places.PlacesServiceStatus.OK) {
//            var marker = new google.maps.Marker({
//                map: map,
//                position: place.geometry.location,
//                visible: true
//            });
//            marker.setMap(map);
//            markers.push(marker);
//            google.maps.event.addListener(marker, 'click', function () {
//                infowindow.setContent('<h4>' + place.name + '</h4>' +
//                    '<h6>' + place.formatted_address + '</h6>' +
//                    '<h6>' + place.formatted_phone_number + '</h6>' +
//                    '<button class="btn" onclick="ViewPlace('+ "'" + place.place_id + "'" +')">See Reviews!</button>');
//                    infowindow.open(map, this);
//            });
//        }
//    });
//}


//function processResults(results, status, pagination) {
//    if (status !== google.maps.places.PlacesServiceStatus.OK) {
//        return;
//    } else {
//        createMarkers(results);
//        if (pagination.hasNextPage) {
//            var moreButton = document.getElementById('more');

//            moreButton.disabled = false;

//            moreButton.addEventListener('click', function () {
//                moreButton.disabled = true;
//                pagination.nextPage();
//            });
//        }
//    }
//}


//function createMarkers(places) {
//    var bounds = new google.maps.LatLngBounds();
//    var placesList = document.getElementById('places');

//    for (var i = 0, place; place = places[i]; i++) {

//        service.getDetails({
//            placeId: place.place_id
//        }, function (place, status) {
//            if (status === google.maps.places.PlacesServiceStatus.OK) {
//                var marker = new google.maps.Marker({
//                    map: map,
//                    position: place.geometry.location
//                });
//                markers.push(marker);
//                google.maps.event.addListener(marker, 'click', function () {
//                    infowindow.setContent('<h4>' + place.name + '</h4>' +
//                        '<h6>' + place.formatted_address + '</h6>' +
//                        '<h6>' + place.formatted_phone_number + '</h6>' +
//                        '<button class="btn" onclick="ViewPlace(' + "'" + place.place_id + "'" + ')">See Reviews!</button>');
//                    infowindow.open(map, this);
//                });
//            }
//        });
//        //var image = {
//        //    url: place.icon,
//        //    size: new google.maps.Size(71, 71),
//        //    origin: new google.maps.Point(0, 0),
//        //    anchor: new google.maps.Point(17, 34),
//        //    scaledSize: new google.maps.Size(25, 25)
//        //};

//        //var marker = new google.maps.Marker({
//        //    map: map,
//        //    icon: image,
//        //    title: place.name,
//        //    position: place.geometry.location
//        //});

//        placesList.innerHTML += '<li>' + place.name + '</li>';

//        bounds.extend(place.geometry.location);
//    }
//    map.fitBounds(bounds);
//}




//function callback(results, status) {
//    if (status === google.maps.places.PlacesServiceStatus.OK) {
//        for (var i = 0; i < results.length; i++) {
//            createMarker(results[i]);
//        }
//    }
//}



//function createMarker(place) {
//    var placeLoc = place.geometry.location;
//    var marker = new google.maps.Marker({
//        map: map,
//        position: place.geometry.location
//    });

//    google.maps.event.addListener(marker, 'click', function () {
//        infowindow.setContent(place.name);
//        infowindow.open(map, this);
//    });
//}


