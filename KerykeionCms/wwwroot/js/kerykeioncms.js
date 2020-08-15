$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();

    $(document).on("mouseenter", "#user-navigations-wrapper", function () {
        $(this).children(".position-absolute").removeClass("d-none");
    });

    $(document).on("mouseleave", "#user-navigations-wrapper", function () {
        $(this).children(".position-absolute").addClass("d-none");
    });

    $(document).on("click", "#open-language-picker", function () {
        $(this).children(".position-absolute").removeClass("d-none");
    });

    $(document).on("mouseleave", "#open-language-picker", function () {
        $(this).children(".position-absolute").addClass("d-none");
    });

    $(document).on("click", ".btn-chosen-language", function () {
        $("#chosen-language").val($(this).data("language"));
        $(this).parent("form").submit();
    });

    $(document).on("click", ".entity-value-to-copy-to-clipboard", function () {
        var txtToCopy = $(this);
        txtToCopy.select();
        document.execCommand("copy");
    });

    $(document).on("click", "#open-sidebar", function () {
        $(this).addClass("d-none");
        document.getElementById("side-navigation").style.width = "250px";
        document.getElementById("main").style.marginLeft = "253px";
        document.getElementById("top-navigation").style.marginLeft = "253px";
        document.getElementById("side-bar-dragger").style.left = "250px";
        document.getElementById("side-bar-dragger").style.width = "3px";
    });

    $(document).on("click", "#close-sidebar", function () {
        $("#open-sidebar").removeClass("d-none");
        document.getElementById("side-navigation").style.width = "0";
        document.getElementById("main").style.marginLeft = "0";
        document.getElementById("top-navigation").style.marginLeft = "0";
        document.getElementById("side-bar-dragger").style.width = "0";
        document.getElementById("side-bar-dragger").style.left = "0";
        $("#main").removeClass("overflow-auto").removeClass("text-nowrap");
    });

    $(document).on("mousedown", "#side-bar-dragger", function () {
        $("#side-bar-dragger-overlay").removeClass("d-none");
        $(document.body).addClass("cursor-ew-resize");
        $(document).on("mousemove", function (evt) {
            document.getElementById("side-navigation").style.width = `${evt.pageX}px`;
            document.getElementById("main").style.marginLeft = `${evt.pageX + 3}px`;
            document.getElementById("top-navigation").style.marginLeft = `${evt.pageX + 3}px`;
            $("#side-navigation").addClass("user-select-none");
            $("#kerykeion-wysiwyg-editor").addClass("user-select-none");
            $("#main").addClass("user-select-none");
            $("#top-navigation").addClass("user-select-none");
            document.getElementById("side-bar-dragger").style.left = `${evt.pageX}px`;
            if (document.getElementById("side-navigation").offsetWidth > ($(window).width() / 2)) {
                $("#main").addClass("overflow-auto").addClass("text-nowrap");
                document.getElementById("side-navigation").style.width = `${$(window).width() / 2}px`;
                document.getElementById("main").style.marginLeft = `${$(window).width() / 2}px`;
                document.getElementById("top-navigation").style.marginLeft = `${$(window).width() / 2}px`;
                document.getElementById("side-bar-dragger").style.left = `${$(window).width() / 2}px`;
            }
            else {
                $("#main").removeClass("overflow-auto").removeClass("text-nowrap");
            }
        });
    });

    $(document).on("mouseup", function () {
        $("#side-bar-dragger-overlay").addClass("d-none");
        $("#side-bar-dragger").off("mousedown");
        $(document).off("mousemove");
        $("#kerykeion-wysiwyg-editor").removeClass("user-select-none");
        $("#main").removeClass("user-select-none");
        $("#top-navigation").removeClass("user-select-none");
        $(document.body).removeClass("cursor-ew-resize");
    });

    manipulateEntitySubNavs(".sub-page-right-caret", ".sub-page-down-caret", "-page-subs", false, true);
    manipulateEntitySubNavs(".sub-page-down-caret", ".sub-page-right-caret", "-page-subs", true, true);

    manipulateEntitySubNavs(".page-links-right-caret", ".page-links-down-caret", "-page-links", false, true);
    manipulateEntitySubNavs(".page-links-down-caret", ".page-links-right-caret", "-page-links", true, true);

    manipulateEntitySubNavs(".page-articles-right-caret", ".page-articles-down-caret", "-page-articles", false, true);
    manipulateEntitySubNavs(".page-articles-down-caret", ".page-articles-right-caret", "-page-articles", true, true);

    manipulateEntitySubNavs(".table-right-caret", ".table-down-caret", "-entities", false, false);
    manipulateEntitySubNavs(".table-down-caret", ".table-right-caret", "-entities", true, false);


    openSubnav("#open-subnav-entities", $("#subnav-entities"), $("#close-subnav-entities"));
    openSubnav("#open-subnav-pages", $("#subnav-webpages"), $("#close-subnav-pages"));
    openSubnav("#open-subnav-articles", $("#subnav-articles"), $("#close-subnav-articles"));
    openSubnav("#open-subnav-roles", $("#subnav-roles"), $("#close-subnav-roles"));
    openSubnav("#open-subnav-images", $("#subnav-images"), $("#close-subnav-images"));


    closeSubnav("#close-subnav-entities", $("#subnav-entities"), $("#open-subnav-entities"));
    closeSubnav("#close-subnav-pages", $("#subnav-webpages"), $("#open-subnav-pages"));
    closeSubnav("#close-subnav-articles", $("#subnav-articles"), $("#open-subnav-articles"));
    closeSubnav("#close-subnav-roles", $("#subnav-roles"), $("#open-subnav-roles"));
    closeSubnav("#close-subnav-images", $("#subnav-images"), $("#open-subnav-images"));




    $(document).on("click", "#open-add-roles-modal", function () {
        $("#add-roles-to-user-form-modal").modal("show");
        postAjax(JSON.stringify($(this).data("userid")), `/KerykeionCms/Users?handler=GetRoles`, "application/json;charset=utf-8", function (result) {
            $("#add-roles-to-user-select").html("");
            $("#UserId").remove();
            for (var i = 0; i < result.roles.length; i++) {
                var option = document.createElement("option");
                option.value = result.roles[i].name;
                option.text = result.roles[i].name;
                $("#add-roles-to-user-select").append(option);
            }
            var input = document.createElement("input");
            input.value = result.id;
            input.id = "UserId";
            input.name = "UserId";
            input.type = "hidden";
            $("#form-add-roles-to-user").append(input);
        });
        hideSpinner();
    });

    $(document).on("click", "#submit-add-roles-to-user", function () {
        if ($("#add-roles-to-user-select :selected").length > 0) {
            showSpinner();
            $("#form-add-roles-to-user").submit();
        }
        else {
            $("#validation-add-roles").removeClass("d-none");
        }
    });

    $(document).on("change", "#add-roles-to-user-select", function () {
        if ($("#add-roles-to-user-select :selected").length > 0) {
            $("#validation-add-roles").addClass("d-none");
        }
    });

    $(document).on("click", "#remove-role-from-user", function () {
        $(this).parent().submit();
    });

    $(document).on("click", "#account-profile-image", function () {
        $("#new-image-input").trigger("click");
    });

    $(document).on("click", "#replace-entity-image", function () {
        $("#replace-entity-image-input").trigger("click");
    });

    $(document).on("click", "#profile-pic", function () {
        $("#add-profile-img-inp").trigger("click");
    });

    readImgFile("#new-image-input", "#account-profile-image");
    readImgFile("#add-profile-img-inp", "#profile-pic");
    readImgFile("#replace-entity-image-input", "#replace-entity-image")

    $(document).on("click", "#btn-add-article", function () {
        replaceUnwantedWysiwygElemsWithWantedElems($("#kerykeion-wysiwyg-editor"));
        $("#txt-add-article-wysiwyg").html($("#kerykeion-wysiwyg-editor").html());
        $(this).parents("form").submit();
    });

    $(document).on("click", "#btn-update-article", function () {
        replaceUnwantedWysiwygElemsWithWantedElems($("#kerykeion-wysiwyg-editor"));
        $("#txt-update-article-wysiwyg").html($("#kerykeion-wysiwyg-editor").html());
        $(this).parents("form").submit();
    });

    $(document).on("dblclick", "#open-close-entities-wrapper", function () {
        if ($("#subnav-entities").hasClass("d-none")) {
            $("#open-subnav-entities").addClass("d-none").removeClass("d-inline-block");
            $("#close-subnav-entities").removeClass("d-none").addClass("d-inline-block");
            $("#subnav-entities").removeClass("d-none");
            $(this).children().find(".fa-folder-o").removeClass("fa-folder-o").addClass("fa-folder-open-o");
        }
        else {
            $("#open-subnav-entities").removeClass("d-none").addClass("d-inline-block");
            $("#close-subnav-entities").addClass("d-none").removeClass("d-inline-block");
            $("#subnav-entities").addClass("d-none");
            $(this).children().find(".fa-folder-open-o").addClass("fa-folder-o").removeClass("fa-folder-open-o");
        }
    });

    $(document).on("click", "#sort-articles-by-name", function (evt) {
        evt.preventDefault();
        postAjax(JSON.stringify(prepSortAjax($(this), $("#sort-articles-by-datetime"), "fa-sort-alpha-asc", "fa-sort-alpha-desc", "fa-sort-asc", "fa-sort-desc", "fa-sort")), $(this).attr("href"), "application/json;charset=utf-8", function (result) {
            $("#articles-wrapper").html(getHtmlContainingSortedEntitiesByAjax(result));
        });
    });

    $(document).on("click", "#sort-articles-by-datetime", function (evt) {
        evt.preventDefault();
        postAjax(JSON.stringify(prepSortAjax($(this), $("#sort-articles-by-title"), "fa-sort-asc", "fa-sort-desc", "fa-sort-alpha-asc", "fa-sort-alpha-desc", "fa-sort")), $(this).attr("href"), "application/json;charset=utf-8", function (result) {
            $("#articles-wrapper").html(getHtmlContainingSortedEntitiesByAjax(result));
        });
    });

    $(document).on("click", "#sort-pages-by-name", function (evt) {
        evt.preventDefault();
        postAjax(JSON.stringify(prepSortAjax($(this), $("#sort-pages-by-datetime"), "fa-sort-alpha-asc", "fa-sort-alpha-desc", "fa-sort-asc", "fa-sort-desc", "fa-sort")), $(this).attr("href"), "application/json;charset=utf-8", function (result) {
            $("#pages-wrapper").html(getHtmlContainingSortedEntitiesByAjax(result));
        });
    });

    $(document).on("click", "#sort-pages-by-datetime", function (evt) {
        evt.preventDefault();
        postAjax(JSON.stringify(prepSortAjax($(this), $("#sort-pages-by-name"), "fa-sort-asc", "fa-sort-desc", "fa-sort-alpha-asc", "fa-sort-alpha-desc", "fa-sort")), $(this).attr("href"), "application/json;charset=utf-8", function (result) {
            $("#pages-wrapper").html(getHtmlContainingSortedEntitiesByAjax(result));
        });
    });

    $(document).on("click", "#sort-links-by-name", function (evt) {
        evt.preventDefault();
        postAjax(JSON.stringify(prepSortAjax($(this), $("#sort-links-by-datetime"), "fa-sort-alpha-asc", "fa-sort-alpha-desc", "fa-sort-asc", "fa-sort-desc", "fa-sort")), $(this).attr("href"), "application/json;charset=utf-8", function (result) {
            $("#links-wrapper").html(getHtmlContainingSortedEntitiesByAjax(result));
        });
    });

    $(document).on("click", "#sort-links-by-datetime", function (evt) {
        evt.preventDefault();
        postAjax(JSON.stringify(prepSortAjax($(this), $("#sort-links-by-name"), "fa-sort-asc", "fa-sort-desc", "fa-sort-alpha-asc", "fa-sort-alpha-desc", "fa-sort")), $(this).attr("href"), "application/json;charset=utf-8", function (result) {
            $("#links-wrapper").html(getHtmlContainingSortedEntitiesByAjax(result));
        });
    });

    $(document).on("click", "#sort-entities-by-name", function (evt) {
        evt.preventDefault();
        postAjax(JSON.stringify(prepSortAjax($(this), $("#sort-entities-by-datetime"), "fa-sort-alpha-asc", "fa-sort-alpha-desc", "fa-sort-asc", "fa-sort-desc", "fa-sort")), $(this).attr("href"), "application/json;charset=utf-8", function (result) {
            $("#entities-wrapper").html(getHtmlContainingSortedEntitiesByAjax(result));
        });
    });

    $(document).on("click", "#sort-entities-by-datetime", function (evt) {
        evt.preventDefault();
        postAjax(JSON.stringify(prepSortAjax($(this), $("#sort-entities-by-name"), "fa-sort-asc", "fa-sort-desc", "fa-sort-alpha-asc", "fa-sort-alpha-desc", "fa-sort")), $(this).attr("href"), "application/json;charset=utf-8", function (result) {
            $("#entities-wrapper").html(getHtmlContainingSortedEntitiesByAjax(result));
        });
    });

    $(document).on("click", "#sort-images-by-name", function (evt) {
        evt.preventDefault();
        postAjax(JSON.stringify(prepSortAjax($(this), $("#sort-images-by-datetime"), "fa-sort-alpha-asc", "fa-sort-alpha-desc", "fa-sort-asc", "fa-sort-desc", "fa-sort")), $(this).attr("href"), "application/json;charset=utf-8", function (result) {
            $("#images-wrapper").html(getHtmlContainingSortedEntitiesByAjax(result));
        });
    });

    $(document).on("click", "#sort-images-by-datetime", function (evt) {
        evt.preventDefault();
        postAjax(JSON.stringify(prepSortAjax($(this), $("#sort-images-by-name"), "fa-sort-asc", "fa-sort-desc", "fa-sort-alpha-asc", "fa-sort-alpha-desc", "fa-sort")), $(this).attr("href"), "application/json;charset=utf-8", function (result) {
            $("#images-wrapper").html(getHtmlContainingSortedEntitiesByAjax(result));
        });
    });

    $("#add-entity-form").on("submit", function () {
        sendBooleanValueForCheckboxes($(this));
    });

    $("#update-entity-form").on("submit", function () {
        sendBooleanValueForCheckboxes($(this))
    });
});

