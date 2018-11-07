using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour {

    public void Play()
    {
        SoundManager.Singleton.MakePlaySound();
        Invoke("ChangeSence", 1.4f);
    }

    void ChangeSence()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
