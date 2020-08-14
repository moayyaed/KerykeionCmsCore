"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/kerykeioncmshub").build();

connection.start().then(function () {
    console.log("SignalR Connected!");
}).catch(function (err) {
    return console.error(err.toString());
});

var counter = 0;
var areSideNavRolesLoaded = false;
var areSideNavImagesLoaded = false;



//#region Images
connection.on("GetSideNavImages", function (images) {
    areSideNavImagesLoaded = true;
    var html = '';

    for (var i = 0; i < images.length; i++) {
        html += `<div class="side-navigation">
                    <span class="nav-link text-inherit context-menu-opener">
                        <span class="ml-5">
                            <i class="fa fa-picture-o mr-1" aria-hidden="true"></i>
                            ${images[i].name}
                        </span>
                    </span>
                    <div class="context-menu position-absolute d-none bg-dark text-white">
                        <ul class="nav nav-pills flex-column">
                            <li class="nav-item m-1">
                                <div class="text-white row m-1">
                                    <span class="col text-left">Open</span>
                                    <span class="col text-right">
                                        <i class="fa fa-arrow-circle-right text-right text-info" aria-hidden="true"></i>
                                    </span>
                                </div>
                            </li>
                            <li class="nav-item m-1">
                                <div class="delete-entity-from-side-nav text-white row m-1">
                                    <span class="col text-left">Delete</span>
                                    <span class="col text-right">
                                        <i class="fa fa-trash-o text-right text-danger" aria-hidden="true"></i>
                                    </span>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>`;
    }

    $("#subnav-images").html(html);
});
$(document).on("click", "#open-subnav-images", function (event) {
    if (!areSideNavImagesLoaded) {
        connection.invoke("SendSideNavImagesAsync").catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    }
});

connection.on("GetMainImages", function (images) {
    var html = `<div class="row border-bottom border-secondary">
                    <div class="col-sm-6">
                        <h2>Images</h2>
                    </div>
                </div>
                <div class="mt-1 text-right">
                    <button class="btn btn-success">
                        Create
                        <i class="fa fa-plus-square" aria-hidden="true"></i>
                    </button>
                </div>
                <div class="mt-5 row pb-1 border-bottom border-secondary">
                <div class="col-sm-3">

                </div>
                <div class="col-sm-3">
                    <h5>
                        <span class="text-inherit" id="sort-images-by-name">
                            Title
                            <i class="fa fa-sort-alpha-asc" aria-hidden="true"></i>
                            <input type="hidden" value="Ascending" />
                        </span>
                    </h5>
                </div>
                <div class="col-sm-3">
                    <h5>
                        <span class="text-inherit" id="sort-images-by-datetime">
                            Added on
                            <i class="fa fa-sort" aria-hidden="true"></i>
                            <input type="hidden" value="None" />
                        </span>
                    </h5>
                </div>
                <div class="col">

                </div>
                </div>`;

    for (var i = 0; i < images.length; i++) {
        html += `<div class="row pt-2 pb-2 border border-secondary mb-1">
                <div class="col-sm-3">
                    <img src="${images[i].url}" class="img-fluid" />
                </div>
                <div class="col-sm-3">
                    <p>
                        ${images[i].name}
                    </p>
                </div>
                <div class="col-sm-3">
                    <p>
                        ${images[i].dateTimeCreated}
                    </p>
                </div>
                <div class="col text-right d-block">
                    <button class="btn btn-info d-inline-block">
                        Details
                        <i class="fa fa-pencil-square-o" aria-hidden="true"></i>
                    </button>
                    <button type="submit" class="btn btn-danger">
                        Delete
                        <i class="fa fa-trash-o" aria-hidden="true"></i>
                    </button>
                </div>
            </div>`;
    }

    $("#main").find("main").html(html);
});

$(document).on("click", ".main-images-opener", function (event) {
    if (!$(this).parents(".side-navigation").first().hasClass("bg-secondary")) {
        removeActiveSidNavClasses();
        connection.invoke("SendMainImagesAsync").catch(function (err) {
            return console.error(err.toString());
        });
        $(this).parents(".side-navigation").first().addClass("text-dark bg-secondary")
        event.preventDefault();
    }
});
//#endregion


//#region Entities

//#endregion


//#region Roles
connection.on("GetSideNavRoles", function (roles) {
    areSideNavRolesLoaded = true;
    fillSideNavRoles(roles);
});
$(document).on("click", "#open-subnav-roles", function (event) {
    if (!areSideNavRolesLoaded) {
        connection.invoke("SendSideNavRolesAsync").catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    }
});

