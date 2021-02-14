// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function ()
{
    //bootstrap-select
    $('select').selectpicker({
        style: 'btn-form',
        styleBase: 'form-control'
    });

    //Active nav list
    if ($('.sidebar').length > 0) {

        $('.sidebar .list-group-item').each(function () {

            if ($(this).attr('href') == location.pathname) {

                localStorage.setItem('active', $(this).data('id'))
                $(this).addClass('active');
            }

            if ($(this).data('id') == localStorage.getItem('active')) {

                $('.sidebar .list-group-item.active').removeClass('active');
                $(this).addClass('active');
            }
        })
    }

    //Input search table
    $(".search-control").on("keyup", function ()
    {
        var value = $(this).val().toLowerCase();
        $("#tableSearch tr").filter(function ()
        {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

    $('.table-responsive').on('show.bs.dropdown', function () {
        $('.table-responsive').css("overflow-x", "inherit");
    });

    $('.table-responsive').on('hide.bs.dropdown', function () {
        $('.table-responsive').css("overflow-x", "auto");
    })

    if ($('.sidebar').length > 0) {

        $centerNav = $(".sidebar").find('.active').offset().top - (($(window).height() / 2) - ($(".sidebar").find('.active').height() / 2));
        $('.sidebar').scrollTop($centerNav)
    }

    if ($('#stocktaking').length > 0) {

        $('#stocktaking').click(function () {
            var keyWord = $('#selectList').val();
            $('#divPartial').load(url, { warehouseFullName: keyWord });
        })
    }

    if ($("#cookieConsent").length > 0){
        $("#cookieConsent").find('button.close').on("click", function (ev) {
            document.cookie = $(this).data('cookie-string');
        });
    }

    if ($('#print').length > 0) {

        $('#print').on('click', function () {
            $(this).printData();
        })
    }


});

(function ($)
{
    //geocoder function
    $.fn.geocodeAddress = function () {
        $geocoder.geocode({ address: $(this).val() }, (results, status) => {

            if (status === "OK")
            {
                clearMarker(null);
                $allMarkers = [];

                let street = [];

                const componentForm = {
                    street_number: "short_name",
                    route: "long_name",
                    locality: "long_name",
                    administrative_area_level_1: "short_name",
                    country: "long_name",
                    postal_code: "short_name",
                };

                for (const component of results[0].address_components) {
                    const addressType = component.types[0];

                    if (componentForm[addressType]) {
                        const val = component[componentForm[addressType]];

                        if ($('#' + addressType).length > 0) $('#' + addressType).val(val);
                        if (addressType == 'street_number' || addressType == 'route') street[addressType] = val;
                    }
                }
                $('#address').val(street['route'] + ' ' + street['street_number']);

                $map.setCenter(results[0].geometry.location);

                let marker = new google.maps.Marker({
                    map: $map,
                    position: results[0].geometry.location,
                });

                $allMarkers.push(marker)

            } else alert("Wystąpił błąd usługi Geocode z powodu: " + status);
        });

        function clearMarker(val)
        {
            for (let i = 0; i < $allMarkers.length; i++) {
                $allMarkers[i].setMap(val);
            }
        }
    };

    $.fn.printData = function () {

        var contentToPrint = $('#' + $(this).data('content'));
        var newWin = window.open('', 'Print-Window');

        newWin.document.open();
        newWin.document.write(
            `<html><head>
            <link rel="stylesheet" href="/css/style.css" />
            <link rel="stylesheet" href="/lib/font-awesome/css/all.css" /></head>
            <body onload="window.print()">` + contentToPrint.html() + `</body>
            </html>`
        );
        newWin.document.close();

        setTimeout(function () { newWin.close(); }, 10);
    }

})(jQuery);


// Google Maps
function initMap()
{
    $allMarkers = [];

    // my current location
    if (navigator.geolocation)
    {
        // my coordinates on the map
        navigator.geolocation.getCurrentPosition(function (pos) {

            $myLocation = new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude);

            $props = {
                center: $myLocation,
                zoom: 12,
                gestureHandling: 'cooperative'
            };

            $map = new google.maps.Map($('#map')[0], $props);
            let marker = new google.maps.Marker({
                map: $map,
                position: $myLocation
            });

            $allMarkers.push(marker);

        }, function (error) {

            console.log(error)

        });
    }
    else alert("Geolokalizacja nie jest obsługiwana przez twoją przeglądarkę.");

    // Geocoder
    $geocoder = new google.maps.Geocoder();

    if ($('#address').length > 0)
    {
        $('#address').on('change autocompletechange', function (ev) {

            $(this).geocodeAddress($(this).val());
        })

        $('#address').on('keypress', function (ev) {
            if (ev.which == 13) {
                ev.preventDefault();
                $('#address').blur();
            }
        })
    }
}