var ProductServices = function () {
    this.initialize = loadDataReviewProduct();
}

const UserID = $('#UserID').val();
const ProductID = $('#productID').val();

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


$("#btnSubmitReview").click(function (event) {
    debugger
    event.preventDefault();

    if ($('#txtTitleReview').val() == "" || $('#txtDetailReview').val() == "" )  {
        toastr.info("Tiêu đề hoặc nội dung đánh giá không được để trống!", "Cảnh báo", 200);
        return false;
    }

    const currentCulture = $('#currentCulture').val();
    const URL = '/' + currentCulture + '/Message/PostMessage';
    if (UserID != null && ProductID != null && UserID != "" && ProductID != "") {
        $.ajax({
            type: 'POST',
            url: URL,
            data: {
                TitleText: $('#txtTitleReview').val(),
                MessageText: $('#txtDetailReview').val(),
                UserID: UserID,
                ProductID: ProductID
            },
            async: false,
            success: function (res) {
                //$('#divReview').hide();
                $("main section.product-reviews .title .write a").parents("section.product-reviews").find(".review").toggle();
                loadDataReviewProduct();
                toastr.success("Binh luận thành công!", "Thông báo", 200)
                return false;
            }
        });
    }
    else {
        toastr.info("Vui lòng đăng nhập!", "Cảnh báo", 200);
        return false;
    }
    

   
});

function loadDataReviewProduct() {
    debugger
 
    const URL = `/${currentCulture}/Message/GetListReviewProduct?ProductID=${ProductID}`;
    $.ajax({
        type: 'GET',
        url: URL,
        success: function (res) {
            debugger
            var html = "";
            $.each(res, function (i, item) {
                debugger
                html +=
                        `
                            <div class="col-12 bdr">
                                <div class="star">
                                    <i class="fa fa-star fa-lg"></i>
                                    <i class="fa fa-star fa-lg"></i>
                                    <i class="fa fa-star fa-lg"></i>
                                    <i class="fa fa-star fa-lg"></i>
                                    <i class="fa fa-star fa-lg"></i>
                                </div>
                                <div class="username mt-2">
                                    <strong>
                                        ${item.titleText}
                                    </strong>
                                </div>
                                <div class="date">
                                    <strong> ${item.username}</strong> on <strong>Aug 31, 2018</strong>
                                </div>
                                <div class="content">
                                    ${item.messageText}
                                </div>
                                <div class="report text-right">
                                   
                                </div>
                            </div>
                        `

            });
            $('#DivParentReview').html(html);

        },
        error: function () {
        }

    });

}
