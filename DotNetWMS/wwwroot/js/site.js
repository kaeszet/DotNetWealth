// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function ()
{
    //Active nav list
    let first = $(location).attr('pathname');

    first.indexOf(1);
    first.toLowerCase();
    first = first.split("/")[1];

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
});