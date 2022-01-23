using LC.Debug;
using LC.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EY_HospitalCreator
{
    public class GridBuildingSystem : MonoBehaviour
    {
        public static GridBuildingSystem Instance { get; private set; }

        [SerializeField]
        private List<PlacedObjectTypeSO> placedObjectTypeSOList;
        private PlacedObjectTypeSO placedObjectTypeSO;
        private PlacedObjectTypeSO.Dir dir = PlacedObjectTypeSO.Dir.Down;
        [SerializeField]
        private GridXZ<GridObject> grid;

        public event EventHandler OnSelectedChanged;
        public event EventHandler OnObjectPlaced;

        void Awake()
        {
            Instance = this;

            int gridWith = 10;
            int gridHeight = 10;
            float cellSize = 1f;
            grid = new GridXZ<GridObject>(gridWith, gridHeight, cellSize, new Vector3(-5,0,-5), (GridXZ<GridObject> g, int x, int y) => new GridObject(g, x, y));

            placedObjectTypeSO = placedObjectTypeSOList[0];
        }

        void Update()
        {
            if(Input.GetMouseButtonDown(0) && placedObjectTypeSO != null)
            {
                Vector3 mousePosition = LC_Utils.GetMouseWorldPosition_Ray();
                grid.GetXZ(mousePosition, out int x, out int z);

                Vector2Int placedObjectOrigin = new Vector2Int(x, z);
                placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);
                List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionsList(new Vector2Int(x, z), dir);

                // Test
                bool canBuild = true;
                foreach(Vector2Int gridPosition in gridPositionList)
                {
                    Debug.Log("Search: " + gridPosition.x + "; " + gridPosition.y);
                    if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                    {
                        Debug.Log("Cannot here!!");
                        // Cannot build here
                        canBuild = false;
                        break;
                    }
                }

                //GridObject gridObject = grid.GetGridObject(x, z);
                if (canBuild)
                {
                    Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                    Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

                    PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), dir, placedObjectTypeSO);
                    
                    foreach(Vector2Int gridPosition in gridPositionList)
                    {
                        Debug.Log("Placed = x: " + gridPosition.x + "; " + gridPosition.y);
                        grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                    }

                    OnObjectPlaced?.Invoke(this, EventArgs.Empty);

                } else
                {
                    LC_Debug.CreateWorldTextPopup("Cannot build here!!", LC_Utils.GetMouseWorldPosition_Ray(), new Vector3(0, 1f));
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                GridObject gridObject = grid.GetGridObject(LC_Utils.GetMouseWorldPosition_Ray());
                if(gridObject != null)
                {
                    PlacedObject placedObject = gridObject.GetPlacedObject();

                    if(placedObject != null)
                    {
                        placedObject.DestroySelf();
                        List<Vector2Int> gridPositionList = placedObject.GetGridPositionsList();

                        foreach (Vector2Int gridPosition in gridPositionList)
                        {
                            grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                dir = PlacedObjectTypeSO.GetNextDir(dir);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) { placedObjectTypeSO = placedObjectTypeSOList[0]; RefreshSelectedObjectType(); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { placedObjectTypeSO = placedObjectTypeSOList[1]; RefreshSelectedObjectType(); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { placedObjectTypeSO = placedObjectTypeSOList[2]; RefreshSelectedObjectType(); }
/*            if (Input.GetKeyDown(KeyCode.Alpha4)) { placedObjectTypeSO = placedObjectTypeSOList[3]; RefreshSelectedObjectType(); }
            if (Input.GetKeyDown(KeyCode.Alpha5)) { placedObjectTypeSO = placedObjectTypeSOList[4]; RefreshSelectedObjectType(); }*/
        }

        private void DeselectObjectType()
        {
            placedObjectTypeSO = null; RefreshSelectedObjectType();
        }

        private void RefreshSelectedObjectType()
        {
            OnSelectedChanged?.Invoke(this, EventArgs.Empty);
        }

        public Vector3 GetMouseWorldSnappedPosition()
        {
            Vector3 mousePosition = LC_Utils.GetMouseWorldPosition_Ray();
            grid.GetXZ(mousePosition, out int x, out int z);

            if (placedObjectTypeSO != null)
            {
                Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
                return placedObjectWorldPosition;
            }
            else
            {
                return mousePosition;
            }
        }

        public Quaternion GetPlacedObjectRotation()
        {
            if (placedObjectTypeSO != null)
            {
                return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
            }
            else
            {
                return Quaternion.identity;
            }
        }

        public PlacedObjectTypeSO GetPlacedObjectTypeSO()
        {
            return placedObjectTypeSO;
        }

        public Vector2Int GetGridPosition(Vector3 worldPosition) {
        grid.GetXZ(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

        public class GridObject
        {
            private GridXZ<GridObject> grid;
            private int x;
            private int z;
            private PlacedObject placedObject;

            public GridObject(GridXZ<GridObject> grid, int x, int z)
            {
                this.grid = grid;
                this.x = x;
                this.z = z;
            }
            public void SetPlacedObject(PlacedObject placedObject)
            {
                this.placedObject = placedObject;
                grid.TriggerGridObjectChanged(x, z);
            }
            public PlacedObject GetPlacedObject()
            {
                return this.placedObject;
            }

            public void ClearPlacedObject()
            {
                this.placedObject = null;
                grid.TriggerGridObjectChanged(x, z);
            }
            public bool CanBuild()
            {
                return placedObject == null;
            }
            public override string ToString()
            {
                return x + ", " + z;
            }
        }
    }

    

}