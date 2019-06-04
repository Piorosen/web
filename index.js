
var canvas;
var tools;

var x = 0;
function animate() {
    let size = canvas.width;
    tools.clearRect(0,0, canvas.width, canvas.height);
    for (var i = 0; i < imageCount; i++){
        tools.drawImage(imageList[i], i * size/4 - x, 0, size/4, 300);
    }
    if (x < (imageCount * size/4 - size)){
        x += 1;
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
        imageList[i] = new Image(400, 300);
        imageList[i].src = "./main/" + (i + 1) + ".jpg";
    }
    animate();
}