$(document).ready(function () {
    var theForm = $("#theForm");
    theForm.hide();



    var button = $("#buyButton");
    button.on("click", function () {
        console.log("buing item");
    });

    var productInfo = $(".product-pros li");
    productInfo.on("click", function () {
        console.log("you clicked on" + $(this).text());
    });

    var $loginToggle = $("#loginToggle");
    var $popupForm = $(".popup-form");

    $loginToggle.on("click", function () {
        $popupForm.fadeToggle(100);
    })


});
