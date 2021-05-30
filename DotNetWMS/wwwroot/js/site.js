// Please see documentation at https://docs.microsoft.com/aspnet/core/client-
// bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function ()
{
    //bootstrap-select
    $('select').selectpicker({
        style: 'btn-form',
        styleBase: 'form-control',
        size: 10,
    });

    // Hamburger button
    $('.hamburger').menuCollapse();

    if ($('.chart-diagram').length) {

        //Chart diagram
        $('.chart-diagram').createChart();
    }

    $('[data-toggle="tooltip"]').tooltip();

    //Active nav list

    $('.sidebar .list-group-item, #navbarTop .nav-link').each(function () {
        sessionStorage.setItem('pathname', location.pathname)

        if ($(this).attr('href') == location.pathname) {

            sessionStorage.setItem('active', $(this).data('id'))
            $(this).addClass('active');
        }

        if (location.pathname == '/') {

            sessionStorage.removeItem('active');
        }
        else if ($(this).data('id') == sessionStorage.getItem('active')) {
            console.log(this)
            $('.sidebar .list-group-item.active').removeClass('active');
            $(this).addClass('active');
        }
        else if ($(this).attr('href') == location.pathname) {
            $(this).addClass('active');
		}


    })

    // Global alert info
    if ($('.alert.global-info').length) {

        setTimeout(function () {
            $('.alert.global-info').alert('close');
        }, 5000);
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

    if ($('.sidebar').length && $(".sidebar").find('.active').length) {

        $centerNav = $(".sidebar").find('.active').offset().top - (($(window).height() / 2) - ($(".sidebar").find('.active').height() / 2));
        $('.sidebar').scrollTop($centerNav)
    }

    // Stocktaking
    $('#stocktaking').click(function () {
        var keyWord = $('#selectList').val();
        $('#divPartial').load(url, { warehouseFullName: keyWord }, function () {

            $('#print').on('click', function () {
                $(this).printData();
            })
        });

    })

    $("#cookieConsent").find('button.close').on("click", function (ev) {
        document.cookie = $(this).data('cookie-string');
    });

    $('#print').on('click', function () {
        $(this).printData();
    })

    $('.table-responsive').on('shown.bs.dropdown', function (e) {
        let $table = $(this),
            $menu = $(e.target).find('.dropdown-menu'),
            tableOffsetHeight = $table.offset().top + $table.height(),
            menuOffsetHeight = $menu.offset().top + $menu.outerHeight(true);

        console.log($menu)

        if (menuOffsetHeight > tableOffsetHeight)
            $table.css("padding-bottom", (menuOffsetHeight - tableOffsetHeight) + 20);
    });

    $('.table-responsive').on('hide.bs.dropdown', function () {
        $(this).css("padding-bottom", 0);
    })

    $('.role-manage').on('change', function () {
        checkboxRoleChange($(this))
    })

});

