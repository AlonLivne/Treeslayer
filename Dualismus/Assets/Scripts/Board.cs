using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    public Transform TileHolder;
    public float BoardSize = 120f;
    private float _tileSize;
    public int TileCountX = 8;
    public int TileCountY = 8;
    public int TreesStartingNumber = 5;

    public static Board Singleton;

    public Tile[,] AllTiles;

    public Tile TilePrefab;

    public float GetTileSize()
    {
        return _tileSize;
    }

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
                AllTiles[i, j] = Instantiate(TilePrefab, TileHolder);
                AllTiles[i, j].transform.position = GetWorldPosition(i, j);
                AllTiles[i, j].Init(i, j, TileType.Clear);

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

            AllTiles[x, y].ChangeTileType(TileType.Tree);

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

            AllTiles[x, y].ChangeTileType(TileType.Building);
            
        }
    }

    public void EndTurn()
    {
        var treeTiles = new List<Tile>();
        var cutTiles = new List<Tile>();
        foreach (var tile in AllTiles)
        {
            if (tile.TileData.Tiletype == TileType.Tree)
            {
                treeTiles.Add(tile);
            }
            if (tile.TileData.Tiletype == TileType.CutThisTurn)
            {
                cutTiles.Add(tile);
            }
        }

        foreach (var cutTile in cutTiles)
        {
            cutTile.ChangeTileType(TileType.Clear);
        }

        foreach (var tree in treeTiles)
        {
            tree.PlayTurn();
        }
    }
    
    public Tile GetTile(int x, int y)
    {
        if (!IsIndexInBoard(x, y))
        {
            return null;
        }

        return AllTiles[x, y];
    }

    public int CutTrees(Tile tile, bool isHorizontal)
    {
        if (tile == null)
        {
            return 0;
        }

        Debug.Log("CutTrees");

        if (!IsIndexInBoard(tile.TileData.X, tile.TileData.Y) || tile.TileData.Tiletype != TileType.Tree)
        {
            return 0;
        }

        var positiveNeighborTile = GetTile(tile.TileData.X, tile.TileData.Y + 1);
        var negativeNeighborTile = GetTile(tile.TileData.X, tile.TileData.Y - 1);
        if (isHorizontal)
        {
            positiveNeighborTile = GetTile(tile.TileData.X + 1, tile.TileData.Y);
            negativeNeighborTile = GetTile(tile.TileData.X - 1, tile.TileData.Y);

        }
        int ans = 1;

        tile.ChangeTileType(TileType.CutThisTurn);

        ans += CutTreesNegativeDirection(negativeNeighborTile, isHorizontal);
        ans += CutTreesPositiveDirection(positiveNeighborTile, isHorizontal);
        return ans;
    }

    public int CutTreesPositiveDirection(Tile tile, bool isHorizontal)
    {
        if (tile == null)
        {
            return 0;
        }

        if (tile.TileData.Tiletype != TileType.Tree)
        {
            return 0;
        }

        var neighborTile = GetTile(tile.TileData.X, tile.TileData.Y + 1);
        if (isHorizontal)
        {
            neighborTile = GetTile(tile.TileData.X + 1, tile.TileData.Y);
        }
        int ans = 1;

        tile.ChangeTileType(TileType.CutThisTurn);

        ans += CutTrees(neighborTile, isHorizontal);
        return ans;

    }

    public int CutTreesNegativeDirection(Tile tile, bool isHorizontal)
    {
        if (tile == null)
        {
            return 0;
        }

        if (tile.TileData.Tiletype != TileType.Tree)
        {
            return 0;
        }

        var neighborTile = GetTile(tile.TileData.X, tile.TileData.Y - 1);
        if (isHorizontal)
        {
            neighborTile = GetTile(tile.TileData.X - 1, tile.TileData.Y);

        }
        int ans = 1;

        tile.ChangeTileType(TileType.CutThisTurn);

        ans += CutTrees(neighborTile, isHorizontal);
        return ans;
    }


    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Debug.Break();
        }
	}


}
