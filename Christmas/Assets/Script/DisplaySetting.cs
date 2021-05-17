using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySetting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Display.displays[1].Activate(0,0,60);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
