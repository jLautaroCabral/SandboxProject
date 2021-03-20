using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionManager : MonoBehaviour
{
    public static PlayerSelectionManager me;

    public RectTransform selectionBox;

    public SelectingMode selectignMode;
    public GameObject buildingPlaceCursor;
    public List<GameObject> currentlySelected;

    bool drawMultiSelectBox;
    Player player;

    // Selection
    Vector2 startPosition;
    Vector3 endPosition;

    Grid grid;
    Camera cam;

    void Awake()
    {
        grid = GetComponent<Grid>();
        me = this;
        cam = Camera.main;
        player = GetComponent<Player>();
        currentlySelected = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        switch(selectignMode)
        {
            case (SelectingMode.units):
                Units_HandleSelection();
                break;
            case (SelectingMode.buildings):
                Buildings_HableSelection();
                break;
            default:
                Units_HandleSelection();
                break;
        }
    }

    /////// REGION
    #region Units
    void Units_HandleSelection()
    {
        LayerMask mask = LayerMask.GetMask("Default");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {

           // if (Input.GetKey(KeyCode.LeftControl))
                Units_MultiSelection(hit);
           // else
               // Units_SingleSelection(hit);
        }
    }

    private void Units_SingleSelection(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0))
        {
            try
            {
                GameObject hitObj = hit.collider.gameObject;
                if (hitObj.tag == "Unit")
                {
                    Debug.Log(hitObj.name);
                    setSelected(hitObj);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("No valid object selected: " + ex );
            }
        }
    }

    private void Units_MultiSelection(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0))
        {
            ToggleSelectionVisual(false);

            currentlySelected = new List<GameObject>();

            TrySelect(Input.mousePosition);
            startPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseSelectionBox();
        }
    }

    private void TrySelect(Vector3 screenPos)
    {
        LayerMask unitLayerMask = LayerMask.GetMask("UnitMask");
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, unitLayerMask))
        {
            Unit unit = hit.collider.GetComponent<Unit>();

            if(player.IsMyUnit(unit.gameObject))
            {
                currentlySelected.Add(unit.gameObject);
                unit.ToggleSelectionVisual(true);
            }
        }
    }

    void UpdateSelectionBox(Vector2 mousePos) {
        if (!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);

        float width = mousePos.x - startPosition.x;
        float height = mousePos.y - startPosition.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPosition + new Vector2(width / 2, height / 2);
    
    }

    void ReleaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);

        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        foreach(GameObject unit in player.units)
        {
            Debug.Log("Revisando unidad");

            Vector3 screenPos = cam.WorldToScreenPoint(unit.transform.position);

            Debug.Log(screenPos);

            if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                currentlySelected.Add(unit);
                unit.GetComponent<Unit>().ToggleSelectionVisual(true);
            }
        }

    }


    private void ToggleSelectionVisual(bool selected)
    {
        foreach(GameObject unit in currentlySelected)
        {
            unit.GetComponent<Unit>().ToggleSelectionVisual(selected);
        }
    }
    #endregion Units

    private void setSelected(GameObject hitObj)
    {
        // Seleccionar unidades
    }

    void Buildings_HableSelection()
    {

    }
}

public enum SelectingMode
{
    tiles,
    units,
    creatingBuildings,
    buildings
}