function prepSortAjax(clickedSorter, partnerSorter, ascClass, descClass, partnerAscClass, partnerDescClass, baseClass) {
    var hiddenInput = clickedSorter.find("input");
    var iEl = clickedSorter.find("i");
    var sortingOrder = hiddenInput.val();

    var dto = {
        SortingOrder: sortingOrder,
        PageId: clickedSorter.data("pageid"),
        Table: clickedSorter.data("table")
    };

    if (sortingOrder === "Ascending") {
        hiddenInput.val("Descending");
        iEl.addClass(descClass).removeClass(ascClass).removeClass(baseClass);
    }
    else if (sortingOrder === "Descending" || sortingOrder === "None") {
        hiddenInput.val("Ascending");
        iEl.addClass(ascClass).removeClass(descClass).removeClass(baseClass);
    }


    partnerSorter.find("i").addClass(baseClass).removeClass(partnerAscClass).removeClass(partnerDescClass);
    partnerSorter.find("input").val("None");

    return dto;
}

function createDeleteForm(result, entityId) {
    var verfTokenEl = document.getElementById("verif-token-holder").innerHTML;
    var formEl = document.createElement("form");
    formEl.method = "Post";
    formEl.className = "ml-1 d-inline-block";
    formEl.action = `/KerykeionCms/${result.deleteReturnPage}?id=${entityId}&handler=Delete`;
    $(formEl).append(`${getHiddenInputIfNeeded(result)}
                    <button type="submit" class="btn btn-danger">
                        ${result.txtDelete}
                        <i class="fa fa-trash-o" aria-hidden="true"></i>
                    </button>${verfTokenEl}`);
    return formEl.outerHTML;
}

