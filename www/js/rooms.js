define(['require', 'CustomFunctions', 'signalRHub'],
function (require, custom, signal) {

    var initialize = function () {
      
        $.ui.autoLaunch = false;
        $.ui.backButtonText = "";
         
     
        bindEvents();
    };
    var bindEvents = function () {
        if (!window.Cordova) {
            $(document).ready(function () {
                readyFunction();
            });
        }
        document.addEventListener('deviceready', onDeviceReady, false);
    }
    var onDeviceReady = function () {                             // called when Cordova is ready
        if (window.Cordova && navigator.splashscreen) {
            readyFunction();
        }

    };
    var readyFunction = function () {
       
        $("li a").on("click", function () {

            var room = $(this).attr("id");
            localStorage.setItem("room", room);
            window.location = "room.html";

        });

        $("#backButton").on("click", function () {
            window.location = "index.html#signin";
        });
        $.ui.launch();
    };

    initialize();
});

