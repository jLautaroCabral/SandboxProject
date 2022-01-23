using LC.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LC.Debug
{
    public class DebugButtonPanel : MonoBehaviour
    {
        public int ButtonQuantity = 0;
        public static void AddButton(DebugButtonPanel btnPanel, string btnLabel, Action onClickFunction)
        {
            AddButton(btnPanel, btnLabel, onClickFunction, out GameObject suppObj);
        }

        public static void AddButton(DebugButtonPanel btnPanel, string btnLabel, Action onClickFunction, out GameObject debugBtnObj)
        {
            debugBtnObj = null;
            if (LC_Utils.IsBuildForProduction()) return;

            Vector3 btnPos = btnPanel.transform.position;
            btnPos += btnPanel.transform.up * (btnPanel.ButtonQuantity * LC_Utils.GetDefaultButtonSize().y);

            if (btnPanel.ButtonQuantity != 0)
                btnPos += btnPanel.transform.up * btnPanel.ButtonQuantity  * LC_Utils.GetDefaultButtonPanelOffset();

            GameObject newBtn = LC_Debug.CreateDebugButton(btnLabel, btnPos, onClickFunction, false);

            newBtn.transform.rotation = btnPanel.transform.rotation;
            newBtn.transform.parent = btnPanel.transform;

            btnPanel.ButtonQuantity++;
            debugBtnObj = newBtn;
        }
    }
}