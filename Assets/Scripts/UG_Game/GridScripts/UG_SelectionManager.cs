using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectionMode
{
    tile,
    unit,
    creatingBuilding,
    building
}

public class UG_SelectionManager : MonoBehaviour
{
    public static UG_SelectionManager sharedInstance; // shaI

    public SelectionMode selectionMode;
    public GameObject buildingPlaceCursor;

    [SerializeField]bool drawMultiSelectBox = false;

    [SerializeField] List<GameObject> currentlySelected;

    GameObject firstTileSelected = null;
    GameObject lastTileSelected = null;

    [HideInInspector]public Vector3 startPosition, startWorld;
    [HideInInspector]public Vector3 endPosition, endWorld;

    private void Awake()
    {
        if (sharedInstance == null) sharedInstance = this;
        currentlySelected = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(selectionMode == SelectionMode.tile)
        {
            Tiles_checkForMouseClick();
        } else if(selectionMode == SelectionMode.creatingBuilding)
        {
            CreateBuildings_checkForMouseClick();
        } else if(selectionMode == SelectionMode.unit)
        {
            Unit_checkForMouseClick();
        }
        shouldWeDisplayBuildingConstrucCursor();
    }

    //////////////// REGION //////////////
    #region Building
    private void shouldWeDisplayBuildingConstrucCursor()
    {
        if(selectionMode == SelectionMode.creatingBuilding)
        {
            buildingPlaceCursor.SetActive(true);
        } else
        {
            buildingPlaceCursor.SetActive(false);
        }
    }

