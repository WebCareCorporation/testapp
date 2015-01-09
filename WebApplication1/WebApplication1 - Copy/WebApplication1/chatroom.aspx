<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chatroom.aspx.cs" Inherits="WebApplication1.chatroom" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="http://code.jquery.com/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.signalR-2.1.2.min.js" type="text/javascript"></script>
    <script src="signalr/hubs" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            // Proxy created on the fly          
            var chat = $.connection.chatHub;
            
                
                $.connection.hub.start().done(function () {
                    
                    var myClientId = $.connection.hub.id;
                    
                    chat.server.updateName(myClientId, $('#displayname').val());
                });

            $.connection.hub.logging = true;
            // Get the user name and store it to prepend to messages.
            if (localStorage.getItem("Name") == undefined) {
                $('#displayname').val(prompt('Enter your name:', ''));
                localStorage.setItem("Name", $('#displayname').val());
            } else {
                $('#displayname').val(localStorage.getItem("Name"));
            }


            // Declare a function on the chat hub so the server can invoke it          
            chat.client.sendMessage = function (name, message) {
                var encodedName = $('<div />').text(name).html();
                var encodedMsg = $('<div />').text(message).html();
                $('#messages').append('<li>' + encodedName +
                    ':  ' + encodedMsg + '</li>');
            };

            chat.client.updateList = function (names) {
                var users = names.split(",");
                $('#users').empty();
                $.each(users, function (index, chunk) {
                    $('#users').append('<li>' + index + " : " + chunk + '</li>');
                });
            };
            // Start the connection
            $.connection.hub.start().done(function () {
                $("#send").click(function () {
                    // Call the chat method on the server
                    chat.server.send($('#displayname').val(), $('#to').val(), $('#msg').val());
                    $('#msg').val()
                });
            });


            $.connection.hub.connectionSlow(function () {
                alert("week network");
            });


            var tryingToReconnect = false;

            $.connection.hub.reconnecting(function () {
                tryingToReconnect = true;
            });

            $.connection.hub.reconnected(function () {
                tryingToReconnect = false;
            });

            $.connection.hub.disconnected(function () {

                chat.server.RemoveUser($('#displayname').val());
              
                    setTimeout(function () {
                        $.connection.hub.start();
                    }, 5000);
               
            });

        });
    </script>
    <title></title>
</head>
<body>
    <div>
       Message : <input type="text" id="msg" /> <br />
        To : <input type="text" id="to" />
        <ul id="users">
        </ul>
        <input type="button" id="send" value="Send" />
        <input type="button" id="displayname" />
        <ul id="messages">
        </ul>
    </div>
</body>
</html>
