
var canvas;
var tools;

var x = 0;

var imageX = 400
var imageY = 300

function animate() {
    let size = canvas.width;

    tools.clearRect(0,0, canvas.width, canvas.height);
    for (var i = 0; i < imageCount; i++){
        tools.drawImage(imageList[i], i * imageX - x, 0, imageX, imageY);
    }
    if (x < (imageCount * size / 4 - size)){
        x += 0.5;
    }else{
        x = 0;
    }
    requestAnimationFrame(animate);
}


var imageCount = 8;
var imageList = new Array()


window.onload = function() {
    canvas = document.getElementById("moveimage");
    tools = canvas.getContext("2d");

    $(document).ready(function(){
        $("#div1").load("./base/head.html");
        $("#div2").load("./base/foot.html");
    });

    for (var i = 0; i < imageCount; i++){
        imageList[i] = new Image(imageX, imageY);
        imageList[i].src = "./main/" + (i + 1) + ".jpg";
    }
    animate();
}