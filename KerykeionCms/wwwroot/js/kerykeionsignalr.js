"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/kerykeioncmshub").build();

connection.start().then(function () {
    console.log("SignalR Connected!");
}).catch(function (err) {
    return console.error(err.toString());
});


var areRolesLoaded = false;
connection.on("GetRoles", function () {
    areRolesLoaded = true;
});

$(document).on("click", "#open-subnav-roles", function (event) {
    if (!areRolesLoaded) {
        connection.invoke("SendRolesAsync").catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    }
});