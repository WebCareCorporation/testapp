//var room = {
define(['require', 'GameHub', 'CustomFunctions'],
function (require, signal, custom) {

    // Application Constructor
    var initialize = function () {

        if (custom.CheckConnection()) {

            custom.show('loading', true);

            //  custom.show('afui', false);


            var width = window.innerWidth;
            var height = window.innerHeight;

            var pixelRatio = window.devicePixelRatio || 1;

            width = window.innerWidth * pixelRatio;
            height = window.innerHeight * pixelRatio;

            //$('.main-content').css('height', height);


            //$('.content').css('height', 19 * (height / 20));

            $('#outer-dropzone').css('height', 3 * (height / 10));
            $('#MainComments').css('height', height);


            ////   $('#outer-dropzone').css('width', width);

            //   //var size = width + "px " + 4 * (height / 10) + "px";
            //   //$('#outer-dropzone').css('background-size', size);


            $('#Message').css('min-height', 4 * (height / 10));
            $('#Message').css('max-height', 5 * (height / 10));
            $('#Message').css('width', 6 * (width / 10));
            bindEvents();
        } else {
            alert("Please check your network connection !");
            window.location = "index.html";
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

            if (messageopen) {
                window.Scroller.unlock();
                window.MessageScroller.lock();
                $("#Message").slideUp(1);
                messageopen = false;
                $("#cardzone").show();
                $('.msglink li').removeClass("backColorOrange");
                return;
            }


            if (window.confirm("Want to leave game ?")) {
                window.location = "index.html";
            }

        }, false);

        document.addEventListener("pause", pauseapp, false);

        document.addEventListener("resume", resumeapp, false);

        menuDiv = document.querySelector("#menu");



    };
    var pauseapp = function () {
        window.background = true;
    }
    var resumeapp = function () {
        window.background = false;
    };
    var onDeviceReady = function () {                             // called when Cordova is ready
        if (window.Cordova && navigator.splashscreen) {
            //  FastClick.attach($('#RoomChatWindow'));

            readyFunction();
        }
    };
    var menuOpen = false;
    var menuDiv = "";

    var messageopen = false;
    var readyFunction = function () {

        FastClick.attach(document.body);
        window.Scroller = $(".MainComments").scroller({
            lockBounce: false
        });
        window.MessageScroller = $(".Message").scroller({
            lockBounce: false
        });

        $('.msglink li').click(
            function () {

                $('.animatedCard').removeClass('animatedCard');

                if (!messageopen) {
                    if ($("#Message li").length) {
                        window.Scroller.scrollToTop(1);
                        window.Scroller.lock();
                        window.MessageScroller.unlock();
                        $("#cardzone").hide();
                        $("#Message").slideDown(1);
                        $('#Message').scrollTop($('#Message li').last().position().top + 40);
                        messageopen = true;

                    }

                } else {
                    window.Scroller.unlock();
                    window.MessageScroller.lock();
                    $("#Message").slideUp(1);
                    messageopen = false;

                    $("#cardzone").show();

                    // $("#Message").empty();
                }
                $('.msglink li').removeClass("backColorOrange");

            });

        $.ui.setSideMenuWidth('210px');



        signal.startConnection();

        $("#Send").on("click", function () {
            $('#HomeMessage').blur();
            signal.SendMessage();
        });


        window.background = false;

        $(document).delegate('.mine', 'click', function () {


            var that = $(this).find('.squaredThreechk');

            var checked = $(that).attr("checked");
            $('.squaredThreechk').attr('checked', false); // Checks it
            if (!checked) {
                $(that).prop('checked', true);
                $(that).attr("checked", true);
                $('#messageTable').hide();
                $('#ThrowTable').show();


                $(that).closest('.select').css({ 'opacity': 1 });
            } else {
                $('#messageTable').show();
                $('#ThrowTable').hide();
                $(that).closest('.select').css({ 'opacity': 0 });
            }

        });


        //$('#HomeMessage').on("touchstart", function () {
        //    $('#HomeMessage').trigger('click');
        //    $('#HomeMessage').trigger('click');
        //    $('#HomeMessage').trigger('click');
        //    $('#HomeMessage').trigger('click');
        //    $('#HomeMessage').trigger('click');

        //    $('#HomeMessage').focus();
        //});

        $(document).delegate(".squaredThreechk", "click", function (event) {

            var checked = $(this).attr("checked");
            $('.squaredThreechk').attr('checked', false); // Checks it
            if (!checked) {
                $(this).prop('checked', true);
                $(this).attr("checked", true);
                $('#messageTable').hide();
                $('#ThrowTable').show();


                $(this).closest('.select').css({ 'opacity': 1 });
            } else {
                $('#messageTable').show();
                $('#ThrowTable').hide();
                $(this).closest('.select').css({ 'opacity': 0 });
            }

            event.stopPropagation();

        });


        $('#throwbtn').on('click', function () {

            var card = $("input[type='checkbox']:checked").closest('.card');
            var cardid = $("input[type='checkbox']:checked").closest('.card').attr('id')
            if ($(card).hasClass('dropcan')) {
                if (window.confirm("Are you sure?")) {
                    var UserName = localStorage.getItem("Name");
                    var Group = localStorage.getItem("Group");

                    $(card).removeClass('active');
                    var hasCard = "true";
                    if (!$('.card .active').length) {
                        hasCard = "false"
                    }
                    if (localStorage.getItem("cardType") == "") {
                        localStorage.setItem("cardType", $(card).attr("data-card"))
                    }
                    $('.dropcan').removeClass('dropcan');
                    //event.relatedTarget.classList.add("thrownCard");


                    if (!window.reconnecting) {
                        window.game.server.throwCard(cardid, UserName, Group, hasCard, localStorage.getItem("cardType"));
                    } else {
                        localStorage.setItem("SendCard", cardid + "$" + UserName + "$" + Group + "$" + hasCard + "$" + localStorage.getItem("cardType"));
                    }
                    $('#messageTable').show();
                    $('#ThrowTable').hide();
                }
            } else {
                if ($('.dropcan').length) {
                    alert("You can't throw this card !");
                } else {
                    alert("Its not your turn !");
                }
            }


        });



    }






    initialize();
});


