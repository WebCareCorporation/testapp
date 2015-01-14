//var room = {
define(['require', 'CustomFunctions', 'signalRHub', 'Database'],
function (require, custom, signal, database) {

    // Application Constructor
    var initialize = function () {

        custom.show('loading', true);

        custom.show('afui', false);

        custom.initialize();

        signal.initialize();

        database.init();

        //$.ui.autoLaunch = false;

        //$.ui.backButtonText = "";

        bindEvents();
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

        //$.ui.launch();

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