function getHiddenInputIfNeeded(result) {
    if (result.table != null) {
        return `<input type="hidden" id="TableName" name="TableName" value="${result.table}" />`;
    }
    if (result.webpageId != null) {
        return `<input type="hidden" id="PageId" name="PageId" value="${result.webpageId}" />`;
    }
    return "";
}

function getTableUrlIfAny(result) {
    if (result.table == null) {
        return "";
    }
    return `&table=${result.table}`;
}

function readImgFile(fileInputElemId, imgElemId) {
    $(document).on("change", fileInputElemId, function () {
        var reader = new FileReader();
        reader.onload = readSucces;
        function readSucces(e) {
            $(imgElemId).attr("src", e.target.result);
        }

        reader.readAsDataURL(this.files[0]);
    });
}

function replaceUnwantedWysiwygElemsWithWantedElems(containerElem) {
    var elemITag = containerElem.find("i");
    var elemIText = elemITag.text();
    elemITag.replaceWith(`<em>${elemIText}</em>`);
    var elemBTag = containerElem.find("b");
    var elemBText = elemBTag.text();
    elemBTag.replaceWith(`<strong>${elemBText}</strong>`);
}


function manipulateEntitySubNavs(clickedElClass, clickedElSiblingClass, subExtension, isOpen, isPage) {
    $(document).on("click", clickedElClass, function () {
        $(this).removeClass("d-inline-block").addClass("d-none");
        $(this).siblings(clickedElSiblingClass).first().addClass("d-inline-block").removeClass("d-none");
        if (isPage) {
            var pageId = $(this).data("pageid");
            if (isOpen) {
                $(`#${pageId}${subExtension}`).addClass("d-none");
                $(this).siblings().children().find(".fa-folder-open-o").first().removeClass("fa-folder-open-o").addClass("fa-folder-o");
            }
            else {
                $(`#${pageId}${subExtension}`).removeClass("d-none");
                $(this).siblings().children().find(".fa-folder-o").first().removeClass("fa-folder-o").addClass("fa-folder-open-o");
            }
        }
        else {
            var table = $(this).data("table");
            if (isOpen) {
                $(`#${table}${subExtension}`).addClass("d-none");
                $(this).siblings().children().find(".fa-folder-open-o").first().removeClass("fa-folder-open-o").addClass("fa-folder-o");
            }
            else {
                $(`#${table}${subExtension}`).removeClass("d-none");
                $(this).siblings().children().find(".fa-folder-o").first().removeClass("fa-folder-o").addClass("fa-folder-open-o");
            }
        }
    });
}