connection.on("GetMainRoles", function (roles) {
    displayMainRoles(roles);
});
$(document).on("click", ".main-roles-opener", function (event) {
    if (!$(this).parents(".side-navigation").first().hasClass("bg-secondary")) {
        removeActiveSidNavClasses();
        connection.invoke("SendMainRolesAsync").catch(function (err) {
            return console.error(err.toString());
        });
        $(this).parents(".side-navigation").first().addClass("text-dark bg-secondary")
        event.preventDefault();
    }
});

connection.on("ReceiveCreateRoleResult", function (result, roles, role) {
    if (!result.succeeded) {
        var html = '<ul>';
        for (var i = 0; i < result.errors.length; i++) {
            html += `<li>${result.errors[i].description}</li>`;
        }
        html += '</ul>';
        $("#create-role-form").find(".errors-wrapper").html(html);
    }
    else {
        $("#create-role-modal").addClass("d-none");
        fillSideNavRoles(roles);
        setupRoleDetails(role, false);
    }
});
$(document).on("submit", "#create-role-form", function (event) {
    if ($(this).valid()) {
        event.preventDefault();
        connection.invoke("CreateRoleAsync", $("#role-name").val()).catch(function (err) {
            return console.error(err.toString());
        });
    }
});

connection.on("ReceiveUpdateRoleResult", function (result, roles, role) {
    console.log(result);
    if (!result.succeeded) {
        var html = '<ul>';
        for (var i = 0; i < result.errors.length; i++) {
            html += `<li>${result.errors[i].description}</li>`;
        }
        html += '</ul>';
        $("#update-role-form").find(".errors-wrapper").html(html);
    }
    else {
        fillSideNavRoles(roles);
        setupRoleDetails(role, true);
    }
});
$(document).on("submit", "#update-role-form", function (event) {
    event.preventDefault();
    $(this).validate({
        errorClass: "text-danger",
        rules: {
            updatedname: {
                required: true,
                minlength: 5,
                maxlength: 50
            }
        },
        messages: {
            updatedname: {
                required: "A name is required.",
                minlength: jQuery.validator.format("At least {0} characters required."),
                maxlength: jQuery.validator.format("At max {0} characters allowed."),
            }
        }
    });

    if ($(this).valid()) {
        connection.invoke("UpdateRoleAsync", $("#updated-role-name").val(), $(this).data("id")).catch(function (err) {
            return console.error(err.toString());
        });
    }
});

connection.on("ReceiveRole", function (role) {
    setupRoleDetails(role);
});
$(document).on("click", ".role-opener", function (event) {
    if (!$(this).parents(".side-navigation").first().hasClass("bg-secondary") && !$(this).hasClass("bg-secondary")) {
        connection.invoke("GetRoleAsync", $(this).data("id")).catch(function (err) {
            return console.error(err.toString());
        });
    }
    event.preventDefault();
});

connection.on("ReceiveRoleDeleted", function (result, id) {
    if (result.succeeded) {
        $(`.${id}`).remove();
        $("#main").find("main").prepend(`<div class="alert alert-success mt-2 mb-2" role="alert">
            The role has been deleted successfully.
        </div>`);
    }
    else {
        alert("The deletion failed.");
    }
});
$(document).on("click", ".role-deleter", function (event) {
    var isSure = confirm(`'${$(this).data("name")}' will be permanently deleted.`);

    if (isSure) {
        connection.invoke("DeleteRoleAsync", $(this).data("id")).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    }
});