    private void CreateBuildings_checkForMouseClick()
    {
        GameObject selectingBuilding = UG_BuildingStore.sharedInstance.getToBuild();

        if (selectingBuilding != null)
        {
            UG_Building selectedBuildingScript = selectingBuilding.GetComponent<UG_Building>();

            getMultipleTilesFromCoords(selectedBuildingScript.widthInTiles, selectedBuildingScript.heightInTiles);

            buildingPlaceCursor.GetComponent<SpriteRenderer>().sprite = selectedBuildingScript.buildingSprite;
            buildingPlaceCursor.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);

            //buildingPlaceCursor.GetComponent<SpriteRenderer>().color = cursorColor;
            if (isCurrentLocAValidForConstruction())
            {
                buildingPlaceCursor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f); // Normal
            }
            else
            {
                buildingPlaceCursor.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f); // Red
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (isCurrentLocAValidForConstruction())
                {
                    createBuildingAtLocation(buildingPlaceCursor.transform.position, selectedBuildingScript.widthInTiles, selectedBuildingScript.heightInTiles);
                }
            }
        }
    }

    // Si hay un tile no walkable dentro de la construccion o al rededor de lla devuelve falso
    private bool isCurrentLocAValidForConstruction()
    {
        if (currentlySelected.Count <= 0)
        {
            return false;
        }

        foreach (GameObject tile in currentlySelected)
        {
            UG_TileMasterClass tm = tile.GetComponent<UG_TileMasterClass>();
            if (!tm.isTileWalkeable())
            {
                return false;
            }
        }

        int xLowBound = (int)currentlySelected[0].GetComponent<UG_TileMasterClass>().getGridCoors().x;
        int xHighBound = (int)currentlySelected[currentlySelected.Count - 1].GetComponent<UG_TileMasterClass>().getGridCoors().x;
        int yLowBound = (int)currentlySelected[0].GetComponent<UG_TileMasterClass>().getGridCoors().y;
        int yHighBound = (int)currentlySelected[currentlySelected.Count - 1].GetComponent<UG_TileMasterClass>().getGridCoors().y;

        for (int x = 0; x < currentlySelected.Count - 1; x++)
        {
            GameObject tile = currentlySelected[x];
            UG_TileMasterClass tm = tile.GetComponent<UG_TileMasterClass>();

            Vector2 curTileGrid = tm.getGridCoors();

            if (curTileGrid.x == xLowBound || curTileGrid.x == xHighBound)
                if (curTileGrid.y == yLowBound || curTileGrid.y == yHighBound)
                    if (!tm.isTileWalkeable())
                        return false;

            if (curTileGrid.y == yLowBound || curTileGrid.y == yHighBound)
                if (curTileGrid.x == xLowBound || curTileGrid.x == xHighBound)
                    if (!tm.isTileWalkeable())
                        return false;
        }

        return true;
    }

    private void getMultipleTilesFromCoords(int width, int height)
    {
        try
        {
            width += 2;
            height += 2;
            GameObject tileAtMousePoint = null;
            selectionRaycast(ref tileAtMousePoint);

            UG_TileMasterClass tm = tileAtMousePoint.GetComponent<UG_TileMasterClass>();
            Vector2 tileGridCoors = tm.getGridCoors();

            if (isSelectionInGridRange(tileGridCoors, width, height))
            {
                Debug.Log("Enough Space");
                Vector2 starPos = new Vector2(tileGridCoors.x - (width / 2), tileGridCoors.y - (height / 2));
                Vector2 endPos = new Vector2(tileGridCoors.x + (width / 2), tileGridCoors.y + (height / 2));
                setSelected(UG_GridGenerator.sharedInstance.getTiles(starPos, endPos), true);
            }
            else
            {
                clearSelected();
                Debug.Log("Not enough space");
            }
        }
        catch
        {
            Debug.Log("No tite at mouse position");
        }
    }

    private bool isSelectionInGridRange(Vector2 tileGridCoors, int width, int height)
    {
        width /= 2;
        height /= 2;
        if (
            (tileGridCoors.x - width) < 0
            || (tileGridCoors.y - height) < 0
            || (tileGridCoors.x + width) >= UG_GridGenerator.sharedInstance.gridDimensions.x
            || (tileGridCoors.y + height) >= UG_GridGenerator.sharedInstance.gridDimensions.y
          )
        {
            return false;
        }

        return true;
    }

    void createBuildingAtLocation(Vector3 cursorPos, int width, int height)
    {
        int xLowBound = (int)currentlySelected[0].GetComponent<UG_TileMasterClass>().getGridCoors().x;
        int xHighBound = (int)currentlySelected[currentlySelected.Count - 1].GetComponent<UG_TileMasterClass>().getGridCoors().x;
        int yLowBound = (int)currentlySelected[0].GetComponent<UG_TileMasterClass>().getGridCoors().y;
        int yHighBound = (int)currentlySelected[currentlySelected.Count - 1].GetComponent<UG_TileMasterClass>().getGridCoors().y;

        for (int x = 0; x < currentlySelected.Count - 1; x++)
        {
            GameObject tile = currentlySelected[x];
            UG_TileMasterClass tm = tile.GetComponent<UG_TileMasterClass>();

            Vector2 curTileGrid = tm.getGridCoors();
            if (curTileGrid.x == xLowBound || curTileGrid.x == xHighBound)
            {
                if (curTileGrid.y == yLowBound || curTileGrid.y == yHighBound)
                    Debug.Log("Keeping " + tile.name + " walkable");
            }
            else if (curTileGrid.y == yLowBound || curTileGrid.y == yHighBound)
            {
                if (curTileGrid.x == xLowBound || curTileGrid.x == xHighBound)
                    Debug.Log("Keeping " + tile.name + " walkable");
            }
            else
            {
                tm.setTileWalkeable(false);
            }
        }

        Vector3 spawnPos = currentlySelected[(currentlySelected.Count - 1) / 2].transform.position; // Get tile on center of building
        GameObject built = (GameObject)Instantiate(UG_BuildingStore.sharedInstance.getToBuild(), spawnPos, Quaternion.Euler(0, 0, 0));
        SpriteRenderer sr = built.AddComponent<SpriteRenderer>();
        sr.sprite = built.GetComponent<UG_Building>().buildingSprite;
        sr.sortingOrder = 10;
        built.SetActive(true);
        clearSelected();
    }

    #endregion Building

    //////////////// REGION //////////////
    #region Other

    private void Tiles_checkForMouseClick()
    {
        if (Input.GetKey(KeyCode.LeftControl)) // Multiselect
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (firstTileSelected == null)
                {
                    selectionRaycast(ref firstTileSelected);
                }
                else if (firstTileSelected != null && lastTileSelected != null)
                {
                    Vector2 starCoors = firstTileSelected.GetComponent<UG_TileMasterClass>().getGridCoors();
                    Vector2 endCoors = lastTileSelected.GetComponent<UG_TileMasterClass>().getGridCoors();

                    if (firstTileSelected != null && lastTileSelected != null)
                    {
                        List<GameObject> selectedTiles = UG_GridGenerator.sharedInstance.getTiles(starCoors, endCoors);
                        setSelected(selectedTiles, true);

                        replaceTiles();

                        firstTileSelected = null;
                        lastTileSelected = null;
                    }
                }
            }

            if (!Input.GetMouseButton(0) && firstTileSelected != null)
            {
                Debug.Log("Selecting second title");
                drawMultiSelectBox = true;
                selectionRaycast(ref lastTileSelected);
            }
            else
            {
                drawMultiSelectBox = false;
            }

        }
        else if (Input.GetMouseButtonDown(0)) // Individual select
        {
            firstTileSelected = null;
            lastTileSelected = null;
            selectionRaycast();
            replaceTiles();
        }
        else
        {
            drawMultiSelectBox = false;
        }
    }

    void Unit_checkForMouseClick()
    {
        if(Input.GetKey(KeyCode.LeftControl) == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                RaycastHit2D raycast = Physics2D.Raycast(mousePos, Vector2.zero, 0f);

                try
                {
                    GameObject hitObj = raycast.collider.gameObject;
                    if (hitObj.tag == "Unit")
                    {
                        Debug.Log(hitObj.name);
                        setSelected(hitObj);
                    }

                }
                catch
                {
                    Debug.Log("No valid object selected");
                }
            }
        } else
        {
            if(Input.GetMouseButtonDown(0))
            {
                startPosition = Input.mousePosition;
                startWorld = Camera.main.ScreenToWorldPoint(startPosition);
            }

            if(Input.GetMouseButton(0))
            {
                drawMultiSelectBox = true;
                endPosition = Input.mousePosition;
                endWorld = Camera.main.ScreenToWorldPoint(endPosition);
            }

            if(Input.GetMouseButtonUp(0))
            {
                drawMultiSelectBox = false;
                setSelected(UG_UnitManager.sharedInstance.getUnitsWithinArea(startWorld, endWorld), true);
                startWorld = Vector3.zero;
                endWorld = Vector3.zero;
                endPosition = Vector3.zero;
                startPosition = Vector3.zero;
            }
        }

    }
    public void setSelected(GameObject toSet) // setter for selected obj
    {
        clearSelected();
        currentlySelected.Add(toSet);
        toSet.GetComponent<UG_TileMasterClass>().OnSelect();
    }

    public void setSelected(List<GameObject> toSet, bool clearExisting)
    {
        if (clearExisting)
        {
            clearSelected();
        }
        currentlySelected = toSet;
        foreach (GameObject obj in getSelected())
        {
            if(obj.GetComponent<UG_TileMasterClass>())
            {
                obj.GetComponent<UG_TileMasterClass>().OnSelect();
            }
        } 
    }

    public List<GameObject> getSelected()
    {
        return currentlySelected;
    }

    void clearSelected()
    {
        firstTileSelected = null;
        lastTileSelected = null;

        foreach (GameObject obj in getSelected())
        {
            if(obj.GetComponent<UG_TileMasterClass>())
            {
                obj.GetComponent<UG_TileMasterClass>().OnDeselect();
            }
        }
        currentlySelected = new List<GameObject>();
    }

    void replaceTiles()
    {
        if(UG_TileChangeManager.sharedInstance.getSelectedTile() != null)
        {
            UG_GridGenerator.sharedInstance.ChangeTilesInGrid(currentlySelected);
            clearSelected();
        }
    }

    bool areWeMultiSelecting()
    {
        return Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl);
    }

    private void selectionRaycast()
    {
        Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D raycastHit = Physics2D.Raycast(mousePos, Vector2.zero, 0f); // Physics2D.Raycast(cameraVec2, vec2Dir)
        try
        {
            GameObject hitObj = raycastHit.collider.gameObject;
            Debug.Log(hitObj.name);
            setSelected(hitObj);
        }
        catch (Exception ex)
        {
            Debug.Log("No valid obj selected " + ex.ToString());
        }
    }
    private void selectionRaycast(ref GameObject objToSet)
    {
        Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D raycastHit = Physics2D.Raycast(mousePos, Vector2.zero, 0f); // Physics2D.Raycast(cameraVec2, vec2Dir)
        try
        {
            GameObject hitObj = raycastHit.collider.gameObject;
            Debug.Log(hitObj.name);
            objToSet = hitObj;
        }
        catch
        {
            Debug.Log("No valid obj selected");
        }
    }

    private void OnGUI()
    {
        if (selectionMode == SelectionMode.tile)
        {
            if (drawMultiSelectBox)
            {
                Vector3 startScreenPos = Camera.main.WorldToScreenPoint(firstTileSelected.transform.position);
                Vector3 endScreenPos = Input.mousePosition;

                float width, height;
                if (startScreenPos.x > endScreenPos.x)
                    width = startScreenPos.x - endScreenPos.x;
                else
                    width = endScreenPos.x - startScreenPos.x;

                if (startScreenPos.y > endScreenPos.y)
                    height = startScreenPos.y - endScreenPos.y;
                else
                    height = endScreenPos.y - startScreenPos.y;

                Rect posToDrawBox;

                if (endScreenPos.x > startScreenPos.x)
                {
                    if (endScreenPos.y > startScreenPos.y)
                        posToDrawBox = new Rect(startScreenPos.x, Screen.height - endScreenPos.y, width, height);
                    else
                        posToDrawBox = new Rect(startScreenPos.x, Screen.height - startScreenPos.y, width, height);
                }
                else
                {
                    if (endScreenPos.y > startScreenPos.y)
                        posToDrawBox = new Rect(endScreenPos.x, Screen.height - endScreenPos.y, width, height);
                    else
                        posToDrawBox = new Rect(endScreenPos.x, Screen.height - startScreenPos.y, width, height);
                }
                GUI.DrawTexture(posToDrawBox, UG_GUIManager.sharedInstance.getBlackBoxSemiTrans());
            }
        } else if (selectionMode == SelectionMode.unit)
        {
            if (drawMultiSelectBox)
            {
                Vector3 startScreenPos = startPosition;
                Vector3 endScreenPos = Input.mousePosition;

                float width, height;
                if (startScreenPos.x > endScreenPos.x)
                    width = startScreenPos.x - endScreenPos.x;
                else
                    width = endScreenPos.x - startScreenPos.x;

                if (startScreenPos.y > endScreenPos.y)
                    height = startScreenPos.y - endScreenPos.y;
                else
                    height = endScreenPos.y - startScreenPos.y;

                Rect posToDrawBox;

                if (endScreenPos.x > startScreenPos.x)
                {
                    if (endScreenPos.y > startScreenPos.y)
                        posToDrawBox = new Rect(startScreenPos.x, Screen.height - endScreenPos.y, width, height);
                    else
                        posToDrawBox = new Rect(startScreenPos.x, Screen.height - startScreenPos.y, width, height);
                }
                else
                {
                    if (endScreenPos.y > startScreenPos.y)
                        posToDrawBox = new Rect(endScreenPos.x, Screen.height - endScreenPos.y, width, height);
                    else
                        posToDrawBox = new Rect(endScreenPos.x, Screen.height - startScreenPos.y, width, height);
                }
                GUI.DrawTexture(posToDrawBox, UG_GUIManager.sharedInstance.getBlackBoxSemiTrans());
            }
        }
    }

    #endregion Other
}


