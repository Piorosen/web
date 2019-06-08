
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

function loadFile(filePath) {
    var result = null;
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.open("GET", filePath, false);
    xmlhttp.send();
    if (xmlhttp.status==200) {
      result = xmlhttp.responseText;
    }
    return result;
}

function drawPage(value){
    value.Image = value.Image == "" ? "./base/img/404.png" : value.Image


    document.getElementById("image").innerHTML = "<img class=\"image_value\" src=\"" + value.Image + "\">";
    let slist = document.getElementById("slist");
    slist.innerHTML += "<p>" + "제목 : " + value.Title + "</p>";
    slist.innerHTML += "<p>" + "국가 : " + value.Country + "</p>";
    slist.innerHTML += "<p>" + "지역 : " + value.Location + "</p>";
    slist.innerHTML += "<p>" + "위치 : " + value.Position + "</p>";
    slist.innerHTML += "<p>" + "행사 일정 : " + value.StartDay + " ~ " + value.EndDay + "</p>";

    let detail = document.getElementById("detail");
    detail.innerHTML += "<p class=\"UpClass\">" + "설명 : " + value.Description + "</p>";
    detail.innerHTML += "<p>" + "이벤트 : " + value.EventTime + "</p>";
    detail.innerHTML += "<p>" + "홈페이지 : " + value.Homapage + "</p>";
    detail.innerHTML += "<p>" + "연락처 : " + value.PhoneNumber + "</p>";

}

var file;

window.onload = function() {
    $(document).ready(function(){
        $("#div1").load("./base/head.html");
        $("#div2").load("./base/foot.html");
    });

    file = JSON.parse(loadFile("./event/event.json"));

    index = Number(getQueryParam("id"));
    
    drawPage(file[index - 1])
    
}


