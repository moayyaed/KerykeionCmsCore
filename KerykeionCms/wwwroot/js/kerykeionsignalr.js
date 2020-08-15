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
var isMainRolesPage = false;
var isMainImagesPage = false;



//#region Images
connection.on("ReceiveSideNavImages", function (images) {
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

connection.on("ReceiveMainImages", function (images) {
    displayMainEntities(images, "Images", "CreateOpenerNotYetDefined", "image-opener", "image-deleter");
    setMainPagesBooleans(false, true);
    setSideNavsMainPageActivity();
});
$(document).on("click", ".main-images-opener", function (event) {
    if (!isMainImagesPage) {
        removeActiveSidNavClasses();
        connection.invoke("SendMainImagesAsync").catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    }
});
//#endregion


//#region Entities

//#endregion


//#region Roles
connection.on("ReceiveSideNavRoles", function (roles) {
    areSideNavRolesLoaded = true;
    displaySideNavRoles(roles);
});
$(document).on("click", "#open-subnav-roles", function (event) {
    if (!areSideNavRolesLoaded) {
        connection.invoke("SendSideNavRolesAsync").catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    }
});

connection.on("ReceiveMainRoles", function (roles) {
    displayMainEntities(roles, "Roles", "create-role-modal-opener", "role-opener", "role-deleter");
    setRolesActive();
});
$(document).on("click", ".main-roles-opener", function (event) {
    if (!isMainRolesPage) {
        removeActiveSidNavClasses();
        connection.invoke("SendMainRolesAsync").catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    }
});

connection.on("ReceiveCreateRoleResult", function (result, roles, role) {
    if (!result.succeeded) {
        displayFormErrors("create-role-form", result.errors);
    }
    else {
        displaySideNavRoles(roles);
        if (isMainRolesPage) displayMainEntities(roles, "Roles", "create-role-modal-opener");
        getNewUpdateRoleForm(role);
    }
});
$(document).on("submit", "#create-role-form", function (event) {
    event.preventDefault();
    if ($(this).valid()) {
        connection.invoke("CreateRoleAsync", $("#role-name").val()).catch(function (err) {
            return console.error(err.toString());
        });
    }
});

connection.on("ReceiveUpdateRoleForm", function (form, role) {
    setupModal(`Role: ${role.name}`, "Close", form);
    reloadValidationScripts();
    removeActiveSidNavClasses();
    setSideNavsMainPageActivity();
    $("#open-subnav-roles").trigger("click");

    setTimeout(function () {
        $("#subnav-roles").find(`.${role.id}`).addClass("text-dark bg-secondary");
        $("#cru-modal").find("form").find("#role-id").val(role.id);
        $("#cru-modal").find("form").find("#role-name").val(role.name);
    }, 50);
});
connection.on("ReceiveCreateRoleForm", function (form) {
    setupModal("Create Role", "Cancel", form);
    reloadValidationScripts();
});
$(document).on("click", ".create-role-modal-opener", function (event) {
    connection.invoke("SendCreateRoleFormAsync").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

connection.on("ReceiveUpdateRoleResult", function (result, roles, roleName) {
    if (!result.succeeded) {
        displayFormErrors("update-role-form", result.errors);
    }
    else {
        displaySideNavRoles(roles);
        if (isMainRolesPage) displayMainEntities(roles, "Roles", "create-role-modal-opener");
        reshapeModalAfterUpdate(`Role: ${roleName}`);
        displaySuccessMessageAfterUpdate(`Role "${roleName}" has been UPDATED successfully.`);
    }
});
$(document).on("submit", "#update-role-form", function (event) {
    event.preventDefault();
    if ($(this).find("#role-id").val().toLowerCase() === "A2EB5341-22E7-43C7-AC0E-C4AFED51DB2B".toLowerCase() || $(this).find("#role-id").val().toLowerCase() === "57F5DC72-FA6D-4038-B337-D00BEF5A759A".toLowerCase() || $(this).find("#role-id").val().toLowerCase() === "2DD7B94B-CE9A-473A-B955-2FAD487BD435".toLowerCase()) {
        alert("This role can not be updated.");
        return;
    }

    if ($(this).valid()) {
        connection.invoke("UpdateRoleAsync", $("#role-name").val(), $(this).find("#role-id").val()).catch(function (err) {
            return console.error(err.toString());
        });
    }
});

connection.on("ReceiveRole", function (role) {
    $(".alert").remove();
    getNewUpdateRoleForm(role);
});
$(document).on("click", ".role-opener", function (event) {
    if (!$(this).parents(".side-navigation").first().hasClass("bg-secondary") && !$(this).hasClass("bg-secondary")) {
        connection.invoke("GetRoleAsync", $(this).data("id")).catch(function (err) {
            return console.error(err.toString());
        });
    }
    event.preventDefault();
});

connection.on("ReceiveDeleteRoleResult", function (result, role) {
    if (result.succeeded) {
        $(`.${role.id}`).remove();
        displaySuccessMessageAfterDeletion(`Role "${role.name}" has been DELETED successfully.`);
    }
    else {
        displayDeleteErrors(result.errors);
    }
});
$(document).on("click", ".role-deleter", function (event) {
    if ($(this).data("id").toLowerCase() === "A2EB5341-22E7-43C7-AC0E-C4AFED51DB2B".toLowerCase() || $(this).data("id").toLowerCase() === "57F5DC72-FA6D-4038-B337-D00BEF5A759A".toLowerCase() || $(this).data("id").toLowerCase() === "2DD7B94B-CE9A-473A-B955-2FAD487BD435".toLowerCase()) {
        alert("This role can not be deleted.");
        return;
    }
    var isSure = confirm(`'${$(this).data("name")}' will be permanently deleted.`);

    if (isSure) {
        connection.invoke("DeleteRoleAsync", $(this).data("id")).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    }
});

function setRolesActive() {
    setMainPagesBooleans(true, false);
    setSideNavsMainPageActivity();
}
function getNewUpdateRoleForm(role) {
    if (role == null) {
        return;
    }

    connection.invoke("SendUpdateRoleFormAsync", role.id).catch(function (err) {
        return console.error(err.toString());
    });
}

// twee functies hieronder herbekijken maandag, zie displaySidNavEntities()
function displaySideNavRoles(roles) {
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
//#endregion


function displayFormErrors(formId, errors) {
    var html = '<ul>';
    for (var i = 0; i < errors.length; i++) {
        html += `<li>${errors[i].description}</li>`;
    }
    html += '</ul>';
    $(`#${formId}`).find(".errors-wrapper").html(html);
}
function setMainPagesBooleans(isRolesPage, isImagesPage) {
    isMainRolesPage = isRolesPage;
    isMainImagesPage = isImagesPage;
}
function setSideNavsMainPageActivity() {
    if (isMainRolesPage) {
        $("#side-nav-roles-wrapper").addClass("bg-grey-trans").children().first().removeClass("side-navigation");
        $("#side-nav-images-wrapper").removeClass("bg-grey-trans").children().first().addClass("side-navigation");
    }
    if (isMainImagesPage) {
        $("#side-nav-roles-wrapper").removeClass("bg-grey-trans").children().first().addClass("side-navigation");
        $("#side-nav-images-wrapper").addClass("bg-grey-trans").children().first().removeClass("side-navigation");
    }
}


function displaySideNavEntities() {
    // werksje voe maandag
}

function displayMainEntities(entities, pageTitle, createOpener, detailOpener, deleter) {
    var html = `<div class="row border-bottom border-secondary">
                    <div class="col-sm-6">
                        <h2>${pageTitle}</h2>
                    </div>
                </div>
                <div class="mt-1 text-right">
                    <button class="btn btn-success ${createOpener}">
                        Create
                        <i class="fa fa-plus-square" aria-hidden="true"></i>
                    </button>
                </div>
                <div class="mt-3 mb-2 row pb-1 border-bottom border-secondary">
                    ${getEntityKeyCollumnsHtml(entities[0])}
                </div>`;

    html += getHtmlContainingEntities(entities, detailOpener, deleter);
    $("#main").find("main").html(html);
}
function getHtmlContainingEntities(entities, detailOpener, deleter) {
    var html = "";

    for (var i = 0; i < entities.length; i++) {
        html += `<div class="row pt-2 pb-2 border border-secondary mb-1 ${entities[i].id}">
            ${getEntityValueCollumnsHtml(entities[i])}
            <div class="col text-right d-block">
                <button data-id="${entities[i].id}" class="btn btn-info d-inline-block ${detailOpener}">
                    Details
                    <i class="fa fa-pencil-square-o" aria-hidden="true"></i>
                </button>
                <button data-id="${entities[i].id}" data-name="${entities[i].name}" class="btn btn-danger d-inline-block ${deleter}">
                    Delete
                    <i class="fa fa-trash-o" aria-hidden="true"></i>
                </button>
            </div>
        </div>`
    }

    return html;
}
function getEntityKeyCollumnsHtml(entity) {
    var columnClass = `col-sm-${Math.floor(12 / Object.keys(entity).length)}`;
    var html = "";
    for (var i = 0; i < Object.keys(entity).length; i++) {
        if (Object.keys(entity)[i] === "id") {
            continue;
        }
        if (Object.keys(entity)[i] === "imageUrl") {
            html += `<div class="${columnClass}">
            </div>`
            continue;
        }
        if (Object.keys(entity)[i] === "name" || Object.keys(entity)[i] === "title") {
            html += `<div class="${columnClass}"><h5>
                        <span class="text-inherit cursor-pointer" id="sort-entities-by-name">
                            ${Object.keys(entity)[i].toUpperCase()}
                            <i class="fa fa-sort-alpha-asc" aria-hidden="true"></i>
                            <input type="hidden" value="Ascending" />
                        </span>
                    </h5></div>`;
            continue;
        }
        if (Object.keys(entity)[i] === "created") {
            html += `<div class="${columnClass}"><h5>
                        <span class="text-inherit cursor-pointer" id="sort-entities-by-datetime">
                            ${Object.keys(entity)[i].toUpperCase()}
                            <i class="fa fa-sort" aria-hidden="true"></i>
                            <input type="hidden" value="Ascending" />
                        </span>
                    </h5></div>`;
            continue;
        }
        html += `<div class="${columnClass}">
                <h5>
                    ${Object.keys(entity)[i].toUpperCase()}
                </h5>
            </div>`
    }
    return html;
}
function getEntityValueCollumnsHtml(entity) {
    var columnClass = `col-sm-${Math.floor(12 / Object.keys(entity).length)}`;
    var html = "";
    for (const [key, value] of Object.entries(entity)) {
        if (key === "id") {
            continue;
        }
        if (key === "imageUrl") {
            html += `<div class="${columnClass}">
                <img src="${value}" class="img-fluid"/>
            </div>`
            continue;
        }
        html += `<div class="${columnClass}">
                <p>
                    ${value}
                </p>
            </div>`
    }
    return html;
}

function displayDeleteErrors(errors) {
    $(".alert-success").remove();
    setTimeout(function () {
        for (var i = 0; i < errors.length; i++) {
            $("#main").find("main").prepend(`<div class="alert alert-danger alert-dismissible mt-2 mb-2" role="alert">
            <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            ${errors[i].description}
        </div>`);
        }
    }, 50)
}
function displaySuccessMessageAfterDeletion(message) {
    $(".alert-success").remove();
    setTimeout(function () {
        $("#main").find("main").prepend(`<div class="alert alert-success alert-dismissible mt-2 mb-2" role="alert">
            <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                "${message}"
            </div>`);
    }, 50)
}
function displaySuccessMessageAfterUpdate(message) {
    $(".alert-success").remove();
    setTimeout(function () {
        $(`<div class="alert alert-success alert-dismissible mt-2 mb-2" role="alert">
            <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                "${message}"
            </div>`).insertBefore("#update-role-form");
    }, 50)
}
function reshapeModalAfterUpdate(title) {
    $("#cru-modal").find("h4").text(title);
    $("#cru-modal").find(".text-danger").html("");
}
function reloadValidationScripts() {
    $("#validOne").remove();
    $("#validTwo").remove();

    var valOne = document.createElement("script");
    valOne.type = "text/javascript";
    valOne.src = "/Identity/lib/jquery-validation/dist/jquery.validate.js";
    valOne.id = "validOne";
    $("body").append(valOne);
    var valTwo = document.createElement("script");
    valTwo.type = "text/javascript";
    valTwo.src = "/Identity/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js";
    valTwo.id = "validTwo";
    $("body").append(valTwo);
}