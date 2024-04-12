// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
window.onload = function() {
    if (!getCookie("cookiesAccepted")) {
        document.getElementById("cookieConsentPopup").style.display = "block";
    }
};

function acceptCookies() {
    setCookie("cookiesAccepted", "true", 365);
    document.getElementById("cookieConsentPopup").style.display = "none";
}

function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "")  + expires + "; path=/";
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for(var i=0;i < ca.length;i++) {
        var c = ca[i];
        while (c.charAt(0)==' ') c = c.substring(1,c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length);
    }
    return null;
}

function redirectToProductDetails(productId) {
    var url = '/Home/ProductDetails?productId=' + productId;
    window.location.href = url;
}

function selectAll(type) {
    var checkboxes = document.getElementsByName(type);
    var selectAllCheckbox = document.getElementById("selectAll" + type.charAt(0).toUpperCase() + type.slice(1));
    for (var i = 0; i < checkboxes.length; i++) {
        checkboxes[i].checked = selectAllCheckbox.checked;
    }
}

function clearFilters() {
    document.getElementById("selectAllCategories").checked = false;
    document.getElementById("selectAllColors").checked = false;
    var checkboxesCategories = document.getElementsByName("categories");
    var checkboxesColors = document.getElementsByName("colors");
    for (var i = 0; i < checkboxesCategories.length; i++) {
        checkboxesCategories[i].checked = false;
    }
    for (var i = 0; i < checkboxesColors.length; i++) {
        checkboxesColors[i].checked = false;
    }
    // Optionally, you can submit the form after clearing the filters
    // document.querySelector("form").submit();
}



// Write your JavaScript code.
