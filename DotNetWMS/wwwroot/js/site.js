// Please see documentation at https://docs.microsoft.com/aspnet/core/client-
// bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function ()
{
    //bootstrap-select
    $('select').selectpicker({
        style: 'btn-form',
        styleBase: 'form-control',
        size: 10,
    });

    // Hamburger button
    $('.hamburger').menuCollapse();

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
    $.fn.geocodeAddress = function (type) {

        let _this = $(this);
        $geocoder.geocode({ address: _this.val() }, (results, status) => {
            if (status === "OK") {
                let place = results[0];

                if (type == 'find') {

                    clearMarker(null);
                    $allMarkers = [];

                    let street = [];

                    _this.val(place.formatted_address);

                    $('#lat').val(place.geometry.location.lat())
                    $('#lng').val(place.geometry.location.lng())

                    const componentForm = {
                        street_number: "short_name",
                        route: "long_name",
                        locality: "long_name",
                        administrative_area_level_1: "short_name",
                        country: "long_name",
                        postal_code: "short_name",
                        premise: "short_name",
                    };
                    for (const component of place.address_components) {
                        const addressType = component.types[0];

                        if (componentForm[addressType]) {
                            const val = component[componentForm[addressType]];
                            
                            if ($('#' + addressType).length > 0) $('#' + addressType).val(val);

                            if (addressType == 'street_number' ||
                                addressType == 'route' ||
                                addressType == 'premise' ||
                                addressType == 'locality') street[addressType] = val;
                        }
                    }
                    if (Number.isInteger( Number(street['premise']) )) {
                        $('#street').val(street['locality'] + ' ' + street['premise']);

                    } else {
                        if (typeof street['street_number'] != "undefined") $('#street').val(street['route'] + ' ' + street['street_number']);
                        else $('#street').val(street['route']);
                        
                    }
                    console.log(street)
                }

                $props = {
                    center: place.geometry.location,
                    zoom: 14,
                    gestureHandling: 'cooperative'
                };

                $map = new google.maps.Map($('#map')[0], $props);

                $map.setCenter(place.geometry.location);

                let marker = new google.maps.Marker({
                    map: $map,
                    position: place.geometry.location,
                });

                $allMarkers.push(marker);

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
            <body onload="window.print()">
            <h4>Inwentaryzacja</h4>`
            + contentToPrint.html() + 
            `</body>
            </html>`
        );
        newWin.document.close();

        setTimeout(function () { newWin.close(); }, 10);
    }
    
    $.fn.menuCollapse = function () {
        $done = true;

        $(this).on('click', function () {
            $id = $(this).data('target');

            if ($done == true) {

                $done = false;
                $(this).toggleClass('active');

                $slideX = $($id).width() + 50;

                if ($(this).hasClass('active')) {

                    show($id, $slideX)
                }
                else hide($id, $slideX)
            }
        })

        $(window).unbind('click');
        $(window).on('click', function (e) {

            if ($('.sidebar').hasClass('show') && $done == true) {

                if (!$('.sidebar').is(e.target) && $('.sidebar').has(e.target).length === 0) {
                    $('.hamburger').removeClass('active');
                    hide( $('.sidebar') )
                }
            }
        });

        function hide(el) {

            $('.content-body').removeClass('blur')

            $(el).animate({
                left: - $slideX,
            }, 1000, function () {
                $(this).removeClass('show');
                $done = true;
            });
        }
        function show(el) {

            $('.content-body').addClass('blur')

            $(el).css({
                'left': - $slideX,
            })
                .addClass('show')
                .animate({
                    left: 0,
                }, 1000, function () {
                    $done = true;
                });
        }
    }

})(jQuery);

// Google Maps
function initMap()
{
    // Geocoder
    $geocoder = new google.maps.Geocoder();

    //option for Autocomplete
    let option = {
        types: ["geocode"],
        componentRestrictions: { country: 'pl' }
    };
    //Autocomplete
    $autocomplete = new google.maps.places.Autocomplete($('#address')[0], option);

    $allMarkers = [];

    if ($('#addressDetails').length > 0 && $('#addressDetails').val()) {

        $('#addressDetails').geocodeAddress();
    }
    else if ($('#address').length > 0 && !$('#address').val()) {

        // my current location
        if (navigator.geolocation) {
            // my coordinates on the map
            navigator.geolocation.getCurrentPosition(function (pos) {

                $myLocation = new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude);

                $props = {
                    center: $myLocation,
                    zoom: 14,
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
    }
    else {
        $('#address').geocodeAddress('find', $(''));
    }

    if ($('#address').length > 0)
    {
        $('#address').on('change autocompletechange', function (ev) {

            $(this).geocodeAddress('find');
        })

        $('#address').on('keypress', function (ev) {

            if (ev.which == 13) {

                ev.preventDefault();
                $('#address').blur();
            }
        })
    }
}