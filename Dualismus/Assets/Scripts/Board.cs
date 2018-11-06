using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {


    public float BoardSize = 6f;
    private float _tileSize;
    public int TileCountX = 8;
    public int TileCountY = 8;
    public int TreesStartingNumber = 5;

    public static Board Singleton;

    public Tile[,] AllTiles;

    public Tile TilePrefab;

    private void Awake()
    {
        _tileSize = BoardSize / TileCountX;
        Singleton = this;
        AllTiles = new Tile[TileCountX , TileCountY];
        SetUpBoard();
    }

    // Use this for initialization
    void Start () {
		
	}
	
    public static bool IsIndexInBoard(int x, int y)
    {
        if (x < 0 || y < 0 || y >= Singleton.TileCountY || x >= Singleton.TileCountY)
        {
            return false;
        }

        return true;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(
            x * _tileSize - BoardSize / 2 + _tileSize / 2,
            BoardSize / 2 - y * _tileSize - _tileSize / 2);
    }

    private void SetUpBoard()
    {
        for (int i = 0; i<TileCountX; i++)
        {
            for (int j = 0; j < TileCountY; j++)
            {
                AllTiles[i, j] = Instantiate(TilePrefab, transform);
                AllTiles[i, j].Init(i, j, TileType.Clear);
                if (i == j)
                {
                    AllTiles[i, j].ChangeTileType(TileType.Tree);
                }
                AllTiles[i, j].transform.position = GetWorldPosition(i,j);
            }
        }

        for (int i = 0; i < TreesStartingNumber; i++)
        {
            int x = 0;
            int y = 0;
            do
            {
                x = Random.Range(0, TileCountX);
                y = Random.Range(0, TileCountY);
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
