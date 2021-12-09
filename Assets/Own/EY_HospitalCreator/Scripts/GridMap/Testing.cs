using LC.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EY_HospitalCreator
{
    public class Testing : MonoBehaviour
    {
        Grid<StringGridObject> grid;
        GridXZ<StringGridObject> gridXZ;
        // Start is called before the first frame update
        void Start()
        {
            //rid = new Grid<bool>(5, 5, 1f, new Vector3(-5, 0), () => false);
            //grid = new Grid<StringGridObject>(5, 5, 1f, new Vector3(-5, 0), (Grid<StringGridObject> g, int x, int y) => new StringGridObject(g, x, y));
            gridXZ = new GridXZ<StringGridObject>(5, 5, 1f, new Vector3(-5, 0, -5), (GridXZ<StringGridObject> g, int x, int y) => new StringGridObject(g, x, y));
        }

        void Update()
        {
            Vector3 position = LC_Utils.GetMouseWorldPosition();
            if (Input.GetMouseButtonDown(0))
            {
                

                //grid.SetValue(LC_Utils.GetMouseWorldPosition(), true);
            }
            if (Input.GetMouseButtonDown(1))
            {
               // Debug.Log(grid.GetValue(LC_Utils.GetMouseWorldPosition()));
            }

            if(Input.GetKeyDown(KeyCode.A)) { gridXZ.GetGridObject(position).AddLetter("A"); }
            if(Input.GetKeyDown(KeyCode.B)) { gridXZ.GetGridObject(position).AddLetter("B"); }
            if(Input.GetKeyDown(KeyCode.C)) { gridXZ.GetGridObject(position).AddLetter("C"); }

            if(Input.GetKeyDown(KeyCode.Alpha1)) { gridXZ.GetGridObject(position).AddNumber("1"); }
            if(Input.GetKeyDown(KeyCode.Alpha2)) { gridXZ.GetGridObject(position).AddNumber("2"); }
            if(Input.GetKeyDown(KeyCode.Alpha3)) { gridXZ.GetGridObject(position).AddNumber("3"); }
        }
    }

    public class StringGridObject
    {
        private string letters;
        private string numbers;
        private GridXZ<StringGridObject> grid;
        private int x;
        private int y;

        public StringGridObject(GridXZ<StringGridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            this.letters = x + ", " + y;
            this.numbers = "";
        }

        public void AddLetter(string letter)
        {
            letters += letter;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void AddNumber(string number)
        {
            numbers += number;
            grid.TriggerGridObjectChanged(x, y);
        }

        public override string ToString()
        {
            return letters + "\n" + numbers;
        }
    }
}
