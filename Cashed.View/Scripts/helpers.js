function activateLink(linkId) {
    $(".navbar-nav li.nav-item").removeClass("active");
    $(linkId).addClass("active");
}

function makeInputAutocomplete(inputId, list, onSelect) {

    var input = document.getElementById(inputId);

    var comboplete = new Awesomplete(input, {
        minChars: 0,
        autoFirst: true,
        sort: false
    });
    if (list) {
        comboplete.list = list;
    }

    if (onSelect) {
        input.addEventListener("awesomplete-selectcomplete", onSelect);
    }

    var cmb = $("div.awesomplete");
    cmb.addClass("col-md-9");

    input.addEventListener("click", function () {
        if (comboplete.ul.childNodes.length === 0) {
            comboplete.minChars = 0;
            comboplete.evaluate();
        }
        else if (comboplete.ul.hasAttribute('hidden')) {
            comboplete.open();
        }
        else {
            comboplete.close();
        }
    });

    return comboplete;
}

function makeInputNumeric(inputId) {
    var input = document.getElementById(inputId);
    var $input = $("#" + inputId);
    input.onkeypress = function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (charCode == 32) return false;
        var last = String.fromCharCode(charCode);
        var value = ($input.val() + last).replace(",", ".");
        var int = +value;
        return !isNaN(int);
    };
}