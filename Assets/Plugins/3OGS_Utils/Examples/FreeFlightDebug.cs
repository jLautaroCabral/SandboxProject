using _3OGS.Debug;
using _3OGS.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFlightDebug : MonoBehaviour {

	// Use this for initialization
	void Start () {
        _3OGS_Utils.CreateKeyCodeAction(KeyCode.F1,
                () => {
                    if (_3OGS_Utils.IsBuildForProduction()) return;

                    CameraSwitcher.CurrentCamera.gameObject.SetActive(false);

                    _3OGS_Debug.UseFreeLookCamera(transform.position);
                });

        GameObject btnPanel = _3OGS_Debug.CreateButtonPanel(transform.position);
        DebugButtonPanel.AddButton(btnPanel.GetComponent<DebugButtonPanel>(), "Test 1", () => _3OGS_Debug.TextPopup("Test1", Vector3.up));
        DebugButtonPanel.AddButton(btnPanel.GetComponent<DebugButtonPanel>(), "Test 2", () => _3OGS_Debug.TextPopup("Test2", Vector3.up));
        GameObject debugBtn = DebugButtonPanel.AddButton(btnPanel.GetComponent<DebugButtonPanel>(), "Test 3", () => _3OGS_Debug.TextPopup("Test3", Vector3.up));

        if (debugBtn != null)
            debugBtn.GetComponent<DebugButton>().OnClickAction = () => { _3OGS_Debug.TextPopup("Action replace test", debugBtn.transform.position + debugBtn.transform.right * (_3OGS_Utils.GetDefaultButtonSize().x / 2 + _3OGS_Utils.GetDefaultButtonPanelOffset())); };

    }
}
