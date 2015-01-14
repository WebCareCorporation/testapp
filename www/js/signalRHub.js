
define(['require', 'CustomFunctions'],
    function (require, custom) {
        //    return signalr;

        if (window.Cordova) {
            $.connection.hub.url = "http://bathindavarinder-001-site1.smarterasp.net/signalr";
        }

        window.chat = $.connection.chatHub;

        var tryingToReconnect = false;

        window.chat.client.updateMembers = function (names) {

            var users = names.split(",");
            var yourname = localStorage.getItem("Name");
            $.each(users, function (index, name) {
                if (name != "") {
                    if ($('#' + name).length == 0) {
                        if (yourname != name)
                            $("#userList").append('<li><a href="#" class="icon chat ui-link" id="' + name + '">' + name + '</li>');
                    }
                }
            });

        };


        window.chat.client.confirmJoin = function (name) {

            var encodedMsg = $('<div />').text(name + " Joined").html();

            var yourname = localStorage.getItem("Name");

            var msg = $('<li>' + encodedMsg + '</li><br>');

            custom.informMessage(msg, yourname, false);

            if ($('#userList #' + name).length == 0) {
                if (yourname != name)
                    $("#userList").append('<li><a href="#" class="icon chat ui-link" id="' + name + '">' + name + '</li>');
            }
        };
        window.chat.client.registerConfirm = function (result) {
            if (result == "true") {

                localStorage.setItem("Name", localStorage.getItem("tempName"));

                $.ui.loadContent("main", null, null, "fade");

                setTimeout(window.location = "rooms.html", 5000);
            }
            else {
                $(".Info").html("This username is already taken. Please use other name.");
            }
        }

        window.chat.client.leftRoom = function (name) {
            $('#userList #' + name).parent().remove();
            Message(name + " Left.", name, true);
        };

        window.chat.client.confirmLeft = function () {
            custom.openRooms();
        };

        $.connection.hub.reconnecting(function () {
            var msg = $('<li> Reconnecting.... </li><br>');
            custom.informMessage(msg, "Gapshap", true);
            tryingToReconnect = true;
        });

        $.connection.hub.connectionSlow(function () {
            var msg = $('<li> Connection slow.... </li><br>');
            custom.informMessage(msg, "Gapshap", true);

        });

        $.connection.hub.reconnected(function () {
            tryingToReconnect = false;
            var myClientId = $.connection.hub.id;
            var msg = $('<li> Reconnected.... </li><br>');
            custom.informMessage(msg, "Gapshap", true);
            if (myClientId != localStorage.getItem("ConnId")) {

                var msg = $('<li> updating connection.... </li><br>');

                custom.informMessage("updating connection....", "Gapshap", true);

                var yourname = localStorage.getItem("Name");
                window.chat.server.updateConnId(localStorage.getItem("ConnId"), myClientId, yourname);
                localStorage.setItem("ConnId", myClientId);
            }

        });

        $.connection.hub.disconnected(function () {

            if (!window.background) {
                if (localStorage.getItem("room")) {
                    $.connection.hub.start().done(function () {

                        var myClientId = $.connection.hub.id;
                        var yourname = localStorage.getItem("Name");
                        if (myClientId != localStorage.getItem("ConnId")) {
                            window.chat.server.updateConnId(localStorage.getItem("ConnId"), myClientId, yourname);
                        }
                        var myClientId = $.connection.hub.id;

                        localStorage.setItem("ConnId", myClientId);
                        //window.chat.server.updateName(myClientId, $('#displayname').val());

                        var name = localStorage.getItem("Name");

                        var room = localStorage.getItem("room");


                        JoinRoom(room, name);


                    });
                }

            } else {
                custom.showNotification("Timeout", "You have been pulled out of room because of no activity");
                localStorage.setItem("room", undefined);
                custom.openRooms();
            }

        });



        window.chat.client.addChatMessage = function (message) {

            var n = message.indexOf(":");

            var name = message.substring(0, n);

            message = message.substring(n + 2, message.length);


            var msg = custom.buildMsg(name, message);

            custom.informMessage(msg, "Gapshap", false);

            if (window.background) {
                custom.showNotification(name, encodedMsg);
            }

        };
        // Personal Message from some one.
        window.chat.client.recievePersonalChat = function (message, by) {

            Message(message, by, false);
            if (window.background) {
                custom.showNotification(by, message);
            }
        }


        // Add msg sent by me to personal window.chat window 
        window.chat.client.byPersonalChat = function (message, by) {

            var yourname = localStorage.getItem("Name");

            if ($('div#' + by).length == 0) {

                var parentDiv = custom.buildChatWindow(by);

                $('#content').append(parentDiv);

                window.scroller[by] = $("#" + by + " .MainComments").scroller({
                    lockBounce: false
                });
            }

            if (window.activeUser != by)
                $('#userList #' + by).parent().css("background-color", "orange");


            var msg = custom.buildMsg(yourname, message);

            $('div#' + by + ' .ChatWindow').append(msg);

            msg.focus();

            if (window.background) {
                custom.showNotification(by, message);
            }

            custom.scrollOnMessage(by);
        }
        var JoinRoom = function (groupname, name) {
            window.chat.server.joinRoom(groupname, name);
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



        var Message = function (message, by, left) {

            var divExist = true;

            if ($('div#' + by).length == 0) {

                divExist = false;

                if (!left) {
                    var parentDiv = custom.buildChatWindow(by);

                    $('#content').append(parentDiv);

                    window.scroller[by] = $("#" + by + " .MainComments").scroller({
                        lockBounce: false
                    });
                }
            }

            if (left && !divExist) {
                return;
            }

            if (window.activeUser != by)
                $('#userList #' + by).parent().css("background-color", "orange");


            var msg = custom.buildMsg(by, message);

            $('div#' + by + ' .ChatWindow').append(msg);

            custom.scrollOnMessage(by);

        };

        var SendGCMID = function (name, GCMId) {
            window.chat.server.updateUserGCMID(name, GCMId);
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


                        JoinRoom(room, name);
                        custom.show('afui', true);
                        custom.show('loading', false);
                    });

                } else {

                    alert("please check your network.");
                    custom.openRooms();
                }
            },
            startConnection: function () {

                if (custom.CheckConnection()) {

                    $.connection.hub.start().done(function () {

                        var name = localStorage.getItem("tempName");

                        var uniqueId = localStorage.getItem("uniqueId");

                        window.chat.server.registerUser(uniqueId, name);

                    });

                } else {
                    alert("please check your network.");
                }
            },
            leaveRoom: function (groupname, name) {

                var myClientId = localStorage.getItem("ConnId");
                localStorage.setItem("room", undefined);
                window.chat.server.leaveRoom(groupname, name, myClientId);

                if (tryingToReconnect) {
                    custom.openRooms();
                }
            },


            SendGroupMessage: function (grpName, name, message) {
                window.chat.server.sendGroupMessage(grpName, name, message);
            },
            onOffline: function () {
                alert("please check your network.")
                custom.openRooms();
            },
            Message: Message,
            // Send personal message
            SendPersonalMessage: function (name, message, by) {
                window.chat.server.sendPersonalMessage(name, message, by);
            },


            // Send group message
            SendMessage: function () {
                var name = localStorage.getItem("Name");
                var Message = $("#HomeMessage").val();

                if (Message == "") {
                    return;
                }

                var room = localStorage.getItem("room");
                if (window.activeUser == "") {
                    this.SendGroupMessage(room, name, Message);
                } else {
                    this.SendPersonalMessage(window.activeUser, Message, name);
                }

                $("#HomeMessage").val("");
            }
        };
    });

