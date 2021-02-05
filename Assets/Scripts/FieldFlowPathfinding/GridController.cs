using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField curFlowField;
    public GridDebug gridDebug;
    public Camera cameraToDrawRay;

    public LayerMask mask;
    Vector3 mousePos;

    RaycastHit hit;
    Ray ray;
    private void InitializeFlowField()
    {
        curFlowField = new FlowField(cellRadius, gridSize);
        curFlowField.CreateGrid();
        gridDebug.SetFlowField(curFlowField);
    }

    private void Update()
    {
        

        if (Input.GetMouseButtonUp(0))
        {
            InitializeFlowField();
            curFlowField.CreateCostField();

            /*
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Cell destinationCell = curFlowField.GetCellFromWorldPos(worldMousePos);
            */
            mousePos = Input.mousePosition;
            ray = cameraToDrawRay.ScreenPointToRay(mousePos);
            Physics.Raycast(ray, out hit, Mathf.Infinity, mask);

            Cell destinationCell = curFlowField.GetCellFromWorldPos(hit.point);
            

            Debug.Log("Ray: " + hit.point);
            //Debug.Log("Cam: " +worldMousePos);

            curFlowField.CreateIntegrationField(destinationCell);
            curFlowField.CreateFlowField();

            gridDebug.DrawFlowField();
        }
    }

}