function openSubnav(clickedEl, subnavToOpen, caretToDisplay) {
    $(document).on("click", clickedEl, function () {
        $(this).addClass("d-none").removeClass("d-inline-block");
        subnavToOpen.removeClass("d-none");
        caretToDisplay.removeClass("d-none").addClass("d-inline-block");
        $(this).siblings().find(".fa-folder-o").removeClass("fa-folder-o").addClass("fa-folder-open-o");
    });
}

function closeSubnav(clickedEl, subnavToOpen, caretToDisplay) {
    $(document).on("click", clickedEl, function () {
        $(this).addClass("d-none").removeClass("d-inline-block");
        subnavToOpen.addClass("d-none");
        caretToDisplay.removeClass("d-none").addClass("d-inline-block");
        $(this).siblings().find(".fa-folder-open-o").removeClass("fa-folder-open-o").addClass("fa-folder-o");
    });
}

function sendBooleanValueForCheckboxes(form) {
    var checkboxes = form.find(':checkbox');
    $(checkboxes).each(function () {
        if ($(this).is(':checked')) {
            $(this).attr('value', 'true');
        } else {
            $(this).prop("checked", true);
            $(this).attr('value', 'false');
        }
    });
}

function postAjax(postValue, postRoute, contentTYPE, reslt) {
    $.ajax({
        type: "POST",
        processData: false,
        contentType: contentTYPE,
        data: postValue,
        dataType: "json",
        async: true,
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        url: postRoute
    }).fail(function (result) {
        console.log("AJAX ERROR");
        console.log(result);
    }).done(function (result) {
        reslt(result);
    });
}

function showSpinner() {
    $("#overlay").removeClass("d-none");
}

function hideSpinner() {
    $("#overlay").addClass("d-none");
}