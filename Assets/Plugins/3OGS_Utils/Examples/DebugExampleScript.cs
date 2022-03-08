using _3OGS.Debug;
using _3OGS.Debug.Cameras;
using _3OGS.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugExampleScript : MonoBehaviour
{
    private void Start()
    {
        GameObject btnObj = _3OGS_Debug.CreateDebugButton("Example", Vector3.up, () => Debug.Log("Example log"));

        GameObject btnPanel = _3OGS_Debug.CreateButtonPanel(Vector3.up);
        DebugButtonPanel.AddButton(btnPanel.GetComponent<DebugButtonPanel>(), "Test 1", () => _3OGS_Debug.TextPopup("Test1", Vector3.up));
        DebugButtonPanel.AddButton(btnPanel.GetComponent<DebugButtonPanel>(), "Test 2", () => _3OGS_Debug.TextPopup("Test2", Vector3.up));
        GameObject debugBtn = DebugButtonPanel.AddButton(btnPanel.GetComponent<DebugButtonPanel>(), "Test 3", () => _3OGS_Debug.TextPopup("Test3", Vector3.up));

        if (debugBtn != null)
            debugBtn.GetComponent<DebugButton>().OnClickAction = () => { _3OGS_Debug.TextPopup("Action replace test", debugBtn.transform.position + debugBtn.transform.right * (_3OGS_Utils.GetDefaultButtonSize().x / 2 + _3OGS_Utils.GetDefaultButtonPanelOffset())); };

        CameraSwitcher.SetupReturnAction(() => Debug.Log("Returning to original camera"));

        _3OGS_Utils.CreateKeyCodeAction(KeyCode.A, () => Debug.Log("Awesome KeyCodeAction Test"));


        _3OGS_Utils.CreateKeyCodeAction(KeyCode.F1,
                () => {
                    if (_3OGS_Utils.IsBuildForProduction()) return;
                    _3OGS_Debug.UseFreeLookCamera();
                });

        GameObject anyObject = GameObject.Find("AnyObject");

        _3OGS_Utils.CreateKeyCodeAction(KeyCode.F2,
                () => {
                    if (_3OGS_Utils.IsBuildForProduction()) return;
                    _3OGS_Debug.UseRotateAroundTargetCamera(anyObject.transform);
                });


        _3OGS_Utils.CreateKeyCodeAction(KeyCode.T,
                () => {
                    if (_3OGS_Utils.IsBuildForProduction()) return;
                    _3OGS_Debug.RuntimeDebugObjectsManager.DisableDebugObjects();
                });

        _3OGS_Utils.CreateKeyCodeAction(KeyCode.R,
                () => {
                    if (_3OGS_Utils.IsBuildForProduction()) return;
                    _3OGS_Debug.RuntimeDebugObjectsManager.EnableDebugObjects();
                });

        _3OGS_Utils.CreateKeyCodeAction(KeyCode.Backspace,
                () => {
                    if (_3OGS_Utils.IsBuildForProduction()) return;
                    CameraSwitcher.ReturnToOriginalCamera();
                });
    }
}
