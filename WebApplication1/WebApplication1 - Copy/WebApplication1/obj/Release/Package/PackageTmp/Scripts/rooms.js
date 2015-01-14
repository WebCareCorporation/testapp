define(['require', 'CustomFunctions', 'signalRHub'],
function (require, custom, signal) {
    $("body").css("display", "none");
    $("body").fadeIn(400);

    var initialize = function () {
      
        //$.ui.autoLaunch = false;
        //$.ui.backButtonText = "";
         
     
        bindEvents();
    };
    var bindEvents = function () {

        document.addEventListener("backbutton", function () {
            $('body').fadeOut(600, function () {
                document.location.href = "index.html"
            });

        }, false);

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
       
        

        $("#townList > li a").on("click", function () {

            var room = $(this).attr("id");
            
            localStorage.setItem("room", room);
            //window.location = "room.html";

            $('body').fadeOut(300, function () {
                document.location.href = "room.html"
            });
        });

        $("#backButton").on("click", function () {
            window.location = "index.html#signin";
        });
     //   $.ui.launch();
    };

    initialize();
});

