using LC.Debug;
using LC.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EY_HospitalCreator
{
    public class Grid<TGridObject>
    {
        public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
        public class OnGridObjectChangedEventArgs : EventArgs
        {
            public int x;
            public int y;
        }
        public void TriggerGridObjectChanged(int x, int y)
        {
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }

        private int width;
        private int height;
        private float cellSize;
        private TGridObject[,] gridArray;
        private Vector3 originPosition;
        private TextMesh[,] debugTextArray;

        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;
            gridArray = new TGridObject[width, height];
            debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                    gridArray[x, y] = createGridObject(this, x, y);
            }

            bool showDebug = true;
            if(showDebug)
            {
                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < gridArray.GetLength(1); y++)
                    {
                        debugTextArray[x, y] = LC_Debug.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorlPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 20, Color.red, TextAnchor.MiddleCenter).GetComponent<TextMesh>();
                        Debug.Log("Cell x: " + x + " y: " + y);
                        Debug.DrawLine(GetWorlPosition(x, y), GetWorlPosition(x, y + 1), Color.red, 100f);
                        Debug.DrawLine(GetWorlPosition(x, y), GetWorlPosition(x + 1, y), Color.red, 100f);
                    }
                }
                Debug.DrawLine(GetWorlPosition(0, height), GetWorlPosition(width, height), Color.red, 100f);
                Debug.DrawLine(GetWorlPosition(width, 0), GetWorlPosition(width, height), Color.red, 100f);

                OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs args) =>
                {
                    debugTextArray[args.x, args.y].text = gridArray[args.x, args.y]?.ToString();
                };
            }
        }

        public Vector3 GetWorlPosition(int x, int y)
        {
            return new Vector3(x, y) * cellSize + originPosition;
        }

        private void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        }

        public void SetGridObject(int x, int y, TGridObject value)
        {
            if(x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x, y] = value;
                debugTextArray[x, y].text = gridArray[x, y].ToString();
            }
        }

        public void SetGridObject(Vector3 worldPosition, TGridObject value)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetGridObject(x, y, value);
        }

        public TGridObject GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                return gridArray[x, y];
            } else
            {
                return default(TGridObject);
            }
        }

        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetGridObject(x, y);
        }
    }
}