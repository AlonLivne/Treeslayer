using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Visuals
{
    Trees,
    Buildings,
    Overlay,
}

public class VisualsMediator : MonoBehaviour {
    public static VisualsMediator Singleton;
    public List<GameObject> TreeVisuals;
    public List<GameObject> BuildingsVisuals;
    public List<GameObject> OverLayVisuals;

    private void Awake()
    {
        Singleton = this;

    }

    // Use this for initialization
    void Start () {
        Board.Singleton.OnVisualMeidatorCreated();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject GetVisual(Visuals visual)
    {
        var visLists = new List<GameObject>();

        switch (visual)
        {
            case Visuals.Buildings:
                visLists = BuildingsVisuals;
                break;
            case Visuals.Trees:
                visLists = TreeVisuals;
                break;
            case Visuals.Overlay:
                visLists = OverLayVisuals;
                break;

        }

        var index = Random.Range(0, visLists.Count);

        return visLists[index];
    }
}
