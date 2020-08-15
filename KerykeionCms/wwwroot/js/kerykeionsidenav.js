$(document).ready(function () {
    configureContextMenu();
    closeModal();
});

function setupModal(modalTitle, btnCloseModalValue, form) {
    var modal = $("#cru-modal");

    modal.find("h4").text(modalTitle);
    modal.find(".btn-cancel").last().html(btnCloseModalValue);
    modal.find("#modal-form-wrapper").html(form);

    modal.removeClass("d-none");
}


function closeModal() {
    $(document).on("click", ".btn-cancel", function (event) {
        $(this).parents(".custom-modal").addClass('d-none'); 7
        removeActiveSidNavClasses();
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
            removeActiveSidNavClasses();
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
function removeActiveSidNavClasses() {
    $(".side-navigation").each(function () {
        if ($(this).hasClass("bg-secondary")) {
            $(this).removeClass("text-dark bg-secondary");
        }
    });
}