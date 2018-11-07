using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingToPlace : MonoBehaviour {
    private List<GameObject> Buildings;
    public GameObject BuildingPrefab;
    public Transform Holder;
    public GameObject BuildingVisual;
    private float _moveSpeed = 0.5f;
    private bool _isHorizontal;

    public void Init(int size, bool isHorizontal = true)
    {
        for (int i = 0; i < size; i++)
        {
            BuildingVisual = Instantiate(VisualsMediator.Singleton.GetVisual(Visuals.Buildings), Holder);
            Buildings.Add(BuildingVisual);
        }

        ChangeHorizontal(isHorizontal);
    }

    public void ChangeHorizontal(bool isHorizontal)
    {
        _isHorizontal = isHorizontal;
        SyncVisual();
    }

    public void SyncVisual()
    {
        var size = 2f;
        for (int i = 0; i < Buildings.Count; i++)
        {
            float x = 0;
            float y = 0;

            if (_isHorizontal)
            {
                x = i * Board.Singleton.GetTileSize();
            }
            else
            {
                y = - i * Board.Singleton.GetTileSize();
            }

            Buildings[i].transform.position = 
                Holder.transform.position + new Vector3(x, y, 0);

        }
    }

    private void Awake()
    {
        Buildings = new List<GameObject>();
    }

    public Tile GetClosest()
    {
        Tile closest = null;
        for (int i = 0; i < Board.Singleton.TileCountX; i++)
        {
            for (int j = 0; j < Board.Singleton.TileCountY; j++)
            {
                var tile = Board.Singleton.AllTiles[i, j];
                if (tile == null)
                {
                    continue;
                }

                if (closest == null)
                {
                    closest = tile;
                    continue;
                }

                if (Vector3.Distance(tile.transform.position, transform.position) <
                    Vector3.Distance(closest.transform.position, transform.position))
                {
                    closest = tile;
                    continue;
                }
            }
        }

        return closest;
    }

    private void MoveToCloseTile()
    {
        var closest = GetClosest();

        Holder.transform.position = closest.transform.position;
        for (int i = 0; i < Buildings.Count; i++)
        {
            Color color = IsLegalForBuilding(closest, i) ? Color.white : Color.red;
            Buildings[i].GetComponent<SpriteRenderer>().color = color;
            var sortingOrder = closest.TileData.Y * 10 + 1;
            if (!_isHorizontal)
            {
                sortingOrder += i * 10;
            }
            Buildings[i].GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
        }

    }

    public bool IsLegalForBuildings()
    {
        var closest = GetClosest();
        for (int i = 0; i < Buildings.Count; i++)
        {
            if (!IsLegalForBuilding(closest, i))
            {
                return false;
            }
        }

        return true;
    }

    public bool IsLegalForBuilding(Tile closest, int index)
    {
        var x = closest.TileData.X;
        var y = closest.TileData.Y;

        if (_isHorizontal)
        {
            x += index;
        }
        else
        {
            y += index;
        }

        var tile = Board.Singleton.GetTile(x, y);
        if (tile == null)
        {
            return false;
        }

        return tile.IsLegalForBuilding();

    }

    public void PlaceBuildings()
    {
        var closest = GetClosest();
        var x = closest.TileData.X;
        var y = closest.TileData.Y;

        for (int i = 0; i < Buildings.Count; i++)
        {
            var tile = Board.Singleton.GetTile(x, y);
            Buildings[i].transform.position = Buildings[i].transform.position =
                Holder.transform.position + new Vector3(0, 0, 0);
            tile.CityVisualPrefab = (Buildings[i]);
            tile.ChangeTileType(TileType.Building);

            if (_isHorizontal)
            {
                x += 1;
            }
            else
            {
                y += 1;
            }
        }

    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(TurnManager.Singleton.TurnState == TurnState.PlayerChoosePlaceForBuilding)
        {
            var mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePosition, _moveSpeed);
        }

	    MoveToCloseTile();
	}
}
