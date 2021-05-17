//全域變數
var Team = getValue("Team");
var yourname = ["▼","Alice","Ian","Jackie","Leo","Jerry","Wayne","James","Vader","Ginter","Johnson","Tina","Sam","Alan","Bmo","Cindy","Dally","Hanning"];
var player = ["▼","Santa","Deer","Snowman"]
var imageSources = ["/Image/UI-1-Button01.png", "/Image/UI-1-Button02.png"]
var UI2Back = ["/Image/UI-2-Layer-1.png", "/Image/UI-2-Layer-2.png", "/Image/UI-2-Layer-3.png"]
var index = 0;
//開始執行
for(a=0;a<yourname.length;a++){

}
//
//使用 WebSocket 的網址向 Server 開啟連結
// let ws = new WebSocket('ws://localhost:3000')
let ws = new WebSocket('ws://192.168.1.120:7999')
//開啟後執行的動作，指定一個 function 會在連結 WebSocket 後執行
ws.onopen = () => {
    // console.log('open connection')
    ws.send("Client");
}

//關閉後執行的動作，指定一個 function 會在連結中斷後執行
ws.onclose = () => {
    console.log('close connection')
}
//接收 Server 發送的訊息
ws.onmessage = event => {
    // console.log(event.data)
    var d = event.data.split(",");
    if(d[0] == "true"){
        document.getElementById("Play").style.display = "block";
        document.getElementById("Start").style.display = "none";
        if(Team == "Red"){
            // console.log(d[1]+","+d[2]);
            document.getElementById("PlayBack").style.backgroundImage  = "url("+UI2Back[d[2]-1]+")";
        }else if (Team == "Blue"){
            // console.log(d[3]+","+d[4]);
            document.getElementById("PlayBack").style.backgroundImage  = "url("+UI2Back[d[4]-1]+")";
        }else if (Team == "Green"){
            // console.log(d[5]+","+d[6]);
            document.getElementById("PlayBack").style.backgroundImage  = "url("+UI2Back[d[6]-1]+")";
        }
    }else if (event.data == "false"){
        document.getElementById("Play").style.display = "none";
        document.getElementById("Start").style.display = "block";
    }else if (event.data == "restart"){
        document.getElementById("Play").style.display = "none";
        document.getElementById("Start").style.display = "block";
        document.getElementById("name").selectedIndex = 0;
        document.getElementById("player").selectedIndex = 0;
        // document.getElementById("Send").style.display = "none";
    }else if (d[0] == "start"){
        document.getElementById("Play").style.display = "block";
        document.getElementById("Start").style.display = "none";
        if(Team == "Red"){
            // console.log(d[1]+","+d[2]);
            document.getElementById("PlayBack").style.backgroundImage  = "url("+UI2Back[d[2]-1]+")";
        }else if (Team == "Blue"){
            // console.log(d[3]+","+d[4]);
            document.getElementById("PlayBack").style.backgroundImage  = "url("+UI2Back[d[4]-1]+")";
        }else if (Team == "Green"){
            // console.log(d[5]+","+d[6]);
            document.getElementById("PlayBack").style.backgroundImage  = "url("+UI2Back[d[6]-1]+")";
        }
    }
}
setInterval (function(){
    if (index === imageSources.length) {
      index = 0;
    }
    n = document.getElementById("name").value;
    p = document.getElementById("player").value;
    if(n!=yourname[0]&&p!=player[0]){
        document.getElementById("SendButton").style.backgroundImage  = "url("+imageSources[index]+")";
        index++;
    }else{
        // document.getElementById("SendButton").style.backgroundImage  = "url("+imageSources[0]+")";
    }
  } , 500);

function Send(angle){
    // var Team = getValue("Team");
    // console.log(getValue("Team"));
    if(Team == "Red"||Team == "Green"||Team == "Blue"){
        ws.send(Team+","+angle);
    }
}
function Select(){
    // console.log(document.getElementById("name").value);
    
}
function StartSend(){
    n = document.getElementById("name").value;
    p = document.getElementById("player").value;
    if(n!=yourname[0]&&p!=player[0]){
        document.getElementById("Play").style.display = "block";
        document.getElementById("Start").style.display = "none";
        n = document.getElementById("name").value;
        p = document.getElementById("player").value;
        for (a = 0; a < yourname.length; a++) {
            for (b = 0; b < player.length; b++) {
                if (n == yourname[a] && p == player[b]) {
                    ws.send("start," + Team + "," + a + "," + b);
                    document.getElementById("PlayBack").style.backgroundImage  = "url("+UI2Back[b-1]+")";
                }
            }
        }
    }else{
        
    }
}
