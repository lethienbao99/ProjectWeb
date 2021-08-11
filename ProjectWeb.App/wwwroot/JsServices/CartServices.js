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
    $('#confirmCheckout').hide();
    loadData();
});





$('body').on('click', '.plusbutton', function (e) {

    const id = $(this).data('id');
    console.log(id);
    const currentCulture = $('#currentCulture').val();
    const URL = '/' + currentCulture + '/Cart/UpdateCart';
    $.ajax({
        type: 'POST',
        url: URL,
        data: {
            id: id,
            status: "plus"
        },
        async: false,
        success: function (res) {
            loadData()
        }
    });


})

$('body').on('click', '.minusbutton', function (e) {

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

$('body').on('click', '.deletebutton', function (e) {

    const id = $(this).data('id');
    console.log(id);
    const currentCulture = $('#currentCulture').val();
    const URL = '/' + currentCulture + '/Cart/UpdateCart';
    $.ajax({
        type: 'POST',
        url: URL,
        data: {
            id: id,
            status: "delete"
        },
        async: false,
        success: function (res) {
            loadData()
        }
    });


})

$("#btnCheckout").click(function () {
    $('#confirmCheckout').show();
});

$("#btnBackCart").click(function () {
    $('#confirmCheckout').hide();
});

$("#btnOrder").click(function () {
    debugger
    var userID = $('#UserID').val();
    var inputEmail = $('#inputShipEmail').val();
    var inputAddress = $('#inputShipAddress').val();
   
    if (userID == null || userID == "") {
        toastr.info("Vui lòng đăng nhập để đặt đơn hàng!", "Cảnh báo", 200)
        return false;
    }
    else if (inputEmail == "" || inputAddress == "") {
        toastr.info("Địa chỉ email hoặc địa chỉ đặt hàng không được để trống!", "Cảnh báo", 200)
        return false;
    }

    if (userID != null && userID != "" && inputEmail != "" && inputAddress != "") {
        const currentCulture = $('#currentCulture').val();
        const URL = '/' + currentCulture + '/Cart/CheckoutOrder';
        $.ajax({
            type: 'POST',
            url: URL,
            data: {
                email: inputEmail,
                shipAddress: inputAddress,
                userID: userID,

            },
            async: false,
            success: function (res) {
                debugger
                console.log("success");
                $('#confirmCheckout').hide();
                loadData();
                toastr.success("Đặt hàng thành công, vui lòng kiểm tra email!", "Thông báo", 200)
            }
        });

    }
   


});



function loadData() {
    debugger
    const currentCulture = $('#currentCulture').val();
    const URL = '/' + currentCulture + '/Cart/GetListCartItems';
    $.ajax({
        type: 'GET',
        url: URL,
        success: function (res) {
            $('#totalInCart').text(res.length);
            if (res.length == 0) {
                $('#CheckoutDiv').hide();
            }
            if (res.length > 0) {
                $('#CheckoutDiv').show();
            }
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
                    "<button style=\"min-width: 2.5rem\" data-id = \"" + item.productID + "\" class=\"btn btn-decrement btn-outline-secondary minusbutton\" type=\"button\">" +
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
                    "<i class=\"fa fa-times fa-1x deletebutton\" data-id = \"" + item.productID + "\" aria-hidden=\"true\"></i>" +
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



