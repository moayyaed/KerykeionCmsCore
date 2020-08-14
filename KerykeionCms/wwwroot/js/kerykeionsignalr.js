"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/kerykeioncmshub").build();

connection.start().then(function () {
    console.log("SignalR Connected!");
}).catch(function (err) {
    return console.error(err.toString());
});

var counter = 0;
var areSideNavRolesLoaded = false;

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
                    <button class="btn btn-success">
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
$(document).on("click", "#open-main-roles", function (event) {
    if (!$(this).parents(".side-navigation").first().hasClass("bg-secondary")) {
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