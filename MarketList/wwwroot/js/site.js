// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(window).on('load resize', function (e) {
    let body = $('body');
    switch (e.type) {
        case 'load':
            break;
        case 'resize':
            if (body.hasClass('desktop'))
                body.removeClass('desktop');

            if (body.hasClass('mobile'))
                body.removeClass('mobile');
            break;
        default:
            break;
    }
    let width = $(window).width();
    if (width > 800) {
        body.addClass('desktop');
    }
    else {
        body.addClass('mobile');
    }
});
