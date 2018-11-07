using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Singleton;
    public GameObject TreeCut;

    public GameObject Place;
    public GameObject CantPlace;
    public GameObject Play;

    void Awake()
    {
        Singleton = this;
    }

    public void MakeTreeCutSound()
    {
        Instantiate(TreeCut, transform);
    }

    public void MakePlaceSound()
    {
        Instantiate(Place, transform);
    }

    public void MakeCantPlaceSound()
    {
        Instantiate(CantPlace, transform);
    }

    public void MakePlaySound()
    {
        Instantiate(Play, transform);
    }



}
