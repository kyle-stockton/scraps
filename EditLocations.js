function EditLocation(ID, displayElement) {
    var locationElements = document.getElementsByClassName('location-display');
    for (var i = 0; i < locationElements.length; i++) {
        locationElements[i].style.display = "block";
    }

    var locationInput = document.getElementById(ID);
    locationInput.style.display = "none";

    var autocomplete = document.getElementById('autoCompleteInput');
    autocomplete.setAttribute('locInput', ID);
    switch (ID) {
        case "homeTownInput": {
            autocomplete.firstElementChild.innerText = "Local Of";
            document.getElementById('autocomplete').value = locationInput.firstElementChild.nextElementSibling.firstElementChild.innerText;
            break;
        }
        case "pastLocal0Input": {
            autocomplete.firstElementChild.innerText = "Past Local Of";
            document.getElementById('autocomplete').value = locationInput.firstElementChild.nextElementSibling.firstElementChild.innerText;
            break;
        }
        case "pastLocal1Input": {
            autocomplete.firstElementChild.innerText = "";
            document.getElementById('autocomplete').value = locationInput.firstElementChild.innerText;
            break;
        }
        case "pastLocal2Input": {
            autocomplete.firstElementChild.innerText = "";
            document.getElementById('autocomplete').value = locationInput.firstElementChild.innerText;
            break;
        }
        case "pastLocal3Input": {
            autocomplete.firstElementChild.innerText = "";
            document.getElementById('autocomplete').value = locationInput.firstElementChild.innerText;
            break;
        }
        case "pastLocal4Input": {
            autocomplete.firstElementChild.innerText = "";
            document.getElementById('autocomplete').value = locationInput.firstElementChild.innerText;
            break;
        }
        case "pastLocal5Input": {
            autocomplete.firstElementChild.innerText = "";
            document.getElementById('autocomplete').value = locationInput.firstElementChild.innerText;
            break;
        }
        case "pastLocal6Input": {
            autocomplete.firstElementChild.innerText = "";
            document.getElementById('autocomplete').value = locationInput.firstElementChild.innerText;
            break;
        }
        case "pastLocal7Input": {
            autocomplete.firstElementChild.innerText = "";
            document.getElementById('autocomplete').value = locationInput.firstElementChild.innerText;
            break;
        }
        case "pastLocal8Input": {
            autocomplete.firstElementChild.innerText = "";
            document.getElementById('autocomplete').value = locationInput.firstElementChild.innerText;
            break;
        }
        case "pastLocal9Input": {
            autocomplete.firstElementChild.innerText = "";
            document.getElementById('autocomplete').value = locationInput.firstElementChild.innerText;
            break;
        }
        case "favoritePlace0Input": {
            autocomplete.firstElementChild.innerText = "Favorite Place";
            document.getElementById('autocomplete').value = locationInput.firstElementChild.nextElementSibling.firstElementChild.innerText;
            break;
        }
        case "favoritePlace1Input": {
            autocomplete.firstElementChild.innerText = "";
            document.getElementById('autocomplete').value = locationInput.firstElementChild.innerText;
            break;
        }
        case "favoritePlace2Input": {
            autocomplete.firstElementChild.innerText = "";
            document.getElementById('autocomplete').value = locationInput.firstElementChild.innerText;
            break;
        }
        case "favoritePlace3Input": {
            autocomplete.firstElementChild.innerText = "";
            document.getElementById('autocomplete').value = locationInput.firstElementChild.innerText;
            break;
        }
        case "favoritePlace4Input": {
            autocomplete.firstElementChild.innerText = "";
            document.getElementById('autocomplete').value = locationInput.firstElementChild.innerText;
            break;
        }
        case "lastTraveledInput": {
            autocomplete.firstElementChild.innerText = "Last Traveled";
            document.getElementById('autocomplete').value = locationInput.firstElementChild.nextElementSibling.firstElementChild.innerText;
            break;
        }
        default:
            break;
    }
    $(locationInput).after(autocomplete);
    autocomplete.style.display = "block";
}

function AddPastLocalLocation(ID, displayElement){
    document.getElementById('add-past-btn').style.display = "none";
    EditLocation(ID, displayElement);
}

function AddFavoritePlaceLocation(ID, displayElement) {
    document.getElementById('add-favorite-btn').style.display = "none";
    EditLocation(ID, displayElement);
}

