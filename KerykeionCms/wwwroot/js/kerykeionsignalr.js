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
                                <div class="text-white row m-1">
                                    <span class="col text-left">Open in new tab</span>
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

$(document).on("click", ".open-main-images", function (event) {
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
    var html = '';

    for (var i = 0; i < roles.length; i++) {
        html += `<span class="nav-link side-navigation text-inherit">
                    <span class="ml-5">
                        <i class="fa fa-id-badge mr-1" aria-hidden="true"></i>
                        ${roles[i].name}
                    </span>
                </span>`;
    }

    $("#subnav-roles").html(html);
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
        html += `<div class="row pt-2 pb-2 border border-secondary mb-1">
                    <div class="col-sm-4">
                        <p>
                            ${roles[i].name}
                        </p>
                    </div>
                    <div class="col-sm-4">
                    </div>
                    <div class="col text-right d-block">
                        <button class="btn btn-info d-inline-block">
                            Details
                            <i class="fa fa-pencil-square-o" aria-hidden="true"></i>
                        </button>
                        ${returnDeleteBtnIfNotDefaultRole(roles[i].name)}
                    </div>
                </div>`;
    }

    $("#main").find("main").html(html);
});
$(document).on("click", ".open-main-roles", function (event) {
    if (!$(this).parents(".side-navigation").first().hasClass("bg-secondary")) {
        removeActiveSidNavClasses();
        connection.invoke("SendMainRolesAsync").catch(function (err) {
            return console.error(err.toString());
        });
        $(this).parents(".side-navigation").first().addClass("text-dark bg-secondary")
        event.preventDefault();
    }
});

function returnDeleteBtnIfNotDefaultRole(roleName) {
    if (roleName === "Administrator" || roleName === "Editor" || roleName === "RegularUser") {
        return ``;
    }

    return `<button class="btn btn-danger">
                Delete
                <i class="fa fa-trash-o" aria-hidden="true"></i>
            </button>`;
}
//#endregion

function removeActiveSidNavClasses() {
    $(".side-navigation").each(function () {
        if ($(this).hasClass("bg-secondary")) {
            $(this).removeClass("text-dark bg-secondary");
        }
    });
}