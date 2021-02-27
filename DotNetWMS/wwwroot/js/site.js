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

        let _this = $(this);
        $geocoder.geocode({ address: _this.val() }, (results, status) => {
            if (status === "OK") {
                let place = results[0];

                if (_this.attr('type') == 'search') {

                    clearMarker(null);
                    $allMarkers = [];

                    let street = [];

                    _this.val(place.formatted_address);

                    $('#lat').val(place.geometry.location.lat())
                    $('#lng').val(place.geometry.location.lng())

                    console.log(place)

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

        $(window).unbind();
        $(window).on('click', function (e) {

            if ($('.sidebar').hasClass('show') && $done == true) {

                if (!$('.sidebar').is(e.target) && $('.sidebar').has(e.target).length === 0) {
                    $('.hamburger').removeClass('active');
                    hide( $('.sidebar') )
                }
            }
        });
        $(window).on('resize', function (e) {

            if ($(this).width() > 991.98) {
                $('.content-body').removeClass('blur')
            }
            else {

                if ($('.sidebar').hasClass('show')) {
                    $('.content-body').addClass('blur')
                }
            }
        });

        function hide(el) {

            $('.content-body').removeClass('blur')

            $(el).animate({
                left: - $slideX,
            }, 1000, function () {
                $(this).removeClass('show');
                $(this).css('left', '');
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
                $(this).css('left', '');
                $done = true;
            });
        }
    }

    $('.table-responsive').on('shown.bs.dropdown', function (e) {
    var $table = $(this),
        $menu = $(e.target).find('.dropdown-menu'),
        tableOffsetHeight = $table.offset().top + $table.height(),
        menuOffsetHeight = $menu.offset().top + $menu.outerHeight(true);

    if (menuOffsetHeight > tableOffsetHeight)
      $table.css("padding-bottom", menuOffsetHeight - tableOffsetHeight);
  });

  $('.table-responsive').on('hide.bs.dropdown', function () {
    $(this).css("padding-bottom", 0);
  })

})(jQuery);

// Google Maps
function initMap()
{
    let a = $('#address');
    // Geocoder
    $geocoder = new google.maps.Geocoder();

    //option for Autocomplete
    let option = {
        types: ["geocode"],
        componentRestrictions: { country: 'pl' }
    };

    //Autocomplete
    $autocomplete = new google.maps.places.Autocomplete(a[0], option);

    $allMarkers = [];

    if (a.length > 0 && a.val()) {

        a.geocodeAddress();
    }
    else {

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

    if (a.length > 0 && a.attr('type') == 'search')
    {
        a.on('change autocompletechange', function (ev) {

            $(this).geocodeAddress();
        })

        a.on('keypress', function (ev) {

            if (ev.which == 13) {

                ev.preventDefault();
                a.blur();
            }
        })
    }
}