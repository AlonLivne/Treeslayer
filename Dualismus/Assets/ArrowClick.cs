using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowClick : MonoBehaviour {
    
    public bool IsHorizontal;
    
    public void OnMouseUp()
    {
        TurnManager.Singleton.RowOrColoumn(IsHorizontal);
    }
}
