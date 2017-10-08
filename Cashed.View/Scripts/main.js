$(document).ready(function () {
    $(".navbar-nav li.nav-item").click(function (event) {
        $(".navbar-nav li.nav-item").removeClass("active");
        $(event.currentTarget).addClass("active");
    });
});