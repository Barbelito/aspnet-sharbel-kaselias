$(document).ready(function () {

    const $button = $("#mobile-menu-button");
    const $menu = $("#mobile-menu-flyout");

    $button.on("click", function () {

        $menu.toggleClass("open");

    });

    $(".mobile-menu-list a").on("click", function () {
        $menu.removeClass("open");
    });

});