using System.Collections;
using System.Collections.Generic;
using CAH.Auth.Mobile;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GoogleAuthService a = new GoogleAuthService();
        a.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
