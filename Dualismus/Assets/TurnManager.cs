using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnState
{
    PlayerChoseTree,
    PlayerChoseRowOrColoumn,
    PlayerApprove,
    PlayerChosePlaceForBuilding,
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
        TurnState = TurnState.PlayerChoseTree;
    }

    public void TileClicked(Tile tile)
    {
        _chosenTile = tile;
        switch (TurnState)
        {
            case (TurnState.PlayerChoseTree):
                if (tile.TileData.Tiletype == TileType.Tree)
                {
                    TurnState = TurnState.PlayerChoseRowOrColoumn;
                }
                break;

            case (TurnState.PlayerChosePlaceForBuilding):
                if (tile.TileData.Tiletype == TileType.Clear)
                {

                    if (IsLegalPlaceForBuilding(_chosenTile))
                    {
                        //ToDo - place building
                        
                    }
                }
                break;

            default:
                break;
        }
    }

    public void KeyPressed(bool isHorizontal)
    {

        switch (TurnState)
        { 
            case (TurnState.PlayerChoseRowOrColoumn):
                var citySize = Board.Singleton.CutTrees(_chosenTile, isHorizontal);
                PlayerChoosePlaceForBuilding(citySize);

                break;

            case (TurnState.PlayerChosePlaceForBuilding):
                _buildingToPlace.SyncVisual(isHorizontal);
                break;

            default:
                break;
        }
    }

    public void PlayerChoosePlaceForBuilding(int size)
    {
        TurnState = TurnState.PlayerChosePlaceForBuilding;
        _buildingToPlace = Instantiate(BuilduingToPlacePrefab);
        _buildingToPlace.Init(size);

    }

    public bool IsLegalPlaceForBuilding(Tile tile)
    {
        return true;
    }	
}
