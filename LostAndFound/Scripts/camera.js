$(document).ready(function () {
    bs_input_file()
    $("#savefind").click(function(){
        saveImage();
    });
    $('#video').resize(function () {
        $('#cameraContainer').height($('#video').height());
        $('#cameraContainer').width($('#video').width());
        //$('#control').height($('#video').height() * 0.1);
        //$('#control').css('top', $('#video').height() * 0.9);
        //$('#control').width($('#video').width());
        //$('#control').show();
    });
    function opencam() {
        $('#cameraContainer').removeAttr("hidden");
        $('#uploadedImage').attr("hidden", "hidden");
        navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.oGetUserMedia || navigator.msGetUserMedia;
        if (navigator.getUserMedia) {
            navigator.getUserMedia({ video: true }, streamWebCam, throwError);
        }
    }

    function closecam() {

        video.pause();

        try {
            video.srcObject = null;
        } catch (error) {
            video.src = null;
        }
        debugger;
        var track = strr.getTracks()[0];  // if only one media track
        // ...
        track.stop();

    }
    var video = document.getElementById('video');
    var canvas = document.getElementById('canvas');
    var context = canvas.getContext('2d');
    var strr;
    function streamWebCam(stream) {
        const mediaSource = new MediaSource(stream);
        //  video.src = URL.createObjectURL(stream);
        webcamStream = stream;
        try {
            video.srcObject = stream;
        } catch (error) {
            video.src = URL.createObjectURL(mediaSource);
        }
        video.play();
        strr = stream;
    }
    function throwError(e) {
        alert(e.name);
    }
    function saveImage() {
        // Draws current image from the video element into the canvas
        // context.drawImage(video, 0, 0, canvas.width, canvas.height);
        //   webcamStream.stop();
        var dataURL = canvas.toDataURL('image/jpeg', 1.0);
        //  document.querySelector('#dl-btn').href = dataURL;
        var formdata = new FormData($("#findform")[0]);
        coverObject = saveCoverInformation();
        formdata.append('coverInfo', coverObject);
        formdata.append('imagedata', dataURL);
        $.ajax({
            type: "POST",
            contentType: false,
            cache: false,
            processData: false,
            async: false,
            url: "/Create/CreateFind2",
            data: { formdata }
        }).done(function (o) {
            console.log('saved');
            // If you want the file to be visible in the browser 
            // - please modify the callback in javascript. All you
            // need is to return the url to the file, you just saved 
            // and than put the image in your browser.
        });
    }
    $('#retake').click(function (event) {
        opencam();
    });

    $('#openCamera').click(function (event) {
        opencam();
      
    });

    $('#snap').click(function (event) {
        canvas.width = video.clientWidth;
        canvas.height = video.clientHeight;
        context.drawImage(video, 0, 0);
       // context.scale(-1, 1);
        $('#vid').css('z-index', '20');
        $('#capture').css('z-index', '30');
        //context.translate(canvas.width, 0);
        //context.scale(-1, 1);
        $(canvas).css('transform', 'scalex(-1)');
    });
  });
//display image from the PC
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#uploadedImage')
                .attr('src', e.target.result);
               
        };
        $('#cameraContainer').width($("#displayPicture").width());
        $('#uploadedImage').removeAttr("hidden");
        $('#cameraContainer').attr("hidden", "hidden");
        //  $('#uploadedImage').css("hidden", "visible");
        $('#cameraContainer').append("<div id='ketem'></div>");
        reader.readAsDataURL(input.files[0]);

    }
}
function bs_input_file() {
    $(".input-file").before(
        function () {
            if (!$(this).prev().hasClass('input-ghost')) {
                var element = $("<input type='file' class='input-ghost' style='visibility:hidden; height:0' accept='image / gif, image / jpeg, image / png'>");
                element.attr("name", "importFile");
                element.change(function () {
                    readURL(this);
                    //var file = this.files[0];
                    //element.next(element).find('.selectfile').val(file.name.split('\\').pop());
                });
                $(this).find("#uploadImage_btn").click(function () {
                   
                    element.click();
                });
                $(this).find("#uploadImage_btn").css("cursor", "pointer");
                      return element;
            }
        }
    );
}
