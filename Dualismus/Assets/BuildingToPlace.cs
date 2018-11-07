using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingToPlace : MonoBehaviour {
    private List<GameObject> Buildings;
    public GameObject BuildingPrefab;
    public Transform Holder;
    private float _moveSpeed = 0.5f;
    private bool _isHorizontal;

    public void Init(int size, bool isHorizontal = true)
    {
        for (int i = 0; i < size; i++)
        {
            var building = Instantiate(BuildingPrefab, Holder);
            Buildings.Add(building);
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

    private void MoveToCloseTile()
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

        Holder.transform.position = closest.transform.position;
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
