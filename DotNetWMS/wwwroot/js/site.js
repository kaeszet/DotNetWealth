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
    $('.sidebar .list-group-item[href="' + location.pathname + '"]').addClass('active');

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

    $centerNav = $(".sidebar").find('.active').offset().top - (($(window).height() / 2) - ($(".sidebar").find('.active').height() / 2));

    if ($('.sidebar').length > 0) {
        //$('.sidebar').animate({
        //    scrollTop: $centerNav
        //}, 500);
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
        $("#cookieConsent").find('button[data-cookie-string]').on("click", function (ev) {
            console.log($(this).data('cookie-string'))
            //document.cookie = $(this).data('cookie-string');
        }, false);
    }
});