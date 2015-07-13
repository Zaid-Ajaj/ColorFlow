/// <reference path="/Scripts/jquery-2.0.3-vsdoc.js" />
// runs in Index.cshtml

$(function () {
    // Html controls initialization
    var btnUpload = $("#btnUpload");
    var btnSubmit = $("#btnSubmit");
    var form = $("#form1");
    var file = $("#file");
    var txtFileName = $("#txtFileName");
    var txtStatus = $("#txtStatus");
    var formSubmit = $("#submit");

    // controls initial state
    btnSubmit.hide();
    form.hide();

    // validate file extension
    function validate(fileName) {
        var ext = fileName.substring(fileName.length - 3);
        if (ext == "bmp" || ext == "jpg" || ext == "peg" || ext == "png")
            return true;
        else
            return false;
    }


    // File changed event handler
    file.change(function () {
        if (validate(file.val()) == true) {
            txtFileName.text("File Selected: " + file.val());
            txtStatus.text("You are good to go!");
            btnSubmit.css("opacity", "0");
            btnSubmit.show();
            btnSubmit.animate({ opacity: "1" }, 1250);
        }
        else {
            txtFileName.text("File Type not supported . . . ");
            txtStatus.text("");
            btnSubmit.hide();
        }
    });

    // bind events
    btnUpload.click(function () { file.click(); });
    btnSubmit.click(function () { form.submit(); });
});