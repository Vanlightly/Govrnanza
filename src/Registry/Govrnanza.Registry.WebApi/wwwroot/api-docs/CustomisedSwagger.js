jQuery(function ($) {
    $('.info_description > h2').prepend("+/- ");

    // hide all contents of each heading
    var firstElement = $('.info_description > h2 :first');
    var currentElement = firstElement.next();
    while (currentElement.attr('class') != 'info_name' && currentElement.prop("tagName") != undefined) {
        if (currentElement.prop("tagName") != 'H2') {
            currentElement.stop(true, true).slideToggle(10);
            currentElement.hide();
        }

        currentElement = currentElement.next();
    }

    // toggle contents of the heading clicked
    $('.info_description > h2').click(function () {
        var currentElement = $(this).next();
        while (currentElement.prop("tagName") != 'H2' && currentElement.next().attr('class') != 'info_name' && currentElement.prop("tagName") != undefined) {
            currentElement.stop(true, true).slideToggle(10);
            currentElement = currentElement.next();
        }
    });
})