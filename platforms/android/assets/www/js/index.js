define(['require', 'CustomFunctions', 'signalRHub'],
function (require, custom, signal) {

    $("body").css("display", "none");
    $("body").fadeIn(200);

    var initialize = function () {

        $.ui.autoLaunch = false;
        $.ui.backButtonText = "Back";

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
       
        document.addEventListener('deviceready', onDeviceReady, false);
    };
    // deviceready Event Handler
    //
    // The scope of 'this' is the event. In order to call the 'receivedEvent'
    // function, we must explicitly call 'app.receivedEvent(...);'
    var onDeviceReady = function () {
        if (window.Cordova && navigator.splashscreen) {
 
            custom.initialize();

            signal.initialize();

            

            readyFunction();
        }
    };
    var readyFunction = function () {

        custom.show('loading', false);

        custom.show('afui', true);


        FastClick.attach(document.body);

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
             
        });

        $("#Submit").on("click", function () {

            if (localStorage.getItem("Name") != $("#changenameText").val()) {
                localStorage.setItem("tempName", $("#changenameText").val());

                signal.startConnection();
            } else {
                $.ui.loadContent("ResturantPicker", null, null, "fade");

               // setTimeout(window.location = "rooms.html", 5000);
            }
            //localStorage.setItem("Name", $("#changenameText").val());

            //$.ui.loadContent("main", null, null, "fade");

            //setTimeout(window.location = "rooms.html", 5000);
        });
        
        $("#GameRoom").on("click", function () {
            $('body').fadeOut(200, function () {
                document.location.href = "bhabo.html"
            });
        });
        $("#GoToRooms").on("click", function () {
            $.ui.loadContent("ResturantPicker", null, null, "fade");
        });

        $("#townList > li a").on("click", function () {

            var room = $(this).attr("id");

            localStorage.setItem("room", room);
            //window.location = "room.html";

            $('body').fadeOut(300, function () {
                document.location.href = "room.html"
            });
        });
    };

    initialize();

});
