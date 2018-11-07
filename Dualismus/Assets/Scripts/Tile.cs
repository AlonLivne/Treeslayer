using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Clear,
    Tree,
    Building,
    CutThisTurn
}

[Serializable]
public class TileData
{
    public int X;
    public int Y;
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
        TileData.X = x;
        TileData.Y = y;
        ChangeTileType(type);
    }

    public void SyncVisual()
    {
        gameObject.name = string.Format("{0}Tile {1},{2}", TileData.Tiletype.ToString(), TileData.X, TileData.Y);

        foreach (Transform child in Holder.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (TileData.Tiletype == TileType.Tree)
        {
            var vis = Instantiate(TreeVisualPrefab, Holder);
            vis.GetComponent<SpriteRenderer>().sortingOrder = TileData.Y * 10;
            
        }
        else if (TileData.Tiletype == TileType.Building)
        {
            var vis = Instantiate(CityVisualPrefab, Holder);
            vis.GetComponent<SpriteRenderer>().sortingOrder = TileData.Y * 10;
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
            destinationX = TileData.X + UnityEngine.Random.Range(-1, 1);
            destinationY = TileData.Y + UnityEngine.Random.Range(-1, 1);
        }

        var tile = Board.Singleton.GetTile(destinationX, destinationY);
        if (tile == null
            || tile.TileData.Tiletype != TileType.Clear)
        {
            FailGrow();
            return;
        }

        tile.TryGrowTree();

    }

    public void OnMouseUp()
    {
        TurnManager.Singleton.TileClicked(this);
        Debug.Log(string.Format("{0} clicked", this.name.ToString()));

    }

    public void FailGrow()
    {
        //ToDo - insert failing animation
    }

    public bool IsNeighborBuilding()
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                var tile = Board.Singleton.GetTile(i, j);
                if (tile == null)
                {
                    continue;
                }

                if (tile.TileData.Tiletype == TileType.Building)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void TryGrowTree()
    {
        if (IsNeighborBuilding())
        {
            FailGrow();
            return;
        }

        GrowTree();
    }

    public void GrowTree()
    {
        ChangeTileType(TileType.Tree);
    }

    public void CutTree()
    {
        ChangeTileType(TileType.Clear);
    }

    public void BuildBuilding()
    {
        ChangeTileType(TileType.Building);
    }

    public void ChangeTileType(TileType newType)
    {
        TileData.Tiletype = newType;
        SyncVisual();
    }

    public bool IsLegalForBuilding()
    {
        if (TileData.Tiletype != TileType.Clear || IsNeighborBuilding())
        {
            return false;
        }

        return true;
    }
}
