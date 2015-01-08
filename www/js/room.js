//var room = {
define(['require', 'CustomFunctions', 'signalRHub', 'Database'],
function (require, custom, signal, database) {



    // Application Constructor
    var initialize = function () {

        custom.initialize();
        signal.initialize();

        database.init();
        pushNotification = window.plugins.pushNotification;
        pushNotification.register(
                successHandler,
                errorHandler,
                {
                    "senderID": "third-diorama-819",
                    "ecb": "onNotification"
                });
        $.ui.autoLaunch = false;

        $.ui.backButtonText = "";

        bindEvents();
    };
    function successHandler(result) {
        alert('result = ' + result);
    }
    function errorHandler(error) {
        alert('error = ' + error);
    }

    function onNotification(e) {
        //$("#app-status-ul").append('<li>EVENT -> RECEIVED:' + e.event + '</li>');

        switch (e.event) {
            case 'registered':
                if (e.regid.length > 0) {
                    //$("#app-status-ul").append('<li>REGISTERED -> REGID:' + e.regid + "</li>");
                    // Your GCM push server needs to know the regID before it can push to this device
                    // here is where you might want to send it the regID for later use.
                    console.log("regID = " + e.regid);


                }
                break;

            case 'message':
                // if this flag is set, this notification happened while we were in the foreground.
                // you might want to play a sound to get the user's attention, throw up a dialog, etc.
                if (e.foreground) {
                    //$("#app-status-ul").append('<li>--INLINE NOTIFICATION--' + '</li>');

                    //// on Android soundname is outside the payload.
                    //// On Amazon FireOS all custom attributes are contained within payload
                    //var soundfile = e.soundname || e.payload.sound;
                    //// if the notification contains a soundname, play it.
                    //var my_media = new Media("/android_asset/www/" + soundfile);
                    //my_media.play();
                }
                else {  // otherwise we were launched because the user touched a notification in the notification tray.
                    //if (e.coldstart) {
                    //    $("#app-status-ul").append('<li>--COLDSTART NOTIFICATION--' + '</li>');
                    //}
                    //else {
                    //    $("#app-status-ul").append('<li>--BACKGROUND NOTIFICATION--' + '</li>');
                    //}
                }

                //$("#app-status-ul").append('<li>MESSAGE -> MSG: ' + e.payload.message + '</li>');
                ////Only works for GCM
                //$("#app-status-ul").append('<li>MESSAGE -> MSGCNT: ' + e.payload.msgcnt + '</li>');
                ////Only works on Amazon Fire OS
                //$status.append('<li>MESSAGE -> TIME: ' + e.payload.timeStamp + '</li>');
                break;

            case 'error':
                //$("#app-status-ul").append('<li>ERROR -> MSG:' + e.msg + '</li>');
                break;

            default:
                //$("#app-status-ul").append('<li>EVENT -> Unknown, an event was received and we do not know what it is</li>');
                break;
        }
    };

    var bindEvents = function () {

        if (!window.Cordova) {
            $(document).ready(function () {
                readyFunction();

            });
        }
        document.addEventListener('deviceready', onDeviceReady, false);

        document.addEventListener("backbutton", function () {

            var name = localStorage.getItem("Name");
            var room = localStorage.getItem("room");
            signal.leaveRoom(room, name);

        }, false);

        document.addEventListener("pause", pauseapp, false);

        document.addEventListener("resume", resumeapp, false);

    };
    var pauseapp = function () {
        window.background = true;
    }
    var resumeapp = function () {
        window.background = false;
    };
    var onDeviceReady = function () {                             // called when Cordova is ready
        if (window.Cordova && navigator.splashscreen) {

            readyFunction();
        }
    };
    var readyFunction = function () {


        window.background = false;

        signal.initiateConnection();

        $.ui.setSideMenuWidth('210px');

        $.ui.launch();

        var room = localStorage.getItem("room");
        var name = localStorage.getItem("Name");

        $(".HeadName").text(room);

        $('#Back').on("click", function () {
            signal.leaveRoom(room, name);
        });

        var $body = $('body');
        var scaleSource = $body.width();
        $("#Send").on("click", function () {
            signal.SendMessage();
        });

        window.activeUser = "";

        $("#userList").delegate("li a", "click", function (e) {

            var username = $(this).attr("id");
            custom.userClick(username);
        });

        window.scroller[room] = $(".MainComments").scroller({
            lockBounce: false
        });
    }

    initialize();
});


