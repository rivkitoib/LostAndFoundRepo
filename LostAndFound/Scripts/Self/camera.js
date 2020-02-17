$(document).ready(function () {
    bs_input_file()
    $("#saveFind").click(function () {
        console.log("save find");
        saveForm();
    });
    $('#video').resize(function () {
        //$('#video').addClass(".imagePlace");
        //$('#cameraContainer').height($('#video').height());
        //$('#cameraContainer').width($('#video').width());
        //$('#control').height($('#video').height() * 0.1);
        //$('#control').css('top', $('#video').height() * 0.9);
        //$('#control').width($('#video').width());
        //$('#control').show();
    });
    function opencam() {
        $(video).removeAttr("hidden");
        $(video).addClass('imagePlace');
        $('#cameraContainer').removeAttr("hidden");
        $('#canvas').attr("hidden", "hidden");
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
        var track = strr.getTracks()[0];  // if only one media track
        // ...
        track.stop();

    }
    var video = document.getElementById('video');
    var canvas = document.getElementById('canvas');
    var context = canvas.getContext('2d');
    var camera;
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
        camera = stream;
    }
    function throwError(e) {
        alert(e.name);
    }
    function saveForm() {
        // Draws current image from the video element into the canvas
        // context.drawImage(video, 0, 0, canvas.width, canvas.height);
        //   webcamStream.stop();
        var dataURL = canvas.toDataURL('image/jpeg', 1.0);
        //  document.querySelector('#dl-btn').href = dataURL;
        var formdata = new FormData($("#findform1")[0]);
        coverObject = saveCoverInformation();
        var ci = coverObject.height + ';' + coverObject.width + ';' + coverObject.fromX + ';' + coverObject.fromy;
        var sizes = getImageSize();
        si = sizes.height + ';' + sizes.width;
        formdata.append('coverInfo', ci);
        formdata.append('sizes', si);
        formdata.append('imagedata', dataURL);

        $.ajax({
            type: "POST",
            contentType: false,
            cache: false,
            processData: false,
            async: false,
            url: "/Create/CreateFind2",
            data: formdata,
            success: function (data) {
                alert('success')
            }
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
        $(canvas).removeAttr("hidden");
        $(canvas).removeClass('imagePlace');
        $(video).removeClass('imagePlace');
        canvas.width = video.clientWidth;
        canvas.height = video.clientHeight;
        context.drawImage(video, 0, 0);
        $('#vid').css('z-index', '10');
        $('#canvas').css('z-index', '20');
        $(video).attr("hidden", "hidden");
        //context.translate(canvas.width, 0);
        //context.scale(-1, 1);
        $(canvas).css('transform', 'scaleX(-1)');
        $(canvas).addClass('imagePlace');

    });
    cover = document.getElementById("dragableDiv");
    uploadImage = document.getElementById("uploadedImage");
    captureImage = document.getElementById("canvas");
    container = document.getElementById("container3");
    function saveCoverInformation() {
        var info = new Object();
        if (!uploadImage.hidden) {
            info["fromX"] = cover.offsetLeft - uploadImage.offsetLeft;
            if (info["fromX"] < 0) {
                info["fromX"] = 0;
            }
            info["fromy"] = cover.offsetTop - uploadImage.offsetTop;
            if (info["fromy"] < 0) {
                info["fromy"] = 0;
            }
        }
        else {
            info["fromX"] = container.offsetLeft + uploadImage.offsetLeft - cover.offsetLeft;
            info["fromy"] = container.offsetTop + uploadImage.offsetTop - cover.offsetTop;
        }
        info["height"] = cover.offsetHeight;
        if (info["fromy"] + info["height"] > uploadImage.offsetHeight)
            info["height"] = uploadImage.offsetHeight - info["fromy"];
        info["width"] = cover.offsetWidth;
        if (info["fromX"] + info["width"] > uploadImage.offsetWidth)
            info["width"] = uploadImheightage.offsetHeight - info["fromX"];
        info["color"] = $(cover).css("backgroundColor");
        return info;
    };
    function getImageSize() {
        var sizes = new Object();
        if (!uploadImage.hidden) {
            sizes["height"] = uploadImage.clientHeight;
            sizes["width"] = uploadImage.clientWidth;
        } else {
            sizes["height"] = captureImage.clientHeight;
            sizes["width"] = captureImage.clientWidth;
        }
        return sizes;
    }
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
        $('#canvas').attr("hidden", "hidden");
        $('#cameraContainer').attr("hidden", "hidden");
        $('#cameraContainer').append("<div id='ketem'></div>");
        reader.readAsDataURL(input.files[0]);

    }
}
function bs_input_file() {
    $("#inputFile").before(
        function () {

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

    );
}
