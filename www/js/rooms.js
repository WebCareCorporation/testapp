var rooms = {
    // Application Constructor
    initialize: function () {

        $.ui.autoLaunch = false;
        $.ui.backButtonText = "";

       
        this.bindEvents();
    },
    bindEvents: function () {
        if (!window.Cordova) {
            $(document).ready(function () {
                rooms.readyFunction();

            });
        }
        document.addEventListener('deviceready', this.onDeviceReady, false);
    },
    onDeviceReady : function () {                             // called when Cordova is ready
        if (window.Cordova && navigator.splashscreen) {
            rooms.readyFunction();
        }
        
    },
    readyFunction : function () {
       
        $.ui.launch();
        $("li a").on("click", function () {

            var room = $(this).attr("id");
            localStorage.setItem("room", room);
            window.location = "room.html";

        });

        $("#backButton").on("click", function () {
            window.location = "index.html#signin";
        });
    }
};
rooms.initialize();

//(function ($) {

//    $.ui.autoLaunch = false;
//    $.ui.backButtonText = "Back";


//    var onDeviceReady = function () {                             // called when Cordova is ready
//        if (window.Cordova && navigator.splashscreen) {
//            readyFunction();
//        }
//    };
//    if (!window.Cordova) {
//        $(document).ready(function () {
//            readyFunction();
//        });
//    }

//    var readyFunction = function () {
       
//            $.ui.launch();
//            $("li a").on("click", function () {

//                var room = $(this).attr("id");
//                localStorage.setItem("room", room);
//                window.location = "room.html";

//            });

//            $("#backButton").on("click", function () {
//                window.location = "index.html#signin";
//            });
//    };
//    document.addEventListener("deviceready", onDeviceReady, false);
//    document.addEventListener("backbutton", function (e) {
//        window.location = "index.html";
//    }, false);

//}(jQuery));
