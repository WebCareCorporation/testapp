
define(['require', 'CustomFunctions'],
    function (require, custom) {
        //    return signalr;

        if (window.Cordova) {
            $.connection.hub.url = "http://bathindavarinder-001-site1.smarterasp.net/signalr";
        }

        window.game = $.connection.gameHub;

        var tryingToReconnect = false;





        window.game.client.StartGame = function (cards) {

        }






         

        $.connection.hub.reconnecting(function () {
            
        });

        $.connection.hub.connectionSlow(function () {
           

        });

        $.connection.hub.reconnected(function () {
           

        });

        $.connection.hub.disconnected(function () {

            //if (!window.background) {
            //    if (localStorage.getItem("room")) {
            //        $.connection.hub.start().done(function () {

            //            var myClientId = $.connection.hub.id;
            //            var yourname = localStorage.getItem("Name");
            //            if (myClientId != localStorage.getItem("ConnId")) {
            //               // window.chat.server.updateConnId(localStorage.getItem("ConnId"), myClientId, yourname);
            //            }
            //            var myClientId = $.connection.hub.id;

            //            localStorage.setItem("ConnId", myClientId);
            //            //window.chat.server.updateName(myClientId, $('#displayname').val());

            //            var name = localStorage.getItem("Name");

            //            var room = localStorage.getItem("room");


            //            JoinRoom(room, name);


            //        });
            //    }

            //} else {
            //    custom.showNotification("Timeout", "You have been pulled out of room because of no activity");
            //    localStorage.setItem("room", undefined);
            //    custom.openRooms();
            //}

        });


         
        var JoinRoom = function (groupname, name) {
           // window.chat.server.joinRoom(groupname, name);
        };

        function successHandler(result) {

        }
        function errorHandler(error) {
            //alert('error = ' + error);
        }

        window.onNotificationGCM = function (e) {
            //$("#app-status-ul").append('<li>EVENT -> RECEIVED:' + e.event + '</li>');

            console.log("event fired : " + e.event);
            switch (e.event) {
                case 'registered':
                    if (e.regid.length > 0) {
                        //$("#app-status-ul").append('<li>REGISTERED -> REGID:' + e.regid + "</li>");
                        // Your GCM push server needs to know the regID before it can push to this device
                        // here is where you might want to send it the regID for later use.
                        console.log("regID = " + e.regid);
                        //alert('resgisterd' + e.regid);
                        localStorage.setItem("FirstTime", "false");
                        var name = localStorage.getItem("Name");
                        SendGCMID(name, e.regid);
                    }
                    break;

                case 'message':
                    // if this flag is set, this notification happened while we were in the foreground.
                    // you might want to play a sound to get the user's attention, throw up a dialog, etc.
                    if (e.foreground) {
                        var message = e.payload.message;

                        var n = message.indexOf(":");

                        var name = message.substring(0, n);
                        console.log("got name : " + name);
                        custom.showNotification(name, e.payload.message);
                        //$("#app-status-ul").append('<li>--INLINE NOTIFICATION--' + '</li>');

                        //// on Android soundname is outside the payload.
                        //// On Amazon FireOS all custom attributes are contained within payload
                        //var soundfile = e.soundname || e.payload.sound;
                        //// if the notification contains a soundname, play it.
                        //var my_media = new Media("/android_asset/www/" + soundfile);
                        //my_media.play();
                    }
                    else {
                        var message = e.payload.message;

                        var n = message.indexOf(":");

                        var name = message.substring(0, n);
                        console.log("got name : " + name);
                        custom.showNotification(name, e.payload.message);


                        // otherwise we were launched because the user touched a notification in the notification tray.
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


 

        var SendGCMID = function (name, GCMId) {
           
        };

        //var signalr = {
        return {
            // Application Constructor

            SendGCMID: SendGCMID,
            initialize: function () {

                document.addEventListener("offline", this.onOffline, false);

            }, JoinRoom: JoinRoom,
            initiateConnection: function () {


                if (custom.CheckConnection()) {



                    $.connection.hub.start().done(function () {
                        var myClientId = $.connection.hub.id;
                        localStorage.setItem("ConnId", myClientId);
                        //chat.server.updateName(myClientId, $('#displayname').val());
                        var name = localStorage.getItem("Name");
                        var room = localStorage.getItem("room");


                        if (!localStorage.getItem("FirstTime")) {
                            if (window.Cordova) {

                                pushNotification = window.plugins.pushNotification;
                                pushNotification.register(
                                        successHandler,
                                        errorHandler,
                                        {
                                            "senderID": "899559090645",
                                            "ecb": "onNotificationGCM"
                                        });
                            }
                        }


                       // JoinRoom(room, name);
                       
                    });

                }  
            },
            startConnection: function () {

                if (custom.CheckConnection()) {

                    $.connection.hub.start().done(function () {

                        //var name = localStorage.getItem("tempName");

                        //var uniqueId = localStorage.getItem("uniqueId");
                        window.game.server.sendOfflineMessage("me", "my", "self");
                   //     window.chat.server.registerUser(uniqueId, name);

                    });

                } else {
                    alert("please check your network.");
                }
            }
        };
    });

