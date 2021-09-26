using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TG_GameTest {
    public class Player : MonoBehaviour
    {
        public List<GameObject> units = new List<GameObject>();

        public bool IsMyUnit(GameObject unitToSearch)
        {
            foreach (GameObject unit in units)
            {
                if (unit == unitToSearch)
                {
                    return true;
                }
            }
            return false;
        }

        /*
        public bool IsMyUnit(Unit unit)
        {
            return unit.playerOwner == this;
        }
        */
    }
}

