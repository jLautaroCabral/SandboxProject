using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSelectionManager : MonoBehaviour
{
    public static PlayerSelectionManager me;

    public RectTransform selectionBox;
    public SelectingMode selectignMode;
    public List<GameObject> currentlySelected;

    TG_GameTest.Player player;
    Camera cam;
    Vector2 startPosition;


    void Awake()
    {
        me = this;
        cam = Camera.main;
        player = GetComponent<TG_GameTest.Player>();
        currentlySelected = new List<GameObject>();
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
    void Units_HandleSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ToggleSelectionVisual(false);

            currentlySelected = new List<GameObject>();

            TrySelect(Input.mousePosition);
            startPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
            UpdateSelectionBox(Input.mousePosition);

        if (Input.GetMouseButtonUp(0))
            ReleaseSelectionBox();

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
