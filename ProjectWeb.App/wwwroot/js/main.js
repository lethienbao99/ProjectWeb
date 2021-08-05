// header

$(window).scroll(function () {
  if ($(window).scrollTop() >= 100) {
    $("header .header").addClass("scrolltop");
    $("header .header ul li .nav-link .logo").addClass("width");
    $("header .header ul li .nav-link .logo").attr(
      "src",
      "data/logo_master_black_300x.png"
    );
    $("header .header nav ul li .nav-link").addClass("acolor");
    $("header .header .notification").hide();
  } else if (
    $(window).scrollTop() < 100 &&
    $("header .header").hasClass("scrolltop")
  ) {
    $("header .header .notification").show();
    if (!$("header .header").hasClass("no-header")) {
      $("header .header").removeClass("scrolltop");
      $("header .header ul li .nav-link .logo").removeClass("width");
      $("header .header ul li .nav-link .logo").attr(
        "src",
        "data/logo_master_white_150x.webp"
      );
      $("header .header nav ul li .nav-link").removeClass("acolor");
    }
  }
});
$("header .header nav") //Gọi tới thẻ header với class là 'header', trong class 'header' gọi tới thẻ nav
  .parents(".container-fluid") //Với class cha là 'container-fluid'
  .hover( //Event hover(rê chuột vào sẽ tạo hiệu ứng)
    function () {
      $("header .header").addClass("scrolltop"); //Thêm class vào thẻ header với class là 'header'
      $("header .header ul li .nav-link .logo").addClass("width");
      $("header .header ul li .nav-link .logo").attr( //Thêm thuộc tính
        "src",
        "data/logo_master_black_300x.png"
      );
      $("header .header nav ul li .nav-link").addClass("acolor");
    },
    function () {
      if (
        $(window).scrollTop() < 100 &&
        $("header .header").hasClass("scrolltop") //Kiểm tra xem có class scrolltop hay không
      ) {
        if (!$("header .header").hasClass("no-header")) {
          $("header .header").removeClass("scrolltop");
          $("header .header ul li .nav-link .logo").removeClass("width"); //Xóa class
          $("header .header ul li .nav-link .logo").attr(
            "src",
            "data/logo_master_white_150x.webp"
          );
          $("header .header nav ul li .nav-link").removeClass("acolor");
        }
      }
    }
  );
$(".menu-mobile ul li a.openHome").on("click", function (e) {
  e.preventDefault();
  $("header .menu-mobile-box").fadeIn("fast");
  $("header .menu-mobile-box ul.big-menu").addClass("open");
});
$("header .menu-mobile-box .bg-menu-mobile-box").on("click", function () {
  $("header .menu-mobile-box").fadeOut("fast");
  $("header .menu-mobile-box ul.big-menu").removeClass("open");
});
$("header .menu-mobile-box ul li.dropdown i").on("click", function () {
  $(this).parent("li.dropdown").find(">ul").toggle();
  $(this).toggleClass("fa-caret-right");
  $(this).toggleClass("fa-caret-down");
});

$("header .header nav .menu-right ul li a.search-pc").on("click", function () {
  $(this).parent(".nav-item").find(".form-modal").fadeIn("fast");
  $(this).parent(".nav-item").find(".form-modal .box-search").addClass("show");
  $("body").addClass("position-fixed");
});
$("header .header nav .menu-right ul li .form-modal .closed").on(
  "click",
  function () {
    $(this)
      .parents(".nav-item")
      .find(".form-modal .box-search")
      .removeClass("show");
    $(this).parents(".nav-item").find(".form-modal").fadeOut("fast");
    $("body").removeClass("position-fixed");
  }
);
$("header .header nav .menu-right ul li .form-modal .bg-search").on(
  "click",
  function () {
    $(this)
      .parents(".nav-item")
      .find(".form-modal .box-search")
      .removeClass("show");
    $(this).parents(".nav-item").find(".form-modal").fadeOut("fast");
    $("body").removeClass("position-fixed");
  }
);

