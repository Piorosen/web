

window.onload = function() {
    $(document).ready(function(){
        $("#div1").load("./base/head.html");
        $("#div2").load("./base/foot.html");
    });
}

function submitData(){
    alert("성공적으로 전달 하였습니다.");
}