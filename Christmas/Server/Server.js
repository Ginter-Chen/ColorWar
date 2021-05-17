console.clear();
console.log("Christmas Server\n");
const e = require('express');
//import express 和 ws 套件
const express = require('express');
const { client } = require('websocket');
const SocketServer = require('ws').Server
var unity = null;
var playing = false;
//指定開啟的 port
const PORT = 7999
var red = new Array(2);
var blue = new Array(2);
var green = new Array(2);
//創建 express 的物件，並綁定及監聽 3000 port ，且設定開啟後在 console 中提示
const server = express()
    .listen(PORT)

//將 express 交給 SocketServer 開啟 WebSocket 的服務
const wss = new SocketServer({ server })

//當 WebSocket 從外部連結時執行
wss.on('connection', ws => {

    //連結時執行此 console 提示
    // console.log('Client connected')
    if(playing){
        ws.send("true,"+red[0]+","+red[1]+","+blue[0]+","+blue[1]+","+green[0]+","+green[1]);
    }else{
        ws.send("false");
    }
    //對 message 設定監聽，接收從 Client 發送的訊息
    ws.on('message', data => {
        //data 為 Client 發送的訊息，現在將訊息原封不動發送出去
        if(data == "unity"){
            if(unity == null){
                unity = ws;
                console.log("unity Connect");
            }else{
                unity = ws;
                console.log("unity ReConnect");
            }
        }else if(data == "Client"){
            console.log("Client Connect");
        }else if(data == "True"){
            playing = true;
            // console.log("Play:True");
            wss.clients.forEach(client => {
                if(client != unity){
                    // client.send("start");
                    client.send("start,"+red[0]+","+red[1]+","+blue[0]+","+blue[1]+","+green[0]+","+green[1]);
                }
            });
        }else if(data == "False"){
            playing = false;
            // console.log("Play:false");
            wss.clients.forEach(client => {
                if(client != unity){
                    client.send("restart");
                }
            });
        }else{
            // console.log("Unity Send:"+data);
            if(unity != null){
                unity.send(data);
                if(playing == false){
                    var res = data.split(",");
                    if(res[0]=="start"){
                        if(res[1]=="Red"){
                            red[0] = res[2];
                            red[1] = res[3];
                        }else if(res[1]=="Blue"){
                            blue[0] = res[2];
                            blue[1] = res[3];
                        }else if(res[1]=="Green"){
                            green[0] = res[2];
                            green[1] = res[3];
                        }
                    }
                }
            }
        }
    })
    //當 WebSocket 的連線關閉時執行
    ws.on('close', () => {
        // console.log('Close connected')
    })
})