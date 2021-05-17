using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
public class Main : MonoBehaviour
{
#region GameMode
    [Header("GameMode")]
    public float MaxStartTime = 10;
    public float MaxStartWaitTime = 3;
    public float MaxGameTime = 30;
    [HideInInspector]
    public bool isplaying = false;
    [HideInInspector]
    public bool Idle = false;
    [HideInInspector]
    public bool TrunToPlay = false;
    [HideInInspector]
    public bool Win = false;
    [HideInInspector]
    public float StartTime = -1;
    [HideInInspector]
    public float StartWaitTime = -1;
    [HideInInspector]
    public float GameTime = -1;
#endregion
#region UI
[Header("UI")]
public GameObject UI_Start;
public GameObject UI_Playing;
public GameObject UI_QRCode;
public Text UI_StartTime;
public Text UI_GameTime;
public Text[] Score;
#endregion
#region Camera
    [Header("Camera")]
    public GameObject MainCamera;
    public GameObject[] CameraPosition;
#endregion
#region Player
    [Header("Player")]
    public float MoveSpeed = 2;
    public GameObject[] Player;
    public GameObject[] PlayerPrefab;
    public GameObject[] PlayerPrefab_Out;
    public RenderTexture[] Plyaer_Out_RenderTexture;
    public GameObject[] OutPlayer;
    public GameObject[] UI_FaceFocus;
    // public Material[] Material_Face;
    public Texture[] Texture_Face;
    public GameObject DeathParticle;
    float CreatGift;
    Vector2[] PlayerSelect = new Vector2[3];
    public GameObject[] PlayerStartPosition;
    public GameObject Gift;
    //////////////////////////////////////////0//////1///////2//////3//////4///////5///////6//////7////////8/////////9///////10////11/////12////13/////14//////15///////16/////
    string [] EveryBodyName = new string[]{"Alice","Ian","Jackie","Leo","Jerry","Wayne","James","Vader","Ginter","Johnson","Tina","Sam","Alan","Bmo","Cindy","Dally","Hanning"};
#endregion
#region Grid
    [Header("地板")]
    public int Size;
    public float Max;
    public GameObject ParentPlane;
    public GameObject ColorPlane ;
    public Material[] ColorMaterial;
    GameObject[] ColorPlaneClone;
#endregion
#region Audio
    [Header("Audio")]
    public AudioSource Audio_Waiting;
    public AudioSource Audio_Playing;
    public AudioSource Audio_CountDown;
    public AudioSource Audio_Win;
#endregion
    // Start is called before the first frame update
    void Start()
    {
        CreatPlaen();
        // Idle = true;
        // GameMode("start");
        UI_StartTime.gameObject.SetActive(false);
        UI_QRCode.SetActive(false);
        StartTime = -1;
        GameTime = -1;
        
    }
    // Update is called once per frame
    void Update()
    {
        ShowScore();
        UI_Select();
        // CameraMove();
        // CreatPlayer();
        StartGame();
        Gameing();
        TestFunction();
    }
    /////建立地板
    void CreatPlaen(){
        float f =(Max/2*-1)+(Max/Size/2);
        float d = Max/Size;
        ColorPlaneClone = new GameObject[Size*Size];
        int n = 0;
        for(int x = 0;x<Size;x++){
            for(int y = 0;y<Size;y++){
                GameObject c = Instantiate(ColorPlane,new Vector3(f+d*x,0.01f,f+d*y),new Quaternion(0,0,0,0),ParentPlane.transform);
                c.transform.localScale = new Vector3(1f/Size,1f/Size,1f/Size);
                c.transform.localPosition = new Vector3(f+d*x,0.01f,f+d*y);
                c.SetActive(true);
                ColorPlaneClone[n] = c;
                n++;
            }
        }
        ColorPlane.SetActive(false);
    }
    /////分數轉換成UI
    void ShowScore(){
        int r = 0;
        int b = 0;
        int g = 0;
        for(int a = 0;a<ColorPlaneClone.Length;a++){
            if(ColorPlaneClone[a].GetComponent<MeshRenderer>().materials[0].color == ColorMaterial[0].color){
                r++;
            }else if(ColorPlaneClone[a].GetComponent<MeshRenderer>().materials[0].color == ColorMaterial[1].color){
                b++;
            }else if(ColorPlaneClone[a].GetComponent<MeshRenderer>().materials[0].color == ColorMaterial[2].color){
                g++;
            }
        }
        Score[0].text = r.ToString();
        Score[1].text = b.ToString();
        Score[2].text = g.ToString();
    }
    void TestFunction(){
        if(Input.GetKeyDown(KeyCode.F1)){
            if(!UI_QRCode.activeSelf||!UI_QRCode.transform.GetChild(0).gameObject.activeSelf||!UI_QRCode.transform.GetChild(1).gameObject.activeSelf||!UI_QRCode.transform.GetChild(2).gameObject.activeSelf){
                UI_QRCode.SetActive(true);
                for(int a = 0;a<UI_QRCode.transform.childCount;a++){
                    UI_QRCode.transform.GetChild(a).gameObject.SetActive(true);
                }
            }else{
                UI_QRCode.SetActive(false);
            }
        }
    }
    public void GameMode(string mode){
        isplaying = false;
        Idle = false;
        TrunToPlay = false;
        Win = false;
        if(mode == "play"){
            isplaying = true;
            GetComponent<ws_script>().SendPlaying(true);
            for (int a = 0; a < OutPlayer[0].transform.childCount; a++)
            {
                GameObject.Destroy(OutPlayer[0].transform.GetChild(a).gameObject);
            }
            for (int a = 0; a < OutPlayer[1].transform.childCount; a++)
            {
                GameObject.Destroy(OutPlayer[1].transform.GetChild(a).gameObject);
            }
            for (int a = 0; a < OutPlayer[2].transform.childCount; a++)
            {
                GameObject.Destroy(OutPlayer[2].transform.GetChild(a).gameObject);
            }
            Audio_Playing.Play();
            Audio_Waiting.Stop();
        }
        if(mode == "start"){
            Idle = true;
            for(int a = 0;a<UI_FaceFocus.Length;a++){
                UI_FaceFocus[a].SetActive(false);
            }
            for(int a = 0;a<GameObject.FindGameObjectsWithTag("Gift").Length;a++){
                GameObject.Destroy(GameObject.FindGameObjectsWithTag("Gift")[a]);
            }
            GetComponent<ws_script>().SendPlaying(false);
            UI_QRCode.SetActive(true);
            for(int a = 0;a<UI_QRCode.transform.childCount;a++){
                UI_QRCode.transform.GetChild(a).gameObject.SetActive(true);
            }
            Audio_Playing.Stop();
            Audio_Waiting.Play();
        }
    }
    void UI_Select(){
        UI_Start.SetActive(false);
        UI_Playing.SetActive(false);
        if(isplaying){
            UI_Playing.SetActive(true);
        }
        if(Idle){
            UI_Start.SetActive(true);
        }
    }
    void CameraMove(){
        if(isplaying){
            MainCamera.transform.position = CameraPosition[1].transform.position;
            MainCamera.transform.rotation = CameraPosition[1].transform.rotation;
        }
        if(Idle){
            MainCamera.transform.position = CameraPosition[0].transform.position;
            MainCamera.transform.rotation = CameraPosition[0].transform.rotation;
        }
    }
    public void CreatPlayer(int team,int face,int playmodel){
        for(int a = 0; a < Player[team].transform.childCount;a++){
            GameObject.Destroy(Player[team].transform.GetChild(a).gameObject);
        }
        for(int a = 0; a < OutPlayer[team].transform.childCount;a++){
            GameObject.Destroy(OutPlayer[team].transform.GetChild(a).gameObject);
        }
        GameObject c = Instantiate(PlayerPrefab[playmodel],new Vector3(0,0,0),new Quaternion(0,0,0,0),Player[team].transform);
        c.transform.localPosition = Vector3.zero;
        c.transform.localRotation = new Quaternion(0,0,0,0);
        c.transform.localScale = new Vector3(1,1,1);
        try{
            c.GetComponentInChildren<SkinnedMeshRenderer>().materials[1].SetTexture("_MainTex",Texture_Face[face]);
        }catch{
            c.GetComponentInChildren<MeshRenderer>().materials[1].SetTexture("_MainTex",Texture_Face[face]);
        }
        GameObject o = Instantiate(PlayerPrefab_Out[playmodel],new Vector3(0,0,0),new Quaternion(0,0,0,0),OutPlayer[team].transform);
        o.transform.localPosition = Vector3.zero;
        o.transform.localRotation = new Quaternion(0,0,0,0);
        o.transform.localScale = new Vector3(1,1,1);
        try{
            o.GetComponentInChildren<SkinnedMeshRenderer>().materials[1].SetTexture("_MainTex",Texture_Face[face]);
        }catch{
            o.GetComponentInChildren<MeshRenderer>().materials[1].SetTexture("_MainTex",Texture_Face[face]);
        }
        o.GetComponentInChildren<Camera>().targetTexture = Plyaer_Out_RenderTexture[team];
        UI_FaceFocus[team].SetActive(true);
        UI_FaceFocus[team].GetComponent<RawImage>().color = new Color(1,1,1,1);
        PlayerSelect[team] = new Vector2(face,playmodel);
        Player[team].GetComponent<Player>()._player = playmodel;
        UI_QRCode.transform.GetChild(team).gameObject.SetActive(false);
        // Debug.Log(face);
    }
    void StartGame(){
        if(GameTime == -1&&Idle){
            if(Player[0].transform.childCount>0&&Player[1].transform.childCount>0&&Player[2].transform.childCount>0&&StartWaitTime==-1&&StartTime == -1){
                StartWaitTime = MaxStartWaitTime;
                UI_QRCode.SetActive(false);
            }
            if(StartWaitTime>=0){
                StartWaitTime -=Time.deltaTime;
            }else if(StartWaitTime!=-1&&StartWaitTime<=0&&StartTime == -1){
                StartWaitTime = -1;
                StartTime = MaxStartTime;
                DOTween.Kill(MainCamera.transform);
                MainCamera.transform.DOMove(CameraPosition[1].transform.position,MaxStartTime);
                MainCamera.transform.DORotate(CameraPosition[1].transform.rotation.eulerAngles,MaxStartTime);
                StartCoroutine(WaitCoundDown(MaxStartTime));
            }
            if(StartTime>=0){
                UI_StartTime.gameObject.SetActive(true);
                StartTime-=Time.deltaTime;
                UI_StartTime.text = Mathf.Floor(StartTime+1).ToString("0");
                for(int a = 0;a<UI_FaceFocus.Length;a++){
                    UI_FaceFocus[a].GetComponent<RawImage>().color = new Color(1,1,1,StartTime/MaxStartTime);
                }
            }else if (Idle&&StartTime!=-1&&StartTime<=0){
                UI_StartTime.gameObject.SetActive(false);
                StartTime = -1;
                GameMode("play");
                GameTime = MaxGameTime;
                CreatGift = Random.Range(5,8);
            }
            for(int a =0;a<3;a++){
                Player[a].transform.position = PlayerStartPosition[a].transform.position;
                Player[a].transform.rotation = PlayerStartPosition[a].transform.rotation;
            }
        }
    }
    void Gameing(){
        if(isplaying){
            if(GameTime>0){
                UI_GameTime.text = Mathf.Floor(GameTime).ToString("0");
                GameTime-=Time.deltaTime;
                if(CreatGift>=0){
                    CreatGift -=Time.deltaTime;
                }else{
                    // GameObject Red = GameObject.FindGameObjectsWithTag("RedPlayer")[0];
                    // GameObject Blue = GameObject.FindGameObjectsWithTag("BluePlayer")[0];
                    // GameObject Green = GameObject.FindGameObjectsWithTag("GreenPlayer")[0];
                    GameObject c = Instantiate(Gift,ParentPlane.transform.position+new Vector3(Random.Range(-4.5f,4.5f),0.23f,Random.Range(-4.5f,4.5f)),Quaternion.Euler(new Vector3(0,Random.Range(0,360),0)));
                    while(Mathf.Abs(Vector3.Distance(c.transform.position,Player[0].transform.position))<=2||Mathf.Abs(Vector3.Distance(c.transform.position,Player[1].transform.position))<=2||Mathf.Abs(Vector3.Distance(c.transform.position,Player[2].transform.position))<=2){
                        c.transform.position = ParentPlane.transform.position+new Vector3(Random.Range(-4.5f,4.5f),0.23f,Random.Range(-4.5f,4.5f));
                    }
                    CreatGift = Random.Range(8,12);
                }
            }else if (GameTime!=-1){
                int RScore = int.Parse(Score[0].text);  
                int BScore = int.Parse(Score[1].text);  
                int GScore = int.Parse(Score[2].text);
                if(RScore>BScore&&RScore>GScore){
                    GameTime = -1;
                    StartCoroutine(BackToStart(7,0));
                }else if (BScore>RScore&&BScore>GScore){
                    GameTime = -1;
                    StartCoroutine(BackToStart(7,1));
                    UI_GameTime.text = "";
                }else if (GScore>RScore&&GScore>BScore){
                    GameTime = -1;
                    StartCoroutine(BackToStart(7,2));
                    UI_GameTime.text = "";
                }else{
                    GameTime = 0.1f;
                }
                // UI_GameTime.text = "";
                // StartCoroutine(BackToStart(10));
            }
        }
    }
    IEnumerator BackToStart(int second , int player)
    {
        GameObject Red = Player[0];
        GameObject Blue = Player[1];
        GameObject Green = Player[2];
        GameObject c = MainCamera;

        isplaying = false;
        try{Red.GetComponentInChildren<Animator>().SetBool("Run", false);}catch{}
        try{Blue.GetComponentInChildren<Animator>().SetBool("Run", false);}catch{}
        try{Green.GetComponentInChildren<Animator>().SetBool("Run", false);}catch{}
        if(player == 0){
            for (int a = 0; a < Blue.transform.childCount; a++)
            {
                GameObject.Destroy(Blue.transform.GetChild(a).gameObject);
            }
            for (int a = 0; a < Green.transform.childCount; a++)
            {
                GameObject.Destroy(Green.transform.GetChild(a).gameObject);
            }
            c = Red.GetComponentInChildren<Camera>().gameObject;
            Instantiate(DeathParticle,Blue.transform.position+new Vector3(0,1,0),new Quaternion(0,0,0,0),Blue.transform);
            Instantiate(DeathParticle,Green.transform.position+new Vector3(0,1,0),new Quaternion(0,0,0,0),Green.transform);
        }else if (player == 1){
            for (int a = 0; a < Red.transform.childCount; a++)
            {
                GameObject.Destroy(Red.transform.GetChild(a).gameObject);
            }
            for (int a = 0; a < Green.transform.childCount; a++)
            {
                GameObject.Destroy(Green.transform.GetChild(a).gameObject);
            }
            c = Blue.GetComponentInChildren<Camera>().gameObject;
            Instantiate(DeathParticle,Red.transform.position+new Vector3(0,1,0),new Quaternion(0,0,0,0),Red.transform);
            Instantiate(DeathParticle,Green.transform.position+new Vector3(0,1,0),new Quaternion(0,0,0,0),Green.transform);
        }else if (player == 2){
            for (int a = 0; a < Red.transform.childCount; a++)
            {
                GameObject.Destroy(Red.transform.GetChild(a).gameObject);
            }
            for (int a = 0; a < Blue.transform.childCount; a++)
            {
                GameObject.Destroy(Blue.transform.GetChild(a).gameObject);
            }
            c = Green.GetComponentInChildren<Camera>().gameObject;
            Instantiate(DeathParticle,Blue.transform.position+new Vector3(0,1,0),new Quaternion(0,0,0,0),Blue.transform);
            Instantiate(DeathParticle,Red.transform.position+new Vector3(0,1,0),new Quaternion(0,0,0,0),Red.transform);
        }
        DOTween.Kill(MainCamera.transform);
        MainCamera.transform.DOMove(c.transform.position, 3);
        MainCamera.transform.DORotate(c.transform.rotation.eulerAngles, 3);
        yield return new WaitForSeconds(1);
        DOTween.Kill(MainCamera.transform);
        MainCamera.transform.DOMove(c.transform.position, 2);
        MainCamera.transform.DORotate(c.transform.rotation.eulerAngles, 2);
        yield return new WaitForSeconds(2);
        Audio_Win.Play();
        // Debug.Log(EveryBodyName[int.Parse(PlayerSelect[player].x.ToString())]);
        yield return new WaitForSeconds(second-3);
        for (int a = 0; a < Red.transform.childCount; a++)
        {
            GameObject.Destroy(Red.transform.GetChild(a).gameObject);
        }
        for (int a = 0; a < Blue.transform.childCount; a++)
        {
            GameObject.Destroy(Blue.transform.GetChild(a).gameObject);
        }
        for (int a = 0; a < Green.transform.childCount; a++)
        {
            GameObject.Destroy(Green.transform.GetChild(a).gameObject);
        }
        GameTime = -1;
        StartTime = -1;
        StartWaitTime = -1;
        UI_StartTime.gameObject.SetActive(false);
        for (int a = 0; a < ColorPlaneClone.Length; a++)
        {
            ColorPlaneClone[a].GetComponent<MeshRenderer>().materials[0].color = ColorPlane.GetComponent<MeshRenderer>().materials[0].color;
        }
        // GameMode("start");
        DOTween.Kill(MainCamera.transform);
        MainCamera.transform.DOMove(CameraPosition[0].transform.position, 3);
        MainCamera.transform.DORotate(CameraPosition[0].transform.rotation.eulerAngles, 3);
        yield return new WaitForSeconds(3);
        GameMode("start");
    }
    IEnumerator WaitCoundDown(float second){
        float Time = second -3;
        // Debug.Log(Time);
        yield return new WaitForSeconds(Time);
        Audio_CountDown.Play();
    }
}