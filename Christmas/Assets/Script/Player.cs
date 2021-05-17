using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public int angle = -1;
    [HideInInspector]
    public float Gift = -1;
    Main main ;
    Material material;
    float gravity = 10;
    public GameObject[] particle;
    public GameObject[] GiftParticle;
    public Color TeamColor;
    // [HideInInspector]
    public int _player;
    GameObject _particle = null;
    GameObject _GiftParticle = null;
    // Start is called before the first frame update
    void Start()
    {
        angle = -1;
        Gift = -1;
        main = GameObject.Find("MainControl").GetComponent<Main>();
        if(gameObject.tag == "RedPlayer"){
            material = main.ColorMaterial[0];
        }else if(gameObject.tag == "BluePlayer"){
            material = main.ColorMaterial[1];
        }else if(gameObject.tag == "GreenPlayer"){
            material = main.ColorMaterial[2];
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(main.isplaying){
            move();
        }
        Gravity();
        Particle();
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject.layer == 8&&main.isplaying){
            other.GetComponent<MeshRenderer>().material = material;
        }
    }
    void move(){
        CharacterController play = GetComponent<CharacterController>();
        if (angle == -1||Gift!=-1){
            if(transform.childCount>0){
                try{gameObject.GetComponentInChildren<Animator>().SetBool("Run", false);}catch{}
                gameObject.transform.GetChild(0).transform.localRotation = new Quaternion();
                gameObject.transform.GetChild(0).transform.localPosition = Vector3.zero;
            }
        }else{
            if(transform.childCount>0){
                try{gameObject.GetComponentInChildren<Animator>().SetBool("Run", true);}catch{}
                gameObject.transform.GetChild(0).transform.localRotation = new Quaternion();
                gameObject.transform.GetChild(0).transform.localPosition = Vector3.zero;
            }
            gameObject.transform.rotation = Quaternion.Euler(0, 45 * angle, 0);
            play.Move(gameObject.transform.forward * Time.deltaTime * main.MoveSpeed);
        }
    }
    void Gravity(){
        CharacterController play = GetComponent<CharacterController>();
        play.Move(new Vector3(0,gravity,0)*Time.deltaTime*-1);
    }
    void Particle(){
        if(transform.childCount>0 && _particle == null){
            if(transform.GetChild(0).gameObject.layer==14){
            _particle = Instantiate(particle[_player],new Vector3(0,0,0),new Quaternion(0,0,0,0),transform);
            _particle.transform.localPosition = Vector3.zero;
            _particle.transform.localRotation = new Quaternion(0,0,0,0);
            _particle.transform.localScale = new Vector3(1,1,1);
            _particle.GetComponentInChildren<ParticleSystem>().startColor = TeamColor;
            }
        }else if (transform.childCount<=0){
            _particle = null;
        }else if (Gift>=0){
            _particle.SetActive(false);
        }else if(angle != -1&&_particle != null){
            _particle.SetActive(false);
        }else if(angle == -1&&_particle != null && Gift == -1){
            _particle.SetActive(true);
        }
        if(Gift >= 0){
            if (_GiftParticle == null){
                _GiftParticle = Instantiate(GiftParticle[_player], transform.position, new Quaternion(0,0,0,0), transform);
                // _GiftParticle.transform.localRotation = new Quaternion(3.14f/4*-1,0,0,0);
            }
            else{
                _GiftParticle.SetActive(true);
            }
            Gift -= Time.deltaTime;
        }
        else if (Gift != -1 && Gift <= 0){
            _GiftParticle.SetActive(false);
            Gift = -1;
        }
    }
}
