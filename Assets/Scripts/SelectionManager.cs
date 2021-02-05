using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager sharedInstance; // shaI

    bool drawMultiSelectBox = false;

    [SerializeField] List<GameObject> currentlySelected;
    [SerializeField] TileMasterClass[] objectsWithSuperClass; // Just for demostrateting how the super clas can access the classes that inherit it
    GameObject firstTileSelected = null;
    GameObject lastTileSelected = null;
    private void Awake()
    {
        if (sharedInstance == null) sharedInstance = this;
        findAllSuperClassExamples();
    }

    // Update is called once per frame
    void Update()
    {
        checkForLeftMouseClick();
    }
    public void setSelected(GameObject toSet) // setter for selected obj
    {
        clearSelected();
        currentlySelected.Add(toSet);
        toSet.GetComponent<TileMasterClass>().OnSelect();
    }

    public void setSelected(List<GameObject> toSet, bool clearExisting)
    {
        if(clearExisting)
        {
            clearSelected();
        }
        currentlySelected = toSet;
        foreach(GameObject obj in getSelected())
        {
            obj.GetComponent<TileMasterClass>().OnSelect();
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

        foreach(GameObject obj in getSelected())
        {
            obj.GetComponent<TileMasterClass>().OnDeselect();
        }
        currentlySelected = new List<GameObject>();
    }

    
    private void checkForLeftMouseClick()
    {
        if(Input.GetKey(KeyCode.LeftControl))
        {
            if(Input.GetMouseButtonDown(0))
            {
                Debug.Log("Multiselect");
                if(firstTileSelected == null)
                {
                    Debug.Log("Selecting first tile");
                    selectionRaycast(ref firstTileSelected);
                } else if(firstTileSelected != null && lastTileSelected != null)
                {
                    Vector2 starCoors = firstTileSelected.GetComponent<TileMasterClass>().getGridCoors();
                    Vector2 endCoors = lastTileSelected.GetComponent<TileMasterClass>().getGridCoors();
                    Debug.Log("Start " + starCoors);
                    Debug.Log("End " + endCoors);

                    if(firstTileSelected != null && lastTileSelected != null)
                    {
                        List<GameObject> selectedTiles = GridGenerator.sharedInstance.getTiles(starCoors, endCoors);
                        setSelected(selectedTiles, true);
                        firstTileSelected = null;
                        lastTileSelected = null;
                    }
                }
            }

            if(!Input.GetMouseButton(0) && firstTileSelected != null)
            {
                Debug.Log("Selecting second title");
                drawMultiSelectBox = true;
                selectionRaycast(ref lastTileSelected);
            } else
            {
                drawMultiSelectBox = false;
            }

        } else if(Input.GetMouseButtonDown(0))
        {
            firstTileSelected = null;
            lastTileSelected = null;
            Debug.Log("Clicking, looking for raycast hits");
            selectionRaycast();
        } else
        {
            drawMultiSelectBox = false;
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
        } catch (Exception ex)
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
        } catch
        {
            Debug.Log("No valid obj selected");
        }
    }

    private void findAllSuperClassExamples()
    {
        objectsWithSuperClass = FindObjectsOfType<TileMasterClass>();
    }

    private void OnGUI()
    {
        if(drawMultiSelectBox)
        {
            Vector3 startScreenPos = Camera.main.WorldToScreenPoint(firstTileSelected.transform.position);
            Vector3 endScreenPos = Input.mousePosition;

            float width, height;
            if(startScreenPos.x > endScreenPos.x)
            {
                width = startScreenPos.x - endScreenPos.x;
            } else
            {
                width = endScreenPos.x - startScreenPos.x;
            }

            if(startScreenPos.y > endScreenPos.y)
            {
                height = startScreenPos.y - endScreenPos.y;
            } else
            {
                height = endScreenPos.y - startScreenPos.y;
            }

            Rect posToDrawBox;

            if(endScreenPos.x > startScreenPos.x)
            {
                if (endScreenPos.y > startScreenPos.y)
                    posToDrawBox = new Rect(startScreenPos.x, Screen.height - endScreenPos.y, width, height);
                else
                    posToDrawBox = new Rect(startScreenPos.x, Screen.height - startScreenPos.y, width, height);
            } else
            {
                if (endScreenPos.y > startScreenPos.y)
                    posToDrawBox = new Rect(endScreenPos.x, Screen.height - endScreenPos.y, width, height);
                else
                    posToDrawBox = new Rect(endScreenPos.x, Screen.height - startScreenPos.y, width, height);
            }

            GUI.DrawTexture(posToDrawBox, GUIManager.sharedInstance.getBlackBoxSemiTrans());
        }
    }
}
