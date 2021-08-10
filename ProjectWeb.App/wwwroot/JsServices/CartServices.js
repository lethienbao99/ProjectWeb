var CartServices = function () {
    this.initialize = function () {
        const currentCulture = $('#currentCulture').val();
        const URL = '/' + currentCulture + '/Cart/GetListCartItems';
        $.ajax({
            type: 'GET',
            url: URL,
            success: function (res) {
                $('#totalInCart').text(res.length);
            },
            error: function () {
                $('#totalInCart').text(0);
            }
        });
    }
}


$('body').on('click', '.addtocart1', function (e) {
    e.preventDefault();
    const id = $(this).data('id');
    const currentCulture = $('#currentCulture').val();
    const URL = '/' + currentCulture + '/Cart/AddToCart';
    $.ajax({
        type: 'POST',
        url: URL,
        data: {
            id : id,
        },
        success: function (res) {
            $('#totalInCart').text(res.length);
        }
    });
})


$('body').on('click', '.cart-pc', function (e) {
    loadData();
});





$('body').on('click', '.minusbutton123', function (e) {

    const id = $(this).data('id');
    console.log(id);
    const currentCulture = $('#currentCulture').val();
    const URL = '/' + currentCulture + '/Cart/UpdateCart';
    $.ajax({
        type: 'POST',
        url: URL,
        data: {
            id: id,
            status: "minus"
        },
        async: false,
        success: function (res) {
            loadData()
        }
    });


})




function loadData() {
    const currentCulture = $('#currentCulture').val();
    const URL = '/' + currentCulture + '/Cart/GetListCartItems';
    $.ajax({
        type: 'GET',
        url: URL,
        success: function (res) {
            $('#totalInCart').text(res.length);
            var html = "";
            var totalPrice = 0;
            var totalPriceDollar = 0;
            $.each(res, function (i, item) {
                var UrlItem = $('#urlItem').val() + item.productID + "/";
                var UrlImage = $('#urlImage').val() + item.imagePath;
                html +=
                    "<div class=\"row cart\">" +
                    "<div class=\"col-sm-3\">" +
                    "<img src=\"" + UrlImage + "\"  class=\"w-100 h-100 p-1\" />" +
                    "</div>" +
                    "<div class=\"col-sm-6\">" +
                    "<div class=\"title-product\">" +
                    "<a href=\" " + UrlItem + " \">" + item.productName + "</a>" +
                    "</div>" +
                    "<div class=\"ml\">$" + numberWithCommas(item.priceDollar) + "</div>" +
                    "<div class=\"price\">" + numberWithCommas(item.price) + " VND</div>" +
                    "<div class=\"quantity\">" +



                    "<div class=\"form-group\">" +


                    "<div class=\"input-group\">" +
                    "<div class=\"input-group-prepend\">" +
                    "<button style=\"min-width: 2.5rem\" data-id = \"" + item.productID + "\" class=\"btn btn-decrement btn-outline-secondary minusbutton123\" type=\"button\">" +
                    "<strong>-</strong>" +
                    "</button>" +
                    "</div>" +
                    "<input type=\"text\" style=\"text-align: center\" class=\"form-control\" placeholder value=\"" + item.quantity + "\">" +
                    "<div class=\"input-group-append\">" +
                    "<button style=\"min-width: 2.5rem\" data-id = \"" + item.productID + "\" class=\"btn btn-increment btn-outline-secondary plusbutton\" type=\"button\">" +
                    "<strong>+</strong>" +
                    "</button>" +
                    "</div>" +
                    "</div>" +


                    "</div>" +




                    "</div>" +
                    "</div>" +
                    "<div class=\"closed-sm p-0\">" +
                    "<button style=\"min-width: 2.5rem; border: none;\" data-id = \"" + item.productID + "\" class=\"btn btn-increment btn-outline-secondary deletebutton\" type=\"button\">" +
                    "<i class=\"fa fa-times fa-1x\" data-id = \"" + item.productID + "\" aria-hidden=\"true\"></i>" +
                    "</button>" +
                    "</div>" +
                    "</div>";


                totalPrice += item.price * item.quantity
                totalPriceDollar += item.priceDollar * item.quantity
            });
            $('#cart-body').html(html);
            $('#totalPrice').text(numberWithCommas(totalPrice) + " VND ($" + numberWithCommas(totalPriceDollar) + ")");
        },
        error: function () {
            $('#totalInCart').text(0);
        }

    });

}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}



