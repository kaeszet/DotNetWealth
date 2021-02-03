var adressToGeoCode = $("#adress").data("value");

function initMap() {
    var myCenter = new google.maps.LatLng(50.046912, 19.998207);
    var mapProp = { center: myCenter, zoom: 12, scrollwheel: false, draggable: true, mapTypeId: google.maps.MapTypeId.ROADMAP };
    var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
    var marker = new google.maps.Marker({ position: myCenter });

    marker.setMap(map);

    const geocoder = new google.maps.Geocoder();
    geocodeAddress(geocoder, map);
}

function geocodeAddress(geocoder, resultsMap) {
    const address = adressToGeoCode;
    geocoder.geocode({ address: address }, (results, status) => {
        if (status === "OK") {
            resultsMap.setCenter(results[0].geometry.location);
            new google.maps.Marker({
                map: resultsMap,
                position: results[0].geometry.location,
            });
        } else {
            alert(
                "Wystąpił błąd usługi Geocode z powodu: " + status
            );
        }
    });
}