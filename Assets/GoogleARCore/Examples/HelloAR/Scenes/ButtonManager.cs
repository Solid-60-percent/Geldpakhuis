using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
    

	public void test(int saldo)
    {
        PlayerPrefs.SetInt("saldo", saldo);
        SceneManager.LoadScene("HelloAR");
        Debug.Log(saldo);
    }
}
