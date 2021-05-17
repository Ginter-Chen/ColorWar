//#region
var angle;
var Team = getValue("Team");
var joyParam = { "internalFillColor": '#ff0000', "internalStrokeColor": '#000000',"externalStrokeColor":'#ff0000'};
if(Team == "Red"){
	joyParam = { "internalFillColor": '#ff0000', "internalStrokeColor": '#ff0000',"externalStrokeColor":'#ff0000'};
}else if (Team == "Blue"){
	joyParam = { "internalFillColor": '#0000ff', "internalStrokeColor": '#0000ff',"externalStrokeColor":'#0000ff'};
}else if (Team == "Green"){
	joyParam = { "internalFillColor": '#00ff00', "internalStrokeColor": '#00ff00',"externalStrokeColor":'#00ff00'};
}
var joy = new JoyStick('joyDiv',joyParam);
setInterval(function(){
	// console.log(joy.GetDir())
	if(joy.GetDir()!=angle){
		angle = joy.GetDir();
		if(angle == "C"){
			Send(-1);
		}
		if(angle == "N"){
			Send(0);
		}
		if(angle == "NE"){
			Send(1);
		}
		if(angle == "E"){
			Send(2);
		}
		if(angle == "SE"){
			Send(3);
		}
		if(angle == "S"){
			Send(4);
		}
		if(angle == "SW"){
			Send(5);
		}
		if(angle == "W"){
			Send(6);
		}
		if(angle == "NW"){
			Send(7);
		}
	}
}, 100);
//#endregion