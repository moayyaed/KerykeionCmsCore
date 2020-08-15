$(document).ready(function () {

    configureContextMenu();
    openCreateRoleModal();
    closeModal();
});

function openCreateRoleModal() {
    $(document).on("click", ".create-role-modal-opener", function () {
        setupModal("Create role", "create-role-form", "Create", "", null, false);
    });
}
function setupModal(title, formId, formSubmitValue, roleName, roleId, isRequiredRole) {
    var modal = $("#role-modal");
    var form = modal.find("form");
    var roleNameInput = form.find("#role-name");
    var btnFormSubmit = form.find("input[type=submit]");
    var btnCancel = modal.find(".btn-cancel").last();

    $("#role-id-form-group").remove();

    modal.find("h4").text(title);
    form.attr("id", formId);
    form.find(".text-danger").html("");
    btnFormSubmit.val(formSubmitValue);
    btnCancel.html("Cancel");

    if (isRequiredRole) btnFormSubmit.addClass("d-none");
    else btnFormSubmit.removeClass("d-none");

    roleNameInput.val(roleName);
    roleNameInput.attr("readonly", isRequiredRole);

    if (roleId != null) {
        $(`<div id="role-id-form-group" class="form-group">
               <label>ID</label>
               <input id="role-id" class="form-control entity-value-to-copy-to-clipboard cursor-pointer" readonly value="${roleId}"/>
           </div>`).insertBefore(form.find(".form-group").first())
        btnCancel.html("Close");
    }

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