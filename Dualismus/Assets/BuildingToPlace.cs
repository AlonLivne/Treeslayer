using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingToPlace : MonoBehaviour {
    private List<GameObject> Buildings;
    public GameObject BuildingPrefab;
    public Transform Holder;
    private float _moveSpeed = 0.5f;
    public bool IsHorizontal;

    public void Init(int size, bool isHorizontal = true, float scale = 5)
    {
        IsHorizontal = isHorizontal;

        for (int i = 0; i < size; i++)
        {
            var building = Instantiate(BuildingPrefab, Holder);
            Buildings.Add(building);
            Buildings[i].transform.localScale = new Vector3(scale, scale, 1);
        }

        SyncVisual(isHorizontal);
    }

    public void SyncVisual(bool isHorizontal)
    {
        var size = 2f;
        for (int i = 0; i < Buildings.Count; i++)
        {
            var x = Buildings[i].transform.position.x;
            var y = Buildings[i].transform.localScale.y * i * size;
            var z = 0;

            if (IsHorizontal)
            {
                y = Buildings[i].transform.position.y;
                x = Buildings[i].transform.localScale.x * i * size;
            }
            Debug.Log(isHorizontal);
            Buildings[i].transform.position = new Vector3(x, y, z);
        }
    }

    private void Awake()
    {
        Buildings = new List<GameObject>();
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
	}
}
