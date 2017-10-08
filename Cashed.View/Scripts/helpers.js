function activateLink(linkId) {
    $(".navbar-nav li.nav-item").removeClass("active");
    $(linkId).addClass("active");
}