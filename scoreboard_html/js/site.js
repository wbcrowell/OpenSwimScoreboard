var transform = null;
var poolParams = null;
var numLanes = null;
var startEnd = null;
var laneOrder = null;
var currentEvent = 0;
var currentHeat = 0;
var clockIsRunning = false;
var lastElapsedTime;
var lastTimeZero = false;
var oldPlaceNumber = [];
var board = document;
var svg = document.querySelector("#svgScoreboard");
if (svg != null) {
    board = svg;
    var lst = localStorage.getItem("transform");
    if (lst != null) {
        transform = JSON.parse(lst);
    }
    var lsp = localStorage.getItem("poolParams");
    if (lsp != null) {
        poolParams = JSON.parse(lsp);
    }
    if (transform != null && transform.matrix3d != null) {
        apply3dTransform(transform.matrix3d);
    }
}
var armedForFinish = [];
if (!("numLanes" in window) || numLanes == null) {
    var qs = window.location.search.replace("?", "");
    numLanes = parseInt(qs);
}
if (isNaN(numLanes) || numLanes < 1) {
    if (poolParams != null && poolParams.numLanes != null) {
        numLanes = parseInt(poolParams.numLanes);
    }
}
if (isNaN(numLanes) || numLanes < 1) {
    numLanes = 6; //6 is the default number of lanes
}
if (poolParams != null) {
    startEnd = poolParams.startEnd;
    laneOrder = poolParams.laneOrder;
}
displayLanes(numLanes, startEnd ?? "right", laneOrder ?? "down");
for (i = 1; i <= numLanes; i++) {
    armedForFinish[i] = true;;
}
if (scoreboardNames == undefined) {
    var scoreboardNames = new Object();
}
this.addEventListener("finish", (e) => console.log(e.detail.lane));
let dataReceivedTimer;
const runTimer = () => {
    dataReceivedTimer = window.setTimeout(
        () => {
            if (connectionIsLive) {
                showDeadConnection();
            }
            connectionIsLive = false;
        }, 3000);
}
runTimer();

let connectionIsLive = false;
let connection = new signalR.HubConnectionBuilder().withUrl('http://localhost:5000/hubs/scoreboarddata').build();
connection.start().then(() => connection.stream('StreamScoreboardData').subscribe({
    next: (scoreboardData) => {
        if (Object.keys(scoreboardData).length > 0) {
            if (!connectionIsLive) {
                showLiveConnection();
            }
            connectionIsLive = true;
            clearTimeout(dataReceivedTimer);
            runTimer();
        }
        timeZero = (0 == parseInt(scoreboardData.currentClock.replace(/[^0-9.]/g, '')));
        var clockIsStarted = lastTimeZero && !timeZero;
        lastTimeZero = timeZero;
        clockIsRunning = lastElapsedTime != scoreboardData.currentClock;
        lastElapsedTime = scoreboardData.currentClock;
        var intEvent = parseInt(scoreboardData.currentEvent.replaceAll("&nbsp;", ""));
        var intHeat = parseInt(scoreboardData.currentHeat.replaceAll("&nbsp;", ""));
        var heChange = false;
        if (intEvent > 0 && intEvent != currentEvent) {
            currentEvent = intEvent;
            heChange = true;
        }
        if (intHeat > 0 && intHeat != currentHeat) {
            currentHeat = intHeat;
            heChange = true;
        }
        document.querySelector(".event").innerHTML = scoreboardData.currentEvent;
        document.querySelector(".heat").innerHTML = scoreboardData.currentHeat;
        document.querySelector(".clock").innerHTML = scoreboardData.currentClock;
        for (i = 1; i <= 10; i++) {
            if (i > 0 && scoreboardData.laneNumber[i] != null) {
                var isNewPlaceNumber = oldPlaceNumber[i] != null && oldPlaceNumber[i] != scoreboardData.lanePlace[i];
                oldPlaceNumber[i] = scoreboardData.lanePlace[i];
                if(board.querySelector("#lane" + i + " .laneNum") != null)
                    board.querySelector("#lane" + i + " .laneNum").innerHTML = scoreboardData.laneNumber[i];
                if (board.querySelector("#lane" + i + " .place") != null) {
                    board.querySelector("#lane" + i + " .place").innerHTML = scoreboardData.lanePlace[i];
                }
                if (board.querySelector("#lane" + i + " .time") != null)
                    board.querySelector("#lane" + i + " .time").innerHTML = scoreboardData.laneTime[i];
                if (scoreboardData.lanePlace[i] > 0 && armedForFinish[i] && isNewPlaceNumber && clockIsRunning) {
                    armedForFinish[i] = false;
                    this.dispatchEvent(
                        new CustomEvent("finish", {
                            detail: {
                                place: scoreboardData.lanePlace[i],
                                lane: scoreboardData.laneNumber[i],
                                time: scoreboardData.laneTime[i],
                            },
                        })
                    );
                }
                if (heChange) {
                    this.dispatchEvent(new CustomEvent("newheat"));
                    console.log("NEW HEAT");
                    armedForFinish[i] = true;
                    if (Object.keys(scoreboardNames).includes(intEvent.toString()) && scoreboardNames[intEvent].hasOwnProperty("Name")) {
                        document.querySelector(".eventName").innerHTML = scoreboardNames[currentEvent].Name;
                        document.querySelector(".eventName").style.display = "block";
                    } else {
                        document.querySelector(".eventName").style.display = "none";
                    }
                    if (Object.keys(scoreboardNames).includes(intEvent.toString()) && scoreboardNames[intEvent].hasOwnProperty("Heats")) {
                        if(board.querySelector("#lane" + i + " .name") != null)
                            board.querySelector("#lane" + i + " .name").innerHTML = scoreboardNames[currentEvent].Heats[currentHeat].Entries[i].Name;
                        if(board.querySelector("#lane" + i + " .lastName") != null)
                            board.querySelector("#lane" + i + " .lastName").innerHTML = scoreboardNames[currentEvent].Heats[currentHeat].Entries[i].LastName;
                        if(board.querySelector("#lane" + i + " .team") != null)
                            board.querySelector("#lane" + i + " .team").innerHTML = scoreboardNames[currentEvent].Heats[currentHeat].Entries[i].Team;
                    } else {
                        if(board.querySelector("#lane" + i + " .name") != null)
                            board.querySelector("#lane" + i + " .name").innerHTML = "";
                        if(board.querySelector("#lane" + i + " .lastName") != null)
                            board.querySelector("#lane" + i + " .lastName").innerHTML = "";
                        if(board.querySelector("#lane" + i + " .team") != null)
                            board.querySelector("#lane" + i + " .team").innerHTML = "";
                    }
                }
            }
        }
        if (clockIsStarted) {
            this.dispatchEvent(new CustomEvent("start"));
            console.log("START");
        }
    },
    error: (err) => {
        console.error(err);
        setInterval(function () { location.reload(); }, 10000);
    },
    complete: () => { }
})).catch((err) => {
    setInterval(function () { location.reload(); }, 10000);
    console.error(err);
});
