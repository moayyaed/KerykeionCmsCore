function isRequiredValid(inputId, validationEl, errorMsg) {
    var errorMessage = decodeUnicodeHexCharacters(errorMsg);
    $(inputId).off("input");
    $(inputId).on("input", function () {
        if ($.trim($(inputId).val()) === "") {
            $(validationEl).text(errorMessage);
        }
        else {
            $(validationEl).text("");
        }
    });
    if (!$(inputId).val() || $.trim($(inputId).val()) === "") {
        $(validationEl).text(errorMessage);
        return false;
    }
    $(validationEl).text("");
    return true;
}

function isStringLengthValid(minLength, maxLength, inputId, validationEl, errorMsg) {
    var errorMessage = decodeUnicodeHexCharacters(errorMsg);
    $(inputId).off("input");
    $(inputId).on("input", function () {
        if ($.trim($(inputId).val().length) < minLength || $.trim($(inputId).val().length) > maxLength) {
            $(validationEl).text(errorMessage);
        }
        else {
            $(validationEl).text("");
        }
    });
    if ($.trim($(inputId).val().length) < minLength || $.trim($(inputId).val().length) > maxLength) {
        $(validationEl).text(errorMessage);
        return false;
    }
    $(validationEl).text("");
    return true;
}

function isEmailValid(inputId, validationEl, errorMsg) {
    var errorMessage = decodeUnicodeHexCharacters(errorMsg);
    $(inputId).off("input");
    $(inputId).on("input", function () {
        if (!regex.test($(inputId).val())) {
            $(validationEl).text(errorMessage);
        }
        else {
            $(validationEl).text("");
        }
    });
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test($(inputId).val())) {
        $(validationEl).text(errorMessage);
        return false;
    }
    $(validationEl).text("");
    return true;
}

function tryCompare(inputId, compareInputId, validationEl, errorMsg) {
    var errorMessage = decodeUnicodeHexCharacters(errorMsg);
    $(inputId).off("input");
    $(inputId).on("input", function () {
        if ($(inputId).val() !== $(compareInputId).val()) {
            $(validationEl).text(errorMessage);
        }
        else {
            $(validationEl).text("");
        }
    });
    if ($(inputId).val() !== $(compareInputId).val()) {
        $(validationEl).text(errorMessage);
        return false;
    }
    $(validationEl).text("");
    return true;
}

function decodeUnicodeHexCharacters(text) {
    return text.replace(/&#x27;/g, "'")
        .replace(/&#xE9;/g, "é")
        .replace(/&#xE8;/g, "è")
        .replace(/&#xF6;/g, "ö")
        .replace(/&#xFC;/g, "ü")
        .replace(/&#x2019;/g, "'")
        .replace(/&#xE4;/g, "ä");
}