var app = {
    // Application Constructor
    initialize: function () {

        $.ui.autoLaunch = false;
        $.ui.backButtonText = "";


        this.bindEvents();
    },
    // Bind Event Listeners
    //
    // Bind any events that are required on startup. Common events are:
    // 'load', 'deviceready', 'offline', and 'online'.
    bindEvents: function () {
        if (!window.Cordova) {
            $(document).ready(function () {
                app.readyFunction(); 
            });
        }
        document.addEventListener('deviceready', this.onDeviceReady, false);
    },
    // deviceready Event Handler
    //
    // The scope of 'this' is the event. In order to call the 'receivedEvent'
    // function, we must explicitly call 'app.receivedEvent(...);'
    onDeviceReady: function () {
        if (window.Cordova && navigator.splashscreen) {
            app.readyFunction();
        }
    },
    readyFunction: function () {
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
            localStorage.setItem("Name", $("#name").val());

            $.ui.loadContent("main", null, null, "fade");

            setTimeout(window.location = "rooms.html", 5000);
        });

        $("#Submit").on("click", function () {
            localStorage.setItem("Name", $("#changenameText").val());

            $.ui.loadContent("main", null, null, "fade");

            setTimeout(window.location = "rooms.html", 5000);
        });

        $("#GoToRooms").on("click", function () {
            window.location = "rooms.html"
        });
    } 
};

app.initialize();