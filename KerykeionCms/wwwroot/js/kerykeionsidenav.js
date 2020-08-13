$(document).ready(function () {
    var isCustomContextMenu = false;

    $(".context-menu-opener").on("contextmenu", function (event) {
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

    $(document).on("click", function () {
        updateContextMenusClasses();
    });

    $(document).on("click", ".delete-entity-from-side-nav", function () {
        var isSure = confirm(`'${$(this).data("name")}' will be permanently deleted.`);
        if (isSure) {
            showSpinner();

            var element = $(this);
            var dto = { Id: $(this).data("id"), Table: $(this).data("table") };

            postAjax(JSON.stringify(dto), `/KerykeionCms${$(this).data("page")}?handler=DeleteViaAjax`, "application/json;charset=utf-8", function (result) {
                if (result.successfull) {
                    hideSpinner();
                    element.parents(".side-navigation").first().remove();
                }
                else {
                    hideSpinner();

                    var error = "";

                    for (var i = 0; i < result.errors.length; i++) {
                        error += `\n${result.errors[i].message}`;
                    }

                    alert(error);
                }
            });
        }
    });
});

function updateContextMenusClasses() {
    $(".context-menu").each(function () {
        if (!$(this).hasClass("d-none")) {
            $(this).addClass("d-none");
            $(this).parents(".side-navigation").first().removeClass("bg-black-trans");
        }
    });
}