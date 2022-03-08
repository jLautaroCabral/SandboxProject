using _3OGS.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace _3OGS.Debug
{
    public class DebugButtonPanel : MonoBehaviour
    {
        public int ButtonQuantity = 0;

        public static GameObject AddButton(DebugButtonPanel btnPanel, string btnLabel, Action onClickFunction)
        {
            if (_3OGS_Utils.IsBuildForProduction()) return _3OGS_Utils.GetDeafultEmptyObject();

            Vector3 btnPos = btnPanel.transform.position;
            btnPos += btnPanel.transform.up * (btnPanel.ButtonQuantity * _3OGS_Utils.GetDefaultButtonSize().y);

            if (btnPanel.ButtonQuantity != 0)
                btnPos += btnPanel.transform.up * btnPanel.ButtonQuantity  * _3OGS_Utils.GetDefaultButtonPanelOffset();

            GameObject newBtn = _3OGS_Debug.CreateDebugButton(btnLabel, btnPos, onClickFunction, false);

            newBtn.transform.rotation = btnPanel.transform.rotation;
            newBtn.transform.parent = btnPanel.transform;

            btnPanel.ButtonQuantity++;
            return newBtn;
        }
    }
}