(function ($)
{
    $.fn.createChart = function (options) {
        
        let _this = $(this);

        let settings = $.extend({
            url: null,
            type: null,
        }, options);

        let defaultOptions = {
            legend: {
                display: false,
            },
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true,
                        stepSize: 1,
                    }
                }],
                xAxes: [{
                    ticks: {
                        callback: function (label, index, labels) {
                            if (/\s/.test(label)) {

                                return label.split(" ");
                            } else {
                                return label;
                            }
                        }
                    }
                }]
            },
        };

        if (_this.length > 1) {

            _this.each(function (key, item) {
                if (settings.url || $(item).data('id')) {

                    $.ajax({
                        method: "GET",
                        url: 'Home/Diagrams?number=' + (settings.url ? settings.url : $(item).data('id')),
                    })
                        .done(function (respose) {

                            let data = {
                                labels: [],
                                data: [],
                            }

                            $(respose).each(function (key) {

                                data.labels.push(this.label)
                                data.data.push(this.data)
                            })

                            if ($(item).data('id') == 1 ||
                                $(item).data('id') == 3) createDefaultChart(data, $(item))
                            else if ($(item).data('id') == 4) createWarrantyDate(data, $(item))
                        })
                }
            })
        }

        function createDefaultChart(data, item) {
            let chart = new Chart(item, {
                type: item.data('type'),
                data: {

                    labels: data.labels,
                    datasets: [{

                        label: false,
                        backgroundColor: 'rgb(0, 89, 220, 0.6)',
                        borderColor: 'rgb(0, 89, 220)',
                        data: data.data,
                    }],
                },
                options: defaultOptions
            })
        }
        function createWarrantyDate(data, item) {

            let chart = new Chart(item, {
                type: item.data('type'),
                data: {

                    labels: data.labels,
                    datasets: [{
                        label: false,
                        backgroundColor:
                            function (context) {

                                var index = context.dataIndex;
                                var value = context.dataset.data[index];

                                return value < 0 ? 'rgb(40, 167, 69, 0.6)' : 'rgb(220, 53, 69, 0.6)';
                            },
                        borderColor:
                            function (context) {

                                var index = context.dataIndex;
                                var value = context.dataset.data[index];

                                return value < 0 ? 'rgb(40, 167, 69, 0.6)' : 'rgb(220, 53, 69, 0.6)';
                            },
                        data: data.data,
                    }],
                },
                options: {
                    responsive: true,
                    legend: {
                        display: false,
                    },
                    indexAxis: 'y',
                    scales: {

                        yAxes: [
                            {
                                ticks: {
                                    suggestedMax: 50,
                                    suggestedMin: -50
                                },
                                barPercentage: 0.5,
                            }
                        ],
                        xAxes: [{
                            stacked: true,
                            gridLines: {
                                display: true,
                                color: "rgba(255,99,132,0.2)"
                            },
                        }]
                    }
                }
            })
        }
    },

    //geocoder plugin
    $.fn.geocodeAddress = function (options) {

        let settings = $.extend({
            lat: null,
            lng: null,
            search: false,
        }, options);

        let _this = $(this);
        let LatLng;

        if (settings.lat && settings.lng) {
            LatLng = new google.maps.LatLng(settings.lat, settings.lng);
        }
        else LatLng = null;

        let search = settings.search ? { address: _this.val() } : { location: LatLng }

        $geocoder.geocode(search, (results, status) => {
            if (status === "OK") {
           
                let place = results[0];

                if (settings.search == true) {

                    getGoogleData(place)
                }

                newMarker(place);

            } else alert("Wystąpił błąd usługi Geocode z powodu: " + status);
        });
    };

    $.fn.printData = function () {

        let ToPrint = $('#' + $(this).data('content'));
        let Warehouse = $('#selectList option:selected').text();
        let title = $(this).data('title');

        let newWin = window.open('', 'Print-Window');

        $('body').append('<div id="contentToPrint" style="display: none;"></div>');
        let newContent = $('#contentToPrint');
        newContent.append(ToPrint.html())
        newContent.find('[data-hidden=true]').remove();
        newContent.find('.d-none').removeClass('d-none');

        let signature = `<div class="form-row text-center">
            <div class="form-group col-md-3 mr-auto">
                <h2>......................</h2>
                <h6>Podpis wydającego</h6>
            </div>
            <div class="form-group col-md-3">
                <h2>......................</h2>
                <h6>Podpis odbiorcy</h6>
            </div>
        </div>`

        if ($(this).data('signature') == true ||
            $(this).data('signature') == 'true') newContent.append(signature)

        newWin.document.open();
        newWin.document.write(
            `<html><head>
            <link rel="stylesheet" href="/css/style.css" />
            <link rel="stylesheet" href="/lib/font-awesome/css/all.css" /></head>
            <body onload="window.print()">
            <h4>`+ title +`</h4>
            <hr>
            <h6>`+ Warehouse +`</h6>`
            + newContent.html() + 
            `</body>
            </html>`
        );
        newWin.document.close();
        newContent.remove();

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
                left: - ($(el).width() + 50),
            }, 1000, function () {
                $(this).removeClass('show');
                $(this).css('left', '');
                $done = true;
            });
        }
        function show(el) {

            $('.content-body').addClass('blur')
            $(el).addClass('show')
            .css({
                left: - ($(el).width() + 50),
            })
            .animate({
                left: 0,
            }, 1000, function () {
                $(this).css('left', '');
                $done = true;
            });
        }
    }

})(jQuery);

