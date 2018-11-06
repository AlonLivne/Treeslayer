using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    public int BoardSizeX;
    public int BoardSizeY;
    public int TreesStartingNumber = 5;

    public static Board Singleton;

    public Tile[,] AllTiles;

    public Tile TilePrefab;

    private void Awake()
    {
        Singleton = this;
        AllTiles = new Tile[BoardSizeX , BoardSizeY];
        SetUpBoard();
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
                AllTiles[i, j] = Instantiate(TilePrefab, transform);
                AllTiles[i, j].Init(i, j, TileType.Clear);
            }
        }

        for (int i = 0; i < TreesStartingNumber; i++)
        {
            int x = 0;
            int y = 0;
            do
            {
                x = Random.Range(0, BoardSizeX);
                y = Random.Range(0, BoardSizeY);
            }
            while (AllTiles[x, y].TileData.Tiletype != TileType.Clear);

            AllTiles[x, y].TileData.Tiletype = TileType.Tree;
            AllTiles[x, y].name = "Tree" + AllTiles[x, y].name;
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
