//set map options

function initMap() {
    var mylatlng = { lat: 10.776530, lng: 106.700980};
    var mapOptions = {
        center: mylatlng,
        zoom: 7,
        mapTypeId: google.maps.MapTypeId.ROADMAP // nếu là 'roadmap' thì phải viết là google.maps.MapTypeId.ROADMAP
    };
//create Map

var map = new google.maps.Map(document.getElementById("googleMap"),mapOptions)

var directionsService = new google.maps.DirectionsService();

var directionsDisplay = new google.maps.DirectionsRenderer();

directionsDisplay.setMap(map);
initMap();
}

function calcRoute() {
    var request = {
        diemdi: document.getElementById("from").value,
        diemden: document.getElementById("to").value,
        travelMode: google.maps.TravelMode.DRIVING, //WALKING, BYCYCLING AND TRANSIT
        unitSystem: google.maps.UnitSystem.IMPERIAL // Đã sửa thành IMPERIAL
    }
    directionsService.route(request, function(result, status) {
        if (status == google.maps.DirectionsStatus.OK) { // Thêm google.maps trước DirectionsStatus.OK
            const output = document.querySelector('#output')
            output.innerHTML = "<div class='alert-info'> From: " + document.getElementById("from").value + " .<br />To: " + document.getElementById("to").value + ". <br /> Driving distance:" + result.routes[0].legs[0].distance.text + ".<br />Duration :" + result.routes[0].legs[0].duration.text + ". <div>";

            directionsDisplay.setDirections(result);
        } else {
            //delete route from map
            directionsDisplay.setDirections({ routes: [] });

            //center map in spain 
            map.setCenter(mylatlng); // Đã sửa thành mylatlng

            output.innerHTML = "<div class='alert-danger'> Could not retrieve driving distance. </div>";
        }
    });
}
var options = {
    types: ['(cities)']
}

var input1 = document.getElementById("from");
var autocomplete1 = new google.maps.places.Autocomplete(input1, options)

var input2 = document.getElementById("to");
var autocomplete2 = new google.maps.places.Autocomplete(input2, options)
