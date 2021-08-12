$("#inputValueSearch").keyup(function (event) {
        $("#btnSearchItems").click();
});

$("#btnSearchNavbar").click(function (event) {
        $("#btnSearchItems").click();
});



$("#btnSearchItems").click(function () {
    const currentCulture = $('#currentCulture').val();
    const URL = '/' + currentCulture + '/Product/GetListProductForSearch';
    $.ajax({
        type: 'GET',
        url: URL,
        data: {
            keyword: $('#inputValueSearch').val()
        },
        success: function (res) {
            debugger;
            console.log(res);
            var html = "";
            $.each(res, function (i, item) {
                debugger
                var UrlItem = $('#urlItem').val() + item.id + "/";
                var UrlImage = $('#urlImage').val() + item.imgDefaultPath;
                html += `<div class="row">
                            <div class="col-3 pl-0 mb-3">
                                <img src="${UrlImage}"
                                     class="w-100"
                                     alt="" />
                            </div>
                            <div class="col-9">
                                <div class="title-product">
                                    <h3>
                                        <a href="${UrlItem}">${item.productName}</a>
                                    </h3>
                                </div>
                                <div class="from-money"> ${numberWithCommas(item.price)} VND ($${numberWithCommas(item.priceDollar)})</div>

                                <div class="content">
                                    Made with 100% certified organic and
                                    naturally grown produce picked in
                                    Australia, plus other care...
                                </div>
                            </div>
                        </div>`

            });

            $('#searchProduct-body').html(html);


        },
        error: function () {

        }
    });
});