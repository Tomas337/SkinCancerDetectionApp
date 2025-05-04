window.startCameraFeed = async (video) => {
    try {
        const mediaStream = await navigator.mediaDevices.getUserMedia({
            video: true,
        });

        video.srcObject = mediaStream;

        await video.play();

        return null;
    } catch (err) {
        return err.message;
    }
};

window.stopCameraFeed = (video) => {
    const stream = video.srcObject;

    if (!stream) {
        return;
    }

    const tracks = stream.getTracks();

    tracks.forEach(function (track) {
        track.stop();
    });

    video.srcObject = null;
};

window.getFrame = (src, dest, width, height, dotNetHelper) => {
    let video = document.getElementById(src);
    let canvas = document.getElementById(dest);
    canvas.getContext('2d').drawImage(video, 0, 0, width, height);

    let dataUrl = canvas.toDataURL("image/jpeg");
    dotNetHelper.invokeMethodAsync('ProcessImage', dataUrl);
}