/// <reference path="/Scripts/jquery-2.0.3-vsdoc.js" />
// runs in view.cshtml

$(function () {

    // control initialization
    var isFiltering = false;
    var btnGrayscale = $("#btnGrayscale");
    var btnInvert = $("#btnInvert");
    var btnDarkblue = $("#btnDarkblue");
    var btnSepiatone = $("#btnSepiatone");
    var btnMirror = $("#btnMirror");
    var imgOriginal = $("#img");
    var imgFiltered = $("#imgFiltered");
    var ajaxLoader = $("#ajaxLoader");
    var title = $("#h2-Intro");

    // initial control state
    ajaxLoader.hide();
    imgFiltered.hide();


    // gathering input, the base64 image 
    var imageSource = imgOriginal.attr("src");
    var base64 = imageSource.split(",")[1];


    // button click handlers
    btnGrayscale.click(function () { if (!isFiltering) sendFilter("grayscale"); });
    btnSepiatone.click(function () { if (!isFiltering) sendFilter("sepiatone"); });
    btnDarkblue.click(function () { if (!isFiltering) sendFilter("darkblue"); });
    btnInvert.click(function () { if (!isFiltering) sendFilter("invert"); });
    btnMirror.click(function () { if (!isFiltering) sendFilter("mirror"); });

    // send out a 'filter' name and the image to server for preccessing
    // ofcourse as an Ajax call.
    function sendFilter(filter) {
        isFiltering = true;
        var requestData = { Image: base64, Filter: filter };
        var requestDataString = JSON.stringify(requestData);
        $.ajax({
            url: "/Home/FilterImage",
            dataType: "application/json",
            type: "POST",
            beforeSend: function () {
                ajaxLoader.show();
            },
            data: { json: requestDataString },
            complete: function (result) {
                isFiltering = false;
                ajaxLoader.hide();

                // processing 'result' (the respone from the server)
                var response = result.responseText; // getting response text.
                response = response.substring(1, response.length - 1); // removing "".
                var responseData = response.split("@@@"); // spliting data into 
                var image = responseData[0]; // index[0] gives the image
                var duration = responseData[1]; // index[1] gives the duration

                // updating the 'view'
                title.text("Editing image took about " + duration + " second(s)");
                if (!imgFiltered.hasClass()) imgFiltered.addClass("image");
                imgFiltered.show();
                imgFiltered.css("opacity", 0);
                imgFiltered.attr("src", "data:image/jpg;base64," + image);
                imgFiltered.animate({ opacity: "1" }, 1000);
            }
        });
    }
});