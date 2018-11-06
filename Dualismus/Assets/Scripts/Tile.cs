using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Clear,
    Tree,
    Building,
    CutTree
}

[Serializable]
public class TileData
{
    public Vector2Int Coordinates;
    public TileType Tiletype;
}

public class Tile : MonoBehaviour {
    public TileData TileData;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(int x, int y, TileType type)
    {
        TileData.Coordinates = new Vector2Int(x, y);
        ChangeTileType(type);
        gameObject.name = "Tile {" + x + "},{" + y + "}";
    }

    public void PlayTurn()
    {
        switch (TileData.Tiletype)
        {
            case (TileType.Tree):
                TryExpanding();
                break;
            default:
                return;              
        }
    }

    public void TryExpanding()
    {
        int _destinationX = TileData.Coordinates.x + UnityEngine.Random.Range(-1, 1);
        int _destinationY = TileData.Coordinates.y + UnityEngine.Random.Range(-1, 1);

        if (Board.IsIndexInBoard(_destinationX,_destinationY))
        {
            FailGrow();
        }

        Board.Singleton.AllTiles[_destinationX, _destinationY].TryGrowTree();

    }

    public void FailGrow()
    {
        //ToDo - insert failing animation
    }

    public bool TryGrowTree()
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (Board.IsIndexInBoard(i, j))
                {
                    continue;
                }

                if (Board.Singleton.AllTiles[i,j].TileData.Tiletype == TileType.Building)
                {
                    FailGrow();
                    return false;
                }
            }
        }

        return true;
    }

    public void ChangeTileType(TileType newType)
    {
        TileData.Tiletype = newType;
    }
    
}
