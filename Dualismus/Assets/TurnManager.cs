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

    private void Awake()
    {
        Singleton = this;
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
                }
                break;

            case (TurnState.PlayerChoosePlaceForBuilding):
                if (tile.TileData.Tiletype == TileType.Clear)
                {

                    if (IsLegalPlaceForBuilding(_chosenTile))
                    {
                        
                        
                    }

                    TurnState = TurnState.TreesTurn;
                }
                break;

            default:
                break;
        }
    }

    public void RowOrColoumn(bool isHorizontal)
    {

        switch (TurnState)
        { 
            case (TurnState.PlayerChooseRowOrColoumn):
                var citySize = Board.Singleton.CutTrees(_chosenTile, isHorizontal);
                PlayerChoosePlaceForBuilding(citySize);

                break;

            case (TurnState.PlayerChoosePlaceForBuilding):
                _buildingToPlace.SyncVisual(isHorizontal);
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

    public bool IsLegalPlaceForBuilding(Tile tile)
    {
        return true;
    }	
}
