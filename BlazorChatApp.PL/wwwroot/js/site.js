let video = null;
let canvas = null;
let context = null;
let streaming = false;

let width = 100;    // We will scale the photo width to this.
let height = 0;     // This will be computed based on the input stream

function onStart(options) {
    video = document.getElementById("video");
    canvas = document.getElementById("canvas");
    context = canvas.getContext('2d');
    width = options.width;

    navigator.mediaDevices.getUserMedia({ video: true, audio: false })
        .then(function (stream) {
            video.srcObject = stream;
            video.play();
        })
        .catch(function (err) {
            console.log("An error occurred: " + err);
        });

    video.addEventListener('canplay', function () {
        if (!streaming) {
            height = video.videoHeight / (video.videoWidth / width);

            if (isNaN(height)) {
                height = width / (4 / 3);
            }

            video.setAttribute('width', width);
            video.setAttribute('height', height);
            canvas.setAttribute('width', width);
            canvas.setAttribute('height', height);
            streaming = true;
        }
    }, false);

    video.addEventListener("play", function () {
        console.log('play');
        timercallback();
    }, false);
}

function timercallback() {
    if (video.paused || video.ended) {
        return;
    }
    computeFrame();
    setTimeout(function () {
        timercallback();
    }, 0);
}

function computeFrame() {
    context.drawImage(video, 0, 0, width, height);
}

window.WebCamFunctions = {
    start: (options) => { onStart(options); }
};



