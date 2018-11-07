using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnState
{
    PlayerChooseTree,
    PlayerChooseRowOrColoumn,
    PlayerApprove,
    PlayerChoosePlaceForBuilding,
    TreesTurn
}

public class TurnManager : MonoBehaviour {
    public static TurnManager Singleton;
    public TurnState TurnState;
    private Tile _chosenTile;
    public BuildingToPlace BuilduingToPlacePrefab;
    private BuildingToPlace _buildingToPlace;
    public float NightTime = 5f;
    private float _nightTimer;
    public int MaxTurns;
    private int _turn = 0;
    public GameObject Arrows;


    private void Awake()
    {
        Singleton = this;
        _nightTimer = NightTime;
    }

    // Use this for initialization
    void Start()
    {
        TurnState = TurnState.PlayerChooseTree;
    }

    public void TileClicked(Tile tile)
    {
        _chosenTile = tile;
        switch (TurnState)
        {
            case (TurnState.PlayerChooseTree):
                if (tile.TileData.Tiletype == TileType.Tree)
                {
                    TurnState = TurnState.PlayerChooseRowOrColoumn;
                    Arrows.SetActive(true);
                    Arrows.transform.position =
                        tile.transform.position + 
                        new Vector3(0,0,-1);
                }
                break;

            case (TurnState.PlayerChoosePlaceForBuilding):
                if (IsLegalPlaceForBuilding())
                {
                    TurnState = TurnState.TreesTurn;
                    _buildingToPlace.PlaceBuildings();
                    Destroy(_buildingToPlace.gameObject);
                    Board.Singleton.EndTurn();
                    
                }

                break;

            default:
                break;
        }
    }

    public void RowOrColoumn(bool isHorizontal)
    {

        Arrows.SetActive(false);
        switch (TurnState)
        { 
            case (TurnState.PlayerChooseRowOrColoumn):
                var citySize = Board.Singleton.CutTrees(_chosenTile, isHorizontal);
                PlayerChoosePlaceForBuilding(citySize);

                break;

            case (TurnState.PlayerChoosePlaceForBuilding):
                _buildingToPlace.ChangeHorizontal(isHorizontal);
                break;

            default:
                break;
        }
    }

    public void PlayerChoosePlaceForBuilding(int size)
    {
        TurnState = TurnState.PlayerChoosePlaceForBuilding;
        _buildingToPlace = Instantiate(BuilduingToPlacePrefab);
        _buildingToPlace.Init(size);

    }

    public bool IsLegalPlaceForBuilding()
    {
        return _buildingToPlace.IsLegalForBuildings();
    }

    public void FinishGame()
    {

    }

    private void Update()
    {
        if(TurnState == TurnState.TreesTurn)
        {
            _nightTimer -= Time.deltaTime;
            if (_nightTimer < 0)
            {
                _turn++;
                if (_turn >= MaxTurns)
                {
                    FinishGame();
                    return;
                }
                TurnState = TurnState.PlayerChooseTree;
                _nightTimer = NightTime;
            }
        }
    }
}
