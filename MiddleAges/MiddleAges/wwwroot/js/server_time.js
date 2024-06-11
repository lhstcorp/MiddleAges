var time = setInterval(function () {
    var date = new Date();
    document.getElementById("serverTime").innerHTML = (date.getUTCDate() + "." + (date.getUTCMonth() + 1) + "." + date.getUTCFullYear() + " " + date.getUTCHours(0) + ":" + date.getUTCMinutes() + ":" + date.getUTCSeconds());
},
1000);