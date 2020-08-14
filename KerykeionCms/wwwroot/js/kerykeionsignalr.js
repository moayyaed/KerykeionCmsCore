"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/kerykeioncmshub").build();

connection.start().then(function () {
    console.log("SignalR Connected!");
}).catch(function (err) {
    return console.error(err.toString());
});


var areRolesLoaded = false;
connection.on("GetRoles", function (roles) {
    areRolesLoaded = true;
    var html = '';

    for (var i = 0; i < roles.length; i++) {
        html += `<a class="nav-link side-navigation text-inherit">
                    <span class="ml-5">
                        <i class="fa fa-id-badge mr-1" aria-hidden="true"></i>
                        ${roles[i].name}
                    </span>
                </a>`;
    }

    $("#subnav-roles").html(html);
});

$(document).on("click", "#open-subnav-roles", function (event) {
    if (!areRolesLoaded) {
        connection.invoke("SendRolesAsync").catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    }
});