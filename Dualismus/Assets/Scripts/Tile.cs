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

    public Transform Holder;
    public GameObject TreeVisualPrefab;
    public GameObject CityVisualPrefab;

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
    }

    public void SyncVisual()
    {
        gameObject.name = "Tile {" + TileData.Coordinates.x + "},{" + TileData.Coordinates.y + "}";

        foreach (Transform child in Holder.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (TileData.Tiletype == TileType.Tree)
        {
            Instantiate(TreeVisualPrefab, transform);
        }

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
        int destinationX = 0;
        int destinationY = 0;
        while (destinationX == 0 & destinationY == 0)
        {
            destinationX = TileData.Coordinates.x + UnityEngine.Random.Range(-1, 1);
            destinationY = TileData.Coordinates.y + UnityEngine.Random.Range(-1, 1);
        }

        if (Board.IsIndexInBoard(destinationX, destinationY))
        {
            FailGrow();
        }

        Board.Singleton.AllTiles[destinationX, destinationY].TryGrowTree();

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
        SyncVisual();
    }
    
}
