

window.onload = function() {
    $(document).ready(function(){
        $("#div1").load("./base/head.html");
        $("#div2").load("./base/foot.html");
    });
}


function searchList(){
    let option = document.getElementById("#searchOption option:selected")
    alert(option);
}