define(function () {

    var buildChatWindow = function (id) {

        var source = $("#personal-template").html();

        var template = Handlebars.compile(source);

        var context = { name: id }

        var html = template(context);

        var parentDiv = $("<div  class='panel chwin' style='display:none;height: 85%;position:static' id='" + id + "'></div>");

        parentDiv.html(html);

        return parentDiv;
    };

    var scrollOnMessage = function (by) {

        var sum = 0;
        var indi = 0;
        $("#" + by + " .ChatWindow li").each(function () {
            sum += $(this).height();
            if (indi == 0)
                indi = sum;
        });

        var windowheight = $("div#" + by).parent().height();

        if (windowheight < sum) {
            window.scroller[by].scrollToBottom(2);
        }

    };
    var showNotification = function (title, msg) {
        window.plugin.notification.local.add({ message: msg, title: title, autoCancel: true })
    };

    var buildMsg = function (name, msg) {

        var encodedMsg = '<div style="display: block;">' + msg + '</div>';
        
        var msg = $('<li><b>' + name + "</b><br>" + encodedMsg + '</li><br>');

        return msg;
    };
    return {

        initialize: function () {

            window.scroller = [];
        },
        showNotification: showNotification,
        buildChatWindow: buildChatWindow,
        buildMsg: buildMsg,
        informMessage: function (msg, name, addToHeader) {

            if (window.background) {
                showNotification(name, msg);
            }

            $("#ChatWindow").append(msg);

            if (window.activeUser == "") {
                var room = localStorage.getItem("room");

                var sum = 0;
                var indi = 0;
                $("#MainComments .ChatWindow li").each(function () {
                    sum += $(this).height();
                    if (indi == 0)
                        indi = sum;
                });
                var windowheight = $("#MainComments").parent().height();

                if (windowheight < sum) {
                    window.scroller[room].scrollToBottom(2);
                }
            }
            else {
                scrollOnMessage(window.activeUser);
            }

            if (addToHeader) {

                //var old = $("#HeadName").text();

                //$("#HeadName").text(msg);

                //setTimeout(function () {
                //    // Do something after 5 seconds
                //    $("#HeadName").text(old)
                //}, 5000);

            }
        },



        userClick: function (clickId) {

            var username = clickId;// $(this).attr("id");

            var room = localStorage.getItem("room");

            var name = localStorage.getItem("Name");

            if (username == name || username == "Back") {
                return;
            }

            if (username == "Home") {

                $.each($('.chwin'), function (name) {
                    $(this).css("display", "none");
                });
                $("#HeadName").text(room);
                $('#RoomChatWindow').css("display", "block");
                window.activeUser = "";
                $.ui.toggleSideMenu();
                return;
            }

            if ($('div#' + username).length == 0) {

                var parentDiv = buildChatWindow(username);
                $('#content').append(parentDiv);

                window.scroller[username] = $("#" + username + " .MainComments").scroller({
                    lockBounce: false
                });
            }


            $.each($('.chwin'), function (name) {
                $(this).css("display", "none");
            });

            $('#RoomChatWindow').css("display", "none");

            window.activeUser = username;
            $("#HeadName").text(username);
            $('div#' + username).css("display", "block");
            var back = $('#userList #Home').css("background-color");

            $('#userList #' + username).parent().css("background-color", back);
            $('#userList #' + username).css("background-color", back);
            $.ui.toggleSideMenu();
        },
        CheckConnection: function () {
            if (window.Cordova && navigator.splashscreen) {

                if (!navigator.network) {
                    // set the parent windows navigator network object to the child window
                    navigator.network = window.top.navigator.network;
                }

                // return the type of connection found
                return ((navigator.network.connection.type === "none" || navigator.network.connection.type === null ||
                       navigator.network.connection.type === "unknown") ? false : true);
            }
            else {
                return true;
            }
        },
        show: function (id, value) {
            if (value) {
                $("#" + id).fadeIn(400);
            } else {
                $("#" + id).fadeOut(400);
            }
            //document.getElementById(id).style.display = value ? 'block' : 'none';
        },
        openRooms: function () {
            window.location = "rooms.html";
        },
        scrollOnMessage: scrollOnMessage
    };
});

