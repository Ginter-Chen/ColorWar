using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ws_script : MonoBehaviour
{
	public int NetworkSpeed = 5;
	public string ip = "localhost";
	public string port = "8000";
	WebSocket w;
	IEnumerator Start ()
	{
		w = new WebSocket (new Uri ("ws://" + ip + ":" + port));
		yield return StartCoroutine (w.Connect ());
		w.SendString ("unity");
		GameObject.FindGameObjectWithTag("Control").GetComponent<Main>().GameMode("start");
	}
	void Update() {
		string s = w.RecvString();
		if(s != null){
			string[] Array = s.Split(',');
			if(Array[0]=="Red"){
				Player p = GameObject.FindGameObjectsWithTag("RedPlayer")[0].GetComponent<Player>();
				p.angle =int.Parse(Array[1]);
			}
			if(Array[0]=="Blue"){
				Player p = GameObject.FindGameObjectsWithTag("BluePlayer")[0].GetComponent<Player>();
				p.angle =int.Parse(Array[1]);
			}
			if(Array[0]=="Green"){
				Player p = GameObject.FindGameObjectsWithTag("GreenPlayer")[0].GetComponent<Player>();
				p.angle =int.Parse(Array[1]);
			}
			if(Array[0]=="start"){
				Main m = GameObject.FindGameObjectsWithTag("Control")[0].GetComponent<Main>();
				if(Array[1]=="Red"){
					m.CreatPlayer(0,int.Parse(Array[2])-1,int.Parse(Array[3])-1);
				}
				if(Array[1]=="Blue"){
					m.CreatPlayer(1,int.Parse(Array[2])-1,int.Parse(Array[3])-1);
				}
				if(Array[1]=="Green"){
					m.CreatPlayer(2,int.Parse(Array[2])-1,int.Parse(Array[3])-1);
				}
			}
		}
	}
	public void SendPlaying(bool p){
		w.SendString(p.ToString());
	}
}

