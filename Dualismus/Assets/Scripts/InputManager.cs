using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class InputManager : MonoBehaviour {
    public static InputManager Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    private bool GetKeyUp(KeyCode key)
    {
        return Input.GetKeyUp(key);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GetKeyUp(KeyCode.W) || GetKeyUp(KeyCode.S) 
            || GetKeyUp(KeyCode.UpArrow) || GetKeyUp(KeyCode.DownArrow))
        {
            TurnManager.Singleton.RowOrColoumn(false);
        }
        else if(GetKeyUp(KeyCode.A) || GetKeyUp(KeyCode.D)
            || GetKeyUp(KeyCode.LeftArrow) || GetKeyUp(KeyCode.RightArrow))
        {
            TurnManager.Singleton.RowOrColoumn(true);
        }


    }
}
