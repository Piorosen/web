
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

var maxShow = 40;
var file;
var query;
var type;

var searchResult = new Array();

window.onload = function() {
    $(document).ready(function(){
        $("#div1").load("./base/head.html");
        $("#div2").load("./base/foot.html");
    });
    
    file = JSON.parse(loadFile("./event/event.json"));
    query = getQueryParam("query");
    type = getQueryParam("type");

    query = decodeURIComponent(query);

    findEvent();
    printResult();

}

function printResult(){
    var sss = document.getElementById("result");

    let size = 5;

    for (let row = 0; row < maxShow / size; row++){
        var data = "<tr class=\"searchRow\">\n";
        for (let col = 0; col < size; col++){
            data += "<td class=\"searchData\">\n";
            data += "<p class=\"p1\">" + searchResult[row * size + col].Title + "</p>"
            data += "<p class=\"p2\">" + searchResult[row * size + col].StartDay + "~" + searchResult[row * size + col].EndDay + "</p>"
            data += "<p class=\"p3\">" + searchResult[row * size + col].Country + "</p>"
            data += "</td>\n";
        }
        data += "</tr>\n";
        sss.innerHTML += data;
    }

    document.getElementById("searchTotal").innerText += " " + searchResult.length + " 개 탐색 됨";
}

function findEvent() {
    var index = 0;
    type = String(type);

    for (let i = 0; i < file.length; i++) {
        if (type == "Title"){
            if (String(file[i].Title).indexOf(query) == -1){
                continue;
            }
        } else if (type == "Date"){
            if (String(file[i].Date).indexOf(query) == -1){
                continue;
            }
        } else if (type == "Country") {
            let t = String(file[i].Country).indexOf(query)
            if (String(file[i].Country).indexOf(query) == -1){
                continue;
            }
        }

        searchResult[index++] = file[i];

    }
}