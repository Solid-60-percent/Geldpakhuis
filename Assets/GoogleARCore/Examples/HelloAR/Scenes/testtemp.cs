using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testtemp : MonoBehaviour {

    [SerializeField]
    public Text guiSaldoText = null;

    void Start()
    {
        guiSaldoText.text = PlayerPrefs.GetInt("saldo").ToString();    
    }

    public void testtttt()
    {
        Debug.Log("Hoi - " + PlayerPrefs.GetInt("saldo"));
    }
}