function setupRoleDetails(role, isUpdated) {
    if (role == null) {
        return;
    }

    removeActiveSidNavClasses();
    $("#open-subnav-roles").trigger("click");

    $("#main").find("main").html(`<div class="border-bottom border-secondary pb-1 mb-3 ${role.id}">
                                        <h1>Role - ${role.name}</h1>
                                  </div>
                                  ${isUpdated ? `<div class="alert alert-success alert-dismissible mt-2 mb-2" role="alert">
        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        The role has been updated successfully.
        </div>` : ``}
                                  <div class="col-sm-6">
                                  <form method="post" class="p-3" id="update-role-form" data-id="${role.id}">
                                      <div class="text-danger errors-wrapper"></div>
                                      <div class="form-group">
                                          <label>ID</label>
                                          <input class="form-control entity-value-to-copy-to-clipboard cursor-pointer" readonly value="${role.id}"/>
                                      </div>
                                      <div class="form-group">
                                          <label for="updatedname">Name</label>
                                          <input name="updatedname" id="updated-role-name" class="form-control" value="${role.name}" ${returnReadonlyIfDefaultRole(role.name)} />
                                      </div>
                                      <div class="form-group">
                                          ${returnUpdateBtnIfNotDefaultRole(role)}
                                      </div>
                                      ${document.getElementById("verif-token-holder").innerHTML}
                                  </form>
                                    <div class="row">
                                        <div class="text-right">
                                        ${returnDeleteBtnIfNotDefaultRole(role)}
                                        </div>
                                    </div>
                              </div>`);

    setTimeout(function () {
        $("#subnav-roles").find(`.${role.id}`).addClass("text-dark bg-secondary");
    }, 100);
}
function fillSideNavRoles(roles) {
    var html = '';

    for (var i = 0; i < roles.length; i++) {
        html += `<div data-id="${roles[i].id}" class="side-navigation ${roles[i].id} role-opener">
                    <span class="nav-link text-inherit context-menu-opener">
                    <span class="ml-5">
                        <i class="fa fa-id-badge mr-1" aria-hidden="true"></i>
                        ${roles[i].name}
                    </span>
                    </span>
                    <div class="context-menu position-absolute d-none bg-dark text-white">
                        <ul class="nav nav-pills flex-column">
                            <li data-id="${roles[i].id}" class="nav-item m-1 role-opener">
                                <div class="text-white row m-1">
                                    <span class="col text-left">Open</span>
                                    <span class="col text-right">
                                        <i class="fa fa-arrow-circle-right text-right text-info" aria-hidden="true"></i>
                                    </span>
                                </div>
                            </li>
                            ${returnDeleteFunctionalityInSideNavIfNotDefaultRole(roles[i])}
                        </ul>
                    </div>
                </div>`;
    }

    $("#subnav-roles").html(html);
}
function displayMainRoles(roles) {
    var html = `<div class="row border-bottom border-secondary">
                    <div class="col-sm-6">
                        <h2>Roles</h2>
                    </div>
                </div>
                <div class="mt-1 text-right">
                    <button class="btn btn-success create-role-modal-opener">
                        Create
                        <i class="fa fa-plus-square" aria-hidden="true"></i>
                    </button>
                </div>
                <div class="mt-3 row pb-1 border-bottom border-secondary">
                    <div class="col-sm-4">
                        <h5>
                            <a class="text-inherit" asp-page-handler="SortName" id="sort-roles-by-name">
                                Name
                                <i class="fa fa-sort-alpha-asc" aria-hidden="true"></i>
                                <input type="hidden" value="Ascending" />
                            </a>
                        </h5>
                    </div>
                <div class="col-sm-4">
                </div>
                <div class="col">

                </div>
                </div>`;

    for (var i = 0; i < roles.length; i++) {
        html += `<div class="row pt-2 pb-2 border border-secondary mb-1 ${roles[i].id}">
                    <div class="col-sm-4">
                        <p>
                            ${roles[i].name}
                        </p>
                    </div>
                    <div class="col-sm-4">
                    </div>
                    <div class="col text-right d-block">
                        <button data-id="${roles[i].id}" class="btn btn-info d-inline-block role-opener">
                            Details
                            <i class="fa fa-pencil-square-o" aria-hidden="true"></i>
                        </button>
                        ${returnDeleteBtnIfNotDefaultRole(roles[i])}
                    </div>
                </div>`;
    }

    $("#main").find("main").html(html);
}
function returnDeleteBtnIfNotDefaultRole(role) {
    if (role.name === "Administrator" || role.name === "Editor" || role.name === "RegularUser") {
        return ``;
    }

    return `<button data-id="${role.id}" data-name="${role.name}" class="btn btn-danger role-deleter">
                Delete
                <i class="fa fa-trash-o" aria-hidden="true"></i>
            </button>`;
}
function returnDeleteFunctionalityInSideNavIfNotDefaultRole(role) {
    if (role.name === "Administrator" || role.name === "Editor" || role.name === "RegularUser") {
        return ``;
    }

    return `<li class="nav-item m-1">
                                <div data-id="${role.id}" data-name="${role.name}" class="delete-entity-from-side-nav text-white row m-1 role-deleter">
                                    <span class="col text-left">Delete</span>
                                    <span class="col text-right">
                                        <i class="fa fa-trash-o text-right text-danger" aria-hidden="true"></i>
                                    </span>
                                </div>
                            </li>`;
}
function returnUpdateBtnIfNotDefaultRole(role) {
    if (role.name === "Administrator" || role.name === "Editor" || role.name === "RegularUser") {
        return ``;
    }

    return `<input class="btn btn-primary" type="submit" value="Update"/>`;
}
function returnReadonlyIfDefaultRole(roleName) {
    if (roleName === "Administrator" || roleName === "Editor" || roleName === "RegularUser") {
        return `readonly`;
    }

    return '';
}
//#endregion

function removeActiveSidNavClasses() {
    $(".side-navigation").each(function () {
        if ($(this).hasClass("bg-secondary")) {
            $(this).removeClass("text-dark bg-secondary");
        }
    });
}