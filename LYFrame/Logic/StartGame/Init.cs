using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LYFrame;

public class Init : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.ShowUIForms("LoginPanel");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
