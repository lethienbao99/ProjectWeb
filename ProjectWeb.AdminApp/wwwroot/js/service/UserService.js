$(function () {
    var PlaceHolderElement = $('#PlaceHolderHere');
    $('button[data-toggle="modal"]').click(function (event) {
        var url = $(this).data('url');
        var decodeURL = decodeURIComponent(url);
        $.get(decodeURL).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show');
        })
    })


    PlaceHolderElement.on('click', '[data-save="modal"]', function (event) {
            event.preventDefault();
            var form = $(this).parents('.modal').find('form');
            var actionURL = form.attr('action');
            var sendData = form.serialize();
            $.post(actionURL, sendData).done(function (data) {
                PlaceHolderElement.find('.modal').modal('hide');
                location.reload();
            })
        }
    )

})