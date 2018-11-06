using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    int BoardSizeX;
    int BoardSizeY;
    public static Board Singleton;

    public Tile[,] AllTiles;

    public Tile TilePrefab;

    private void Awake()
    {
        Singleton = this;
        AllTiles = new Tile[BoardSizeX , BoardSizeY];
    }

    // Use this for initialization
    void Start () {
		
	}
	
    public static bool IsIndexInBoard(int x, int y)
    {
        if (x < 0 || y < 0 || y >= Singleton.BoardSizeY || x >= Singleton.BoardSizeY)
        {
            return false;
        }

        return true;
    }

    private void SetUpBoard()
    {
        for (int i = 0; i<BoardSizeX; i++)
        {
            for (int j = 0; j < BoardSizeY; j++)
            {
                AllTiles[i, j] = Instantiate(TilePrefab, this.transform);
                AllTiles[i, j].Init(i, j, TileType.Clear);
            }
        }
    }

    public void EndTurn()
    {
        var treeTiles = new List<Tile>();
        foreach (var tile in AllTiles)
        {
            if (tile.TileData.Tiletype == TileType.Tree)
            {
                treeTiles.Add(tile);
            }
        }

        foreach (var tree in treeTiles)
        {
            tree.PlayTurn();
        }
    }

	// Update is called once per frame
	void Update () {
		
	}


}
