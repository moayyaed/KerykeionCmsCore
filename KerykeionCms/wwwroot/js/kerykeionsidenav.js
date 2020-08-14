﻿$(document).ready(function () {

    configureContextMenu();
    openCreateRoleModal();
    closeModal();
});

function openCreateRoleModal() {
    $(document).on("click", ".create-role-modal-opener", function (event) {
        $("#create-role-modal").removeClass("d-none");
    });
}

function closeModal() {
    $(document).on("click", ".btn-cancel", function (event) {
        $(this).parents(".custom-modal").addClass('d-none');
    });
}

function configureContextMenu() {
    var isCustomContextMenu = false;

    $(document).on("contextmenu", ".context-menu-opener", function (event) {
        event.preventDefault();
        isCustomContextMenu = true;

        updateContextMenusClasses();

        $(this).parents(".side-navigation").first().addClass("bg-black-trans");
        $(this).siblings(".context-menu").removeClass("d-none");
        setTimeout(function () {
            isCustomContextMenu = false;
        }, 50);
    });

    $(document).on("contextmenu", function (evt) {
        evt.preventDefault();
        setTimeout(function () {
            if (!isCustomContextMenu) {
                updateContextMenusClasses();
            }
        }, 25);
    });

    $(document).on("click", function (event) {
        var target = event.target;
        updateContextMenusClasses();

        if ($(target).hasClass("inner-modal")) {
            return;
        }

        if ($(target).hasClass("custom-modal")) {
            $(target).addClass("d-none");
        }
    });
}

function updateContextMenusClasses() {
    $(".context-menu").each(function () {
        if (!$(this).hasClass("d-none")) {
            $(this).addClass("d-none");
            $(this).parents(".side-navigation").first().removeClass("bg-black-trans");
        }
    });
}