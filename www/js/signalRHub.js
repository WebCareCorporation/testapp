
define(['require', 'CustomFunctions'],
    function (require, custom) {
        //    return signalr;

        var JoinRoom = function (groupname, name) {
            chat.server.joinRoom(groupname, name);
        };

        function successHandler(result) {

        }
        function errorHandler(error) {
            //alert('error = ' + error);
        }

        window.onNotificationGCM = function (e) {
            //$("#app-status-ul").append('<li>EVENT -> RECEIVED:' + e.event + '</li>');
            alert("received");
            alert(e.event);
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
                        custom.showNotification("Gapshap", e.payload.message);
                        //$("#app-status-ul").append('<li>--INLINE NOTIFICATION--' + '</li>');

                        //// on Android soundname is outside the payload.
                        //// On Amazon FireOS all custom attributes are contained within payload
                        //var soundfile = e.soundname || e.payload.sound;
                        //// if the notification contains a soundname, play it.
                        //var my_media = new Media("/android_asset/www/" + soundfile);
                        //my_media.play();
                    }
                    else {
                        custom.showNotification("Gapshap", e.payload.message);
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

            var encodedMsg = $('<div />').text(message).html();

            var msg = $('<li>' + by + ' : ' + encodedMsg + '</li>');

            $('div#' + by + ' .ChatWindow').append(msg);

            custom.scrollOnMessage(by);

        };

        var SendGCMID = function (name, GCMId) {
            chat.server.updateUserGCMID(name, GCMId);
        };

        //var signalr = {
        return {
            // Application Constructor
            chat: undefined,
            SendGCMID: SendGCMID,
            initialize: function () {

                document.addEventListener("offline", this.onOffline, false);

                if (window.Cordova) {
                    $.connection.hub.url = "http://bathindavarinder-001-site1.smarterasp.net/signalr";
                }

                chat = $.connection.chatHub;

                var tryingToReconnect = false;

                chat.client.updateMembers = function (names) {

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

                chat.client.confirmJoin = function (name) {

                    var encodedMsg = $('<div />').text(name + " Joined").html();

                    var yourname = localStorage.getItem("Name");

                    var msg = $('<li>' + encodedMsg + '</li>');

                    custom.informMessage(msg, yourname, false);

                    if ($('#userList #' + name).length == 0) {
                        if (yourname != name)
                            $("#userList").append('<li><a href="#" class="icon chat ui-link" id="' + name + '">' + name + '</li>');
                    }
                };
                chat.client.registerConfirm = function (result) {
                    if (result == "true") {

                        localStorage.setItem("Name", localStorage.getItem("tempName"));

                        $.ui.loadContent("main", null, null, "fade");

                        setTimeout(window.location = "rooms.html", 5000);
                    }
                    else {
                        $("#Info").html("This username is already taken. Please use other name.");
                    }
                }

                chat.client.leftRoom = function (name) {
                    $('#userList #' + name).parent().remove();
                    Message(name + " Left.", name, true);
                };

                chat.client.confirmLeft = function () {
                    custom.openRooms();
                };

                $.connection.hub.reconnecting(function () {
                    var msg = $('<li> Reconnecting.... </li>');
                    custom.informMessage(msg, "Gapshap", true);
                    tryingToReconnect = true;
                });

                $.connection.hub.connectionSlow(function () {
                    var msg = $('<li> Connection slow.... </li>');
                    custom.informMessage(msg, "Gapshap", true);

                });

                $.connection.hub.reconnected(function () {
                    tryingToReconnect = false;
                    var myClientId = $.connection.hub.id;
                    var msg = $('<li> Reconnected.... </li>');
                    custom.informMessage(msg, "Gapshap", true);
                    if (myClientId != localStorage.getItem("ConnId")) {

                        var msg = $('<li> updating connection.... </li>');

                        custom.informMessage("updating connection....", "Gapshap", true);

                        var yourname = localStorage.getItem("Name");
                        chat.server.updateConnId(localStorage.getItem("ConnId"), myClientId, yourname);
                        localStorage.setItem("ConnId", myClientId);
                    }

                });

                $.connection.hub.disconnected(function () {

                    if (!window.background) {

                        $.connection.hub.start().done(function () {

                            var myClientId = $.connection.hub.id;

                            if (myClientId != localStorage.getItem("ConnId")) {
                                chat.server.updateConnId(localStorage.getItem("ConnId"), myClientId, yourname);
                            }
                            var myClientId = $.connection.hub.id;

                            localStorage.setItem("ConnId", myClientId);
                            //chat.server.updateName(myClientId, $('#displayname').val());

                            var name = localStorage.getItem("Name");

                            var room = localStorage.getItem("room");


                            JoinRoom(room, name);


                        });

                    } else {
                        custom.showNotification("Timeout", "You have been pulled out of room because of no activity");
                        custom.openRooms();
                    }

                });



                chat.client.addChatMessage = function (message) {

                    var n = message.indexOf(":");

                    var name = message.substring(0, n);

                    var encodedMsg = $('<div />').text(message).html();


                    var msg = $('<li>' + encodedMsg + '</li>');
                    custom.informMessage(msg, "Gapshap", false);

                    if (window.background) {
                        custom.showNotification(name, encodedMsg);
                    }

                };
                // Personal Message from some one.
                chat.client.recievePersonalChat = function (message, by) {

                    signalr.Message(message, by, false);
                    if (window.background) {
                        custom.showNotification(by, message);
                    }
                }


                // Add msg sent by me to personal chat window 
                chat.client.byPersonalChat = function (message, by) {

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

                    var encodedMsg = $('<div />').text(message).html();

                    var msg = $('<li>' + yourname + ' : ' + encodedMsg + '</li>');

                    $('div#' + by + ' .ChatWindow').append(msg);

                    msg.focus();

                    if (window.background) {
                        custom.showNotification(by, message);
                    }

                    custom.scrollOnMessage(by);
                }

            }, JoinRoom: JoinRoom,
            initiateConnection: function () {


                if (custom.CheckConnection()) {

                    custom.show('afui', false);

                    custom.show('loading', true);

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

                        chat.server.registerUser(uniqueId, name);

                    });

                } else {
                    alert("please check your network.");
                }
            },
            leaveRoom: function (groupname, name) {

                var myClientId = localStorage.getItem("ConnId");

                chat.server.leaveRoom(groupname, name, myClientId);

                if (tryingToReconnect) {
                    custom.openRooms();
                }
            },


            SendGroupMessage: function (grpName, name, message) {
                chat.server.sendGroupMessage(grpName, name, message);
            },
            onOffline: function () {
                alert("please check your network.")
                custom.openRooms();
            },
            Message: Message,
            // Send personal message
            SendPersonalMessage: function (name, message, by) {
                chat.server.sendPersonalMessage(name, message, by);
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

