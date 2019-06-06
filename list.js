
function getQueryParam(name) {
    let url = window.location.search.substring(1);
    let valueList = url.split('&');

    for (let i = 0; i < valueList.length; i++) {
        let pair = valueList[i].split('=');

        if (pair[0] == name) {
            return pair[1];
        }
    }
    return "";
}


window.onload = function() {
    $(document).ready(function(){
        $("#div1").load("./base/head.html");
        $("#div2").load("./base/foot.html");
    });

}



