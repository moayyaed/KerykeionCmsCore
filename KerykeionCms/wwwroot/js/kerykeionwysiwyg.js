$(document).ready(function () {
    var toolbar = $("#kerykeion-wysiwyg-toolbar");
    toolbar.addClass("border border-white min-h-80px bg-black mb-1 p-1");

    var editor = $("#kerykeion-wysiwyg-editor");
    editor.addClass("border border-black min-h-200px bg-white mb-2 text-dark p-2");
    editor.attr("contenteditable", true);

    var formatSelect = document.createElement("select");
    $(formatSelect).addClass("d-inline-block mr-1 p-1 mt-1");
    $(formatSelect).html(`<option selected>- formatering -</option>
        <option value="h1">Titel 1 &lt;h1&gt;</option>
        <option value="h2">Titel 2 &lt;h2&gt;</option>
        <option value="h3">Titel 3 &lt;h3&gt;</option>
        <option value="h4">Titel 4 &lt;h4&gt;</option>
        <option value="h5">Titel 5 &lt;h5&gt;</option>
        <option value="h6">Ondertitel &lt;h6&gt;</option>
        <option value="p">Paragraaf &lt;p&gt;</option>
        <option value="pre">Preformateerd &lt;pre&gt;</option>`
    );
    $(formatSelect).on("change", function () {
        document.execCommand('formatblock', false, $(this).children("option:selected").val());
        editor.focus();
    });
    toolbar.append(formatSelect);

    var fontSizeSelect = document.createElement("select");
    $(fontSizeSelect).html(`<option class="heading" selected>- grootte -</option>
<option value="1">Heel klein</option>
<option value="2">Een beetje klein</option>
<option value="3">Normaal</option>
<option value="4">Medium-groot</option>
<option value="5">Groot</option>
<option value="6">Heel groot</option>
<option value="7">Maximum</option>`);
    $(fontSizeSelect).on("change", function () {
        document.execCommand('fontsize', false, $(this).children("option:selected").val());
    });
    $(fontSizeSelect).addClass("d-inline-block mr-1 p-1 mt-1");
    toolbar.append(fontSizeSelect);


    var toolsDiv = document.createElement("div");
    var cleanTool = document.createElement("img");
    cleanTool.className = "cursor-pointer p-1";
    cleanTool.title = "Alles verwijderen";
    cleanTool.setAttribute("data-toggle", "tooltip");
    cleanTool.src = "data:image/gif;base64,R0lGODlhFgAWAIQbAD04KTRLYzFRjlldZl9vj1dusY14WYODhpWIbbSVFY6O7IOXw5qbms+wUbCztca0ccS4kdDQjdTLtMrL1O3YitHa7OPcsd/f4PfvrvDv8Pv5xv///////////////////yH5BAEKAB8ALAAAAAAWABYAAAV84CeOZGmeaKqubMteyzK547QoBcFWTm/jgsHq4rhMLoxFIehQQSAWR+Z4IAyaJ0kEgtFoLIzLwRE4oCQWrxoTOTAIhMCZ0tVgMBQKZHAYyFEWEV14eQ8IflhnEHmFDQkAiSkQCI2PDC4QBg+OAJc0ewadNCOgo6anqKkoIQA7";
    $(cleanTool).on("click", function () {
        if (confirm("Bent u zeker dat u alles wilt verwijderen?")) {
            editor.html("");
        }
    });
    var undoTool = document.createElement("img");
    undoTool.className = "cursor-pointer  p-1";
    undoTool.title = "Ongedaan maken";
    undoTool.setAttribute("data-toggle", "tooltip");
    undoTool.src = "data:image/gif;base64,R0lGODlhFgAWAOMKADljwliE33mOrpGjuYKl8aezxqPD+7/I19DV3NHa7P///////////////////////yH5BAEKAA8ALAAAAAAWABYAAARR8MlJq7046807TkaYeJJBnES4EeUJvIGapWYAC0CsocQ7SDlWJkAkCA6ToMYWIARGQF3mRQVIEjkkSVLIbSfEwhdRIH4fh/DZMICe3/C4nBQBADs=";
    $(undoTool).on("click", function () {
        document.execCommand("undo", false);
    });
    var redoTool = document.createElement("img");
    redoTool.className = "cursor-pointer  p-1";
    redoTool.title = "Opnieuw";
    redoTool.setAttribute("data-toggle", "tooltip");
    redoTool.src = "data:image/gif;base64,R0lGODlhFgAWAMIHAB1ChDljwl9vj1iE34Kl8aPD+7/I1////yH5BAEKAAcALAAAAAAWABYAAANKeLrc/jDKSesyphi7SiEgsVXZEATDICqBVJjpqWZt9NaEDNbQK1wCQsxlYnxMAImhyDoFAElJasRRvAZVRqqQXUy7Cgx4TC6bswkAOw==";
    $(redoTool).on("click", function () {
        document.execCommand("redo", false);
    });
    var boldTool = document.createElement("img");
    boldTool.className = "cursor-pointer  p-1";
    boldTool.title = "Vet";
    boldTool.setAttribute("data-toggle", "tooltip");
    boldTool.src = "data:image/gif;base64,R0lGODlhFgAWAID/AMDAwAAAACH5BAEAAAAALAAAAAAWABYAQAInhI+pa+H9mJy0LhdgtrxzDG5WGFVk6aXqyk6Y9kXvKKNuLbb6zgMFADs=";
    $(boldTool).on("click", function () {
        document.execCommand("bold", false);
    });
    var italicTool = document.createElement("img");
    italicTool.className = "cursor-pointer  p-1";
    italicTool.title = "Cursief";
    italicTool.setAttribute("data-toggle", "tooltip");
    italicTool.src = "data:image/gif;base64,R0lGODlhFgAWAKEDAAAAAF9vj5WIbf///yH5BAEAAAMALAAAAAAWABYAAAIjnI+py+0Po5x0gXvruEKHrF2BB1YiCWgbMFIYpsbyTNd2UwAAOw==";
    $(italicTool).on("click", function () {
        document.execCommand("italic", false);
    });
    var underlineTool = document.createElement("img");
    underlineTool.className = "cursor-pointer  p-1";
    underlineTool.title = "Onderstrepen";
    underlineTool.setAttribute("data-toggle", "tooltip");
    underlineTool.src = "data:image/gif;base64,R0lGODlhFgAWAKECAAAAAF9vj////////yH5BAEAAAIALAAAAAAWABYAAAIrlI+py+0Po5zUgAsEzvEeL4Ea15EiJJ5PSqJmuwKBEKgxVuXWtun+DwxCCgA7";
    $(underlineTool).on("click", function () {
        document.execCommand("underline", false);
    });
    var justLeftTool = document.createElement("img");
    justLeftTool.className = "cursor-pointer  p-1";
    justLeftTool.title = "Links uitlijnen";
    justLeftTool.setAttribute("data-toggle", "tooltip");
    justLeftTool.src = "data:image/gif;base64,R0lGODlhFgAWAID/AMDAwAAAACH5BAEAAAAALAAAAAAWABYAQAIghI+py+0Po5y02ouz3jL4D4JMGELkGYxo+qzl4nKyXAAAOw==";
    $(justLeftTool).on("click", function () {
        document.execCommand("justifyleft", false);
    });
    var justCenterTool = document.createElement("img");
    justCenterTool.className = "cursor-pointer  p-1";
    justCenterTool.title = "Centreren";
    justCenterTool.setAttribute("data-toggle", "tooltip");
    justCenterTool.src = "data:image/gif;base64,R0lGODlhFgAWAID/AMDAwAAAACH5BAEAAAAALAAAAAAWABYAQAIfhI+py+0Po5y02ouz3jL4D4JOGI7kaZ5Bqn4sycVbAQA7";
    $(justCenterTool).on("click", function () {
        document.execCommand("justifycenter", false);
    });
    var justRightTool = document.createElement("img");
    justRightTool.className = "cursor-pointer  p-1";
    justRightTool.title = "Rechts uitlijnen";
    justRightTool.setAttribute("data-toggle", "tooltip");
    justRightTool.src = "data:image/gif;base64,R0lGODlhFgAWAID/AMDAwAAAACH5BAEAAAAALAAAAAAWABYAQAIghI+py+0Po5y02ouz3jL4D4JQGDLkGYxouqzl43JyVgAAOw==";
    $(justRightTool).on("click", function () {
        document.execCommand("justifyright", false);
    });
    var orderedLstTool = document.createElement("img");
    orderedLstTool.className = "cursor-pointer  p-1";
    orderedLstTool.title = "Een genummerd lijst beginnen";
    orderedLstTool.setAttribute("data-toggle", "tooltip");
    orderedLstTool.src = "data:image/gif;base64,R0lGODlhFgAWAMIGAAAAADljwliE35GjuaezxtHa7P///////yH5BAEAAAcALAAAAAAWABYAAAM2eLrc/jDKSespwjoRFvggCBUBoTFBeq6QIAysQnRHaEOzyaZ07Lu9lUBnC0UGQU1K52s6n5oEADs=";
    $(orderedLstTool).on("click", function () {
        document.execCommand("insertorderedlist", false);
    });
    var unOrderedLstTool = document.createElement("img");
    unOrderedLstTool.className = "cursor-pointer  p-1";
    unOrderedLstTool.title = "Een lijst beginnen";
    unOrderedLstTool.setAttribute("data-toggle", "tooltip");
    unOrderedLstTool.src = "data:image/gif;base64,R0lGODlhFgAWAMIGAAAAAB1ChF9vj1iE33mOrqezxv///////yH5BAEAAAcALAAAAAAWABYAAAMyeLrc/jDKSesppNhGRlBAKIZRERBbqm6YtnbfMY7lud64UwiuKnigGQliQuWOyKQykgAAOw==";
    $(unOrderedLstTool).on("click", function () {
        document.execCommand("insertunorderedlist", false);
    });
    var quoteTool = document.createElement("img");
    quoteTool.className = "cursor-pointer  p-1";
    quoteTool.title = "Quote";
    quoteTool.setAttribute("data-toggle", "tooltip");
    quoteTool.src = "data:image/gif;base64,R0lGODlhFgAWAIQXAC1NqjFRjkBgmT9nqUJnsk9xrFJ7u2R9qmKBt1iGzHmOrm6Sz4OXw3Odz4Cl2ZSnw6KxyqO306K63bG70bTB0rDI3bvI4P///////////////////////////////////yH5BAEKAB8ALAAAAAAWABYAAAVP4CeOZGmeaKqubEs2CekkErvEI1zZuOgYFlakECEZFi0GgTGKEBATFmJAVXweVOoKEQgABB9IQDCmrLpjETrQQlhHjINrTq/b7/i8fp8PAQA7";
    $(quoteTool).on("click", function () {
        document.execCommand("formatblock", false, "blockquote");
    });
    var linkTool = document.createElement("img");
    linkTool.className = "cursor-pointer  p-1";
    linkTool.title = "Hyperlink";
    linkTool.setAttribute("data-toggle", "tooltip");
    linkTool.src = "data:image/gif;base64,R0lGODlhFgAWAOMKAB1ChDRLY19vj3mOrpGjuaezxrCztb/I19Ha7Pv8/f///////////////////////yH5BAEKAA8ALAAAAAAWABYAAARY8MlJq7046827/2BYIQVhHg9pEgVGIklyDEUBy/RlE4FQF4dCj2AQXAiJQDCWQCAEBwIioEMQBgSAFhDAGghGi9XgHAhMNoSZgJkJei33UESv2+/4vD4TAQA7";
    $(linkTool).on("click", function () {
        var link = prompt("Schrijf hier de URL", 'http:\/\/')
        if (link && link != '' && link != 'http://')
            document.execCommand("createLink", false, link);
    });



    $(toolsDiv).addClass("bg-white mt-2 mb-2")
        .append(cleanTool)
        .append(undoTool)
        .append(redoTool)
        .append(boldTool)
        .append(italicTool)
        .append(underlineTool)
        .append(justLeftTool)
        .append(justCenterTool)
        .append(justRightTool)
        .append(orderedLstTool)
        .append(unOrderedLstTool)
        .append(linkTool);

    $(toolbar).append(toolsDiv);

    $('[data-toggle="tooltip"]').tooltip();
});

function validateMode(element) {
    if (!document.compForm.switchMode.checked) { return true; }
    alert("Uncheck \"Show HTML\".");
    element.focus();
    return false;
}