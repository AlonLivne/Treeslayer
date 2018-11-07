using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Singleton;
    public GameObject TreeCut;

    void Awake()
    {
        Singleton = this;
    }

    public void MakeTreeCutSound()
    {
        Instantiate(TreeCut, transform);
    }




}
