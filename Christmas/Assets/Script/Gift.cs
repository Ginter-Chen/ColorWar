using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
    public GameObject bad;
    public GameObject good;
    bool up = false;
    public float move = 0;
    public float speed = 0;
    public float LockTime = 2;
    float m = 0;
    // Start is called before the first frame update
    void Start()
    {
        up = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(up){
            if(m<move){
                Vector3 a = new Vector3(0,Time.deltaTime*speed,0);
                transform.position += a;
                for(int x = 0;x<transform.childCount;x++){
                    transform.GetChild(x).transform.position-=a;
                }
                m+=a.y;
            }else{
                up = false;
                m = 0;
            }
        }else{
            if(m<move){
                Vector3 a = new Vector3(0,Time.deltaTime*speed,0);
                transform.position -= a;
                for(int x = 0;x<transform.childCount;x++){
                    transform.GetChild(x).transform.position+=a;
                }
                m+=a.y;
            }else{
                up = true;
                m = 0;
            }
        }
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "RedPlayer"||other.gameObject.tag == "BluePlayer"||other.gameObject.tag == "GreenPlayer"){
            int f = Random.Range(0,10);
            if(f<2){
                Instantiate(bad,new Vector3(0,0,0),new Quaternion(0,0,0,0));
                other.GetComponent<Player>().Gift = LockTime;
            }else{
                Instantiate(good,new Vector3(0,0,0),new Quaternion(0,0,0,0));
                GameObject Red = GameObject.FindGameObjectsWithTag("RedPlayer")[0];
                GameObject Blue = GameObject.FindGameObjectsWithTag("BluePlayer")[0];
                GameObject Green = GameObject.FindGameObjectsWithTag("GreenPlayer")[0];
                if(other.gameObject.tag == "RedPlayer"){
                    Blue.GetComponent<Player>().Gift = LockTime;
                    Green.GetComponent<Player>().Gift = LockTime;
                }else if (other.gameObject.tag == "BluePlayer"){
                    Red.GetComponent<Player>().Gift = LockTime;
                    Green.GetComponent<Player>().Gift = LockTime;
                }else if (other.gameObject.tag == "GreenPlayer"){
                    Red.GetComponent<Player>().Gift = LockTime;
                    Blue.GetComponent<Player>().Gift = LockTime;
                }
            }
            GameObject.Destroy(this.gameObject);
        }
    }
}
