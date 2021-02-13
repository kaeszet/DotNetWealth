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
    //$('.sidebar .list-group-item[href="' + location.pathname + '"]').addClass('active');

    if ($('.sidebar').length > 0)
    {

        $('.sidebar .list-group-item').each(function () {

            if ($(this).attr('href') == location.pathname) {
                localStorage.setItem('active', $(this).data('id'))
            }

            if ($(this).data('id') == localStorage.getItem('active'))
            {
                $(this).addClass('active');
            }

            $(this).on('click', function () {
                localStorage.setItem('active', $(this).data('id'))
            })
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
        $('#stocktaking').on('click', function () {

            $keyWord = $('#selectList').val();
            $('#divPartial').load('/Warehouses/Stocktaking', { warehouseFullName: $keyWord });
        })
    }

    if ($("#cookieConsent").length > 0)
    {
        $("#cookieConsent").find('button.close').on("click", function (ev) {
            document.cookie = $(this).data('cookie-string');
        });
    }


});

(function ($)
{
    $.fn.myfunction = function () {
        alert('hello world');
        return this;
    };

})(jQuery);

function initMap()
{
    console.log('asd')
    var myCenter = new google.maps.LatLng(50.046912, 19.998207);
    var mapProp = { center: myCenter, zoom: 12, scrollwheel: false, draggable: true, mapTypeId: google.maps.MapTypeId.ROADMAP };
    var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
    var marker = new google.maps.Marker({ position: myCenter });

    marker.setMap(map);

    const geocoder = new google.maps.Geocoder();
    geocodeAddress(geocoder, map);
}

function geocodeAddress(geocoder, resultsMap)
{
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