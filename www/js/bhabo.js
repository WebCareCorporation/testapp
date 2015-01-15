//var room = {
define(['require'],
function (require) {

    // Application Constructor
    var initialize = function () {

        var height = $(window).height();
        $('body').css('height', height);

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

        interact('.draggable')
.draggable({
    // enable inertial throwing
    inertia: true,
    // keep the element within the area of it's parent
    restrict: {
        restriction: "parent",
        endOnly: true,
        elementRect: { top: 0, left: 0, bottom: 1, right: 1 }
    },

    // call this function on every dragmove event
    onmove: function (event) {
        var target = event.target,
            // keep the dragged position in the data-x/data-y attributes
            x = (parseFloat(target.getAttribute('data-x')) || 0) + event.dx,
            y = (parseFloat(target.getAttribute('data-y')) || 0) + event.dy;

        // translate the element
        target.style.webkitTransform =
        target.style.transform =
          'translate(' + x + 'px, ' + y + 'px)';

        // update the posiion attributes
        target.setAttribute('data-x', x);
        target.setAttribute('data-y', y);
    },
    // call this function on every dragend event
    onend: function (event) {
        var textEl = event.target.querySelector('p');

        textEl && (textEl.textContent =
          'moved a distance of '
          + (Math.sqrt(event.dx * event.dx +
                       event.dy * event.dy) | 0) + 'px');
    }
});


        $(".draggable").hover(
      function () {
          $(this).find(".select").css({ 'opacity': 1 });
      }, function () {
          if (!$(this).find('.checkbox').is(':checked')) {
              $(this).find(".select").css({ 'opacity': 0 });
          }
      }
    );
        $('.checkbox').on('click', function () { 
            if($(this).prop('checked'))
            {
                $('.checkbox:checked').prop('checked', false);
                $(this).prop('checked', true);
            }

        });

        //$(".draggable").click(function () {


        //    if (!$(this).find('.checkbox').prop('checked')) {
        //        $('.checkbox:checked').prop('checked', false);
        //        $(this).find('.checkbox').prop('checked', true);
        //    } else {
        //        $(this).find('.checkbox').prop('checked', false);
        //    }

        //});


    }






    initialize();
});


