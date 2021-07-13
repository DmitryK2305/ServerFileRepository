// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $("#adddir-btn").click(function (event) {
        event.preventDefault();
        let name = $("#name").val();
        if (name !== "") {
            url = "FileSystem/AddDirectory?name=" + name;
            //alert("you will be redirected to: " + url)
            window.location.href = url
        }
    });
    $("#uplfile-btn").click(function (event) {
        event.preventDefault();
        let path = $("#path").val();
        if (path !== "") {
            url = "FileSystem/UploadFile?path=" + path;
            //alert("you will be redirected to: " + url)
            window.location.href = url
        }
    });
})