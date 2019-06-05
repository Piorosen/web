

window.onload = function() {
    $(document).ready(function(){
        $("#div1").load("./base/head.html");
        $("#div2").load("./base/foot.html");
    });
}


function searchList(){
    let option = $("#searchOption").children(":selected").text();
    let query = $("#query").val();
    let site = "web/list.html?query=" + query + "&type=" + option;
    
}