$(
  "header .header nav .menu-right ul li a.cart-pc,header .header nav .menu-mobile ul li a.cart-mobile"
).on("click", function (e) {
  e.preventDefault();
  $(
    "header .header nav .menu-right ul li a.cart-pc,header .header nav .menu-mobile ul li a.cart-mobile"
  )
    .parent(".nav-item")
    .find(".form-modal")
    .fadeIn("fast");
  $(
    "header .header nav .menu-right ul li a.cart-pc,header .header nav .menu-mobile ul li a.cart-mobile"
  )
    .parent(".nav-item")
    .find(".form-modal .box-cart")
    .addClass("show");
  $("body").addClass("position-fixed");
});
$(
  "header .header nav .menu-right ul li .form-modal .closed,header .header nav .menu-mobile ul li .form-modal .closed"
).on("click", function () {
  $(
    "header .header nav .menu-right ul li a.cart-pc,header .header nav .menu-mobile ul li a.cart-mobile"
  )
    .parents(".nav-item")
    .find(".form-modal .box-cart")
    .removeClass("show");
  $(
    "header .header nav .menu-right ul li a.cart-pc,header .header nav .menu-mobile ul li a.cart-mobile"
  )
    .parents(".nav-item")
    .find(".form-modal")
    .fadeOut("fast");
  $("body").removeClass("position-fixed");
});
$(
  "header .header nav .menu-right ul li .form-modal .bg-modal,header .header nav .menu-mobile ul li .form-modal .bg-modal"
).on("click", function () {
  $(
    "header .header nav .menu-right ul li a.cart-pc,header .header nav .menu-mobile ul li a.cart-mobile"
  )
    .parents(".nav-item")
    .find(".form-modal .box-cart")
    .removeClass("show");
  $(
    "header .header nav .menu-right ul li a.cart-pc,header .header nav .menu-mobile ul li a.cart-mobile"
  )
    .parents(".nav-item")
    .find(".form-modal")
    .fadeOut("fast");
  $("body").removeClass("position-fixed");
});

$("section.info-product .info-box .addcart button.addtocart").on(
  "click",
  function (e) {
    e.preventDefault();
    $(
      "header .header nav .menu-right ul li a.cart-pc,header .header nav .menu-mobile ul li a.cart-mobile"
    )
      .parent(".nav-item")
      .find(".form-modal")
      .fadeIn("fast");
    $(
      "header .header nav .menu-right ul li a.cart-pc,header .header nav .menu-mobile ul li a.cart-mobile"
    )
      .parent(".nav-item")
      .find(".form-modal .box-cart")
      .addClass("show");
    $("body").addClass("position-fixed");
  }
);
// end header
// wow

new WOW().init();
// end wow

// what is new
$("section.whatisnew .owl-carousel").owlCarousel({
  loop: true,
  margin: 10,
  dots: true,
  nav: false,
  responsive: {
    0: {
      items: 1,
    },
    768: {
      items: 3,
    },
  },
});
// end what is new
// instagram
$("section.instagram .owl-carousel").owlCarousel({
  loop: false,
  margin: 10,
  dots: true,
  nav: false,
  responsive: {
    0: {
      items: 1,
    },
    480: {
      items: 3,
    },
  },
});

$("section.instagram .instagram-box .box-insta").hover(
  function () {
    $("section.instagram .instagram-box .box-insta .bg-box").fadeTo(200, 0.5);
    $(this).find(".bg-box").hide();
    $(this).find(".camera").show();
  },
  function () {
    $("section.instagram .instagram-box .box-insta .bg-box").hide();
    $("section.instagram .instagram-box .box-insta .camera").hide();
  }
);
// end instagram

// product
//review
$("main section.product-reviews .title .write a").on("click", function (e) {
  e.preventDefault();
  $(this).parents("section.product-reviews").find(".review").toggle();
});
//or for example
options = {
  symbols: {
    fontawesome_star: {
      base: '<i class="far fa-star fa-lg"></i>',
      hover: '<i class="fa fa-star fa-lg"></i>',
      selected: '<i class="fa fa-star fa-lg"></i>',
    },
  },
  selected_symbol_type: "fontawesome_star",
  max_value: 5,
};
$(".rating").rate(options);
// end review
// quantity
$(".quantity input[type='number']").inputSpinner();
// end quantity
// images-box product slide
$("section.info-product .images-box-mobile .owl-carousel").owlCarousel({
  loop: true,
  margin: 10,
  dots: true,
  nav: false,
  items: 1,
});

$("section.info-product .images-box-pc .images-sm img").on(
  "click",
  function () {
    let imgSrc = $(this).attr("src");
    $(this)
      .parents(".images-box-pc")
      .find(".images-big img")
      .attr("src", imgSrc);
  }
);

// end images-box product slide

// product detail
$("section.products.detail .product .owl-carousel").owlCarousel({
  loop: false,
  margin: 10,
  dots: false,
  nav: true,
  navText: [
    '<i class="fa fa-chevron-left"></i>',
    '<i class="fa fa-chevron-right"></i>',
  ],
  responsive: {
    0: {
      items: 2,
    },
    600: {
      items: 4,
    },
  },
});
// End product detail
// end product
