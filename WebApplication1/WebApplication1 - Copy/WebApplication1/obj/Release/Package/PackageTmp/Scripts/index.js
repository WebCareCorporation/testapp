define(['require', 'CustomFunctions', 'signalRHub'],
function (require, custom, signal) {

    var initialize = function () {

        $.ui.autoLaunch = false;
        $.ui.backButtonText = "";

        if (!localStorage.getItem("uniqueId")) {
            var uniqueId = guid();
            localStorage.setItem("uniqueId", uniqueId);
        }

        bindEvents();
    };

    var guid = function () {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                       .toString(16)
                       .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
              s4() + '-' + s4() + s4() + s4();

    };

    // Bind Event Listeners
    //
    // Bind any events that are required on startup. Common events are:
    // 'load', 'deviceready', 'offline', and 'online'.
    var bindEvents = function () {
        if (!window.Cordova) {
            $(document).ready(function () {
                custom.initialize();

                signal.initialize();
                readyFunction();
            });
        }
        document.addEventListener('deviceready', this.onDeviceReady, false);
    };
    // deviceready Event Handler
    //
    // The scope of 'this' is the event. In order to call the 'receivedEvent'
    // function, we must explicitly call 'app.receivedEvent(...);'
    var onDeviceReady = function () {
        if (window.Cordova && navigator.splashscreen) {

            custom.initialize();

            signal.initialize();

            //signal.initiateConnection();

            readyFunction();
        }
    };
    var readyFunction = function () {
        if (localStorage.getItem("Name") != undefined && localStorage.getItem("Name") != "") {

            if (window.Cordova && navigator.splashscreen) {
                navigator.splashscreen.hide();
            }
            $("#logname").text(localStorage.getItem("Name"));
            $.ui.launch();
            $.ui.loadContent("signin", null, null, "fade");

        } else {
            if (window.Cordova && navigator.splashscreen) {
                navigator.splashscreen.hide();
            }
            $.ui.launch();

        }


        $("#register").on("click", function () {
            localStorage.setItem("tempName", $("#name").val());

            signal.startConnection();

            //$.ui.loadContent("main", null, null, "fade");

            //setTimeout(window.location = "rooms.html", 5000);
        });

        $("#Submit").on("click", function () {

            if (localStorage.getItem("Name") != $("#changenameText").val()) {
                localStorage.setItem("tempName", $("#changenameText").val());

                signal.startConnection();
            } else {
                $.ui.loadContent("main", null, null, "fade");

                setTimeout(window.location = "rooms.html", 5000);
            }
            //localStorage.setItem("Name", $("#changenameText").val());

            //$.ui.loadContent("main", null, null, "fade");

            //setTimeout(window.location = "rooms.html", 5000);
        });

        $("#GoToRooms").on("click", function () {
            window.location = "rooms.html"
        });
    };

    initialize();

});