//Stocktasking table
function checkboxChangeInTableRow(obj) {

    var checkboxes = $(obj).closest('tr').find("input:checkbox");
    checkboxes.each(function () {
        if ($(this).attr('id') != $(obj).attr('id')) {
            $(this).prop('checked', false);
            $(this).attr('checked', false);
        }
        else {
            $(this).attr('checked', true);
        }
    })
}
//Manage User Roles
function checkboxChange(obj) {

    var checkboxes = $("input:checkbox");
    checkboxes.each(function () {
        if ($(this).attr('id') != $(obj).attr('id')) {
            $(this).prop('checked', false);
            $(this).attr('checked', false);
        }
        else
        {
            $(this).attr('checked', true);
        }
    })
}

// Change custom role checkbox
function checkboxRoleChange(data) {

    let active = data.data('type');

    $('.role-manage').each(function () {
        if ($(this).attr('id') != data.attr('id')) {

            if (active == 'Kadry') {

                if ($(this).data('type') != 'Standard' && $(this).data('type') != 'StandardPlus') {
                    $(this).prop('checked', false);
                    $(this).attr('checked', false);
                }
            }
            else if (active == 'Admin' || active == 'Moderator') {

                $(this).prop('checked', false);
                $(this).attr('checked', false);
            }
            else if (active == 'Standard' || active == 'StandardPlus') {   

                if ($(this).data('type') != 'Kadry') {
                    $(this).prop('checked', false);
                    $(this).attr('checked', false);
                }
            }
        }
    })
}

// Google Maps
function initMap() {

    let a = $('#address'),
        lat = $('#lat'),
        lng = $('#lng');

    // Geocoder
    $geocoder = new google.maps.Geocoder();

    //option for Autocomplete
    let option = {
        types: ["geocode"],
        componentRestrictions: { country: 'pl' }
    };

    //Autocomplete
    $autocomplete = new google.maps.places.Autocomplete(a[0], option);

    $autocomplete.addListener("place_changed", () => {
        const place = $autocomplete.getPlace();

        getGoogleData(place)
    });

    $allMarkers = [];

    if (a.length > 0 && (lat.val() && lng.val())) {

        $props = {
            zoom: 14,
            gestureHandling: 'cooperative'
        };
        $map = new google.maps.Map($('#map')[0], $props);

        a.geocodeAddress({
            lat: lat.val(),
            lng: lng.val(),
        });
    }
    else {

        // my current location
        if (navigator.geolocation) {

            navigator.geolocation.getCurrentPosition(function (pos) {

                $myLocation = new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude);

                $props = {
                    center: $myLocation,
                    zoom: 14,
                    gestureHandling: 'cooperative'
                };

                // Create map
                $map = new google.maps.Map($('#map')[0], $props);

                let marker = new google.maps.Marker({
                    map: $map,
                    position: $myLocation
                });

                // Update marker
                $allMarkers.push(marker);

            }, function (error) {

                console.log(error)

            });
        }
        else alert("Geolokalizacja nie jest obsługiwana przez twoją przeglądarkę.");
    }

    if (a.length > 0 && a.attr('type') == 'search') {

        a.on('keypress', function (ev) {

            if (ev.which == 13) {

                ev.preventDefault();
                a.blur();
            }
        })
    }
}

function clearMarker(val) {
    for (let i = 0; i < $allMarkers.length; i++) {
        $allMarkers[i].setMap(val);
    }
}

function newMarker(place) {

    $map.setCenter(place.geometry.location);

    let marker = new google.maps.Marker({
        map: $map,
        position: place.geometry.location,
    });

    $allMarkers.push(marker);
}

function getGoogleData(place) {
    console.log(place)
    if (place.geometry) {

        clearMarker(null);
        $allMarkers = [];

        let street = [];

        $('#address').val(place.formatted_address);

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
        if (Number.isInteger(Number(street['premise']))) {
            $('#street').val(street['locality'] + ' ' + street['premise']);

        } else {
            if (typeof street['street_number'] != "undefined") $('#street').val(street['route'] + ' ' + street['street_number']);
            else $('#street').val(street['route']);

        }

        newMarker(place);
    }
    else {
        $('#address').geocodeAddress({ search: true });
    }
}