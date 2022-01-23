using LC.Debug;
using LC.Debug.Cameras;
using LC.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugExampleScript : MonoBehaviour
{
    private void Start()
    {
        GameObject btnPanel = LC_Debug.CreateButtonPanel(Vector3.up);
        DebugButtonPanel.AddButton(btnPanel.GetComponent<DebugButtonPanel>(), "Test 1", () => LC_Debug.TextPopup("Test1", Vector3.up));
        DebugButtonPanel.AddButton(btnPanel.GetComponent<DebugButtonPanel>(), "Test 2", () => LC_Debug.TextPopup("Test2", Vector3.up));
        DebugButtonPanel.AddButton(btnPanel.GetComponent<DebugButtonPanel>(), "Test 3", () => LC_Debug.TextPopup("Test3", Vector3.up), out GameObject debugBtn);

        if(debugBtn != null)
            debugBtn.GetComponent<DebugButton>().OnClickAction = () => { LC_Debug.TextPopup("Action replace test", debugBtn.transform.position + debugBtn.transform.right * (LC_Utils.GetDefaultButtonSize().x / 2 + LC_Utils.GetDefaultButtonPanelOffset())); };

        CameraSwitcher.SetupReturnAction(() => Debug.Log("Returning to original camera"));


        LC_Utils.CreateKeyCodeAction(KeyCode.F1,
                () => {
                    if (LC_Utils.IsBuildForProduction()) return;
                    LC_Debug.UseFreeLookCamera();
                });

        LC_Utils.CreateKeyCodeAction(KeyCode.F2,
                () => {
                    if (LC_Utils.IsBuildForProduction()) return;
                    LC_Debug.UseFreeLookCamera();
                });


        LC_Utils.CreateKeyCodeAction(KeyCode.E,
                () => {
                    if (LC_Utils.IsBuildForProduction()) return;
                    LC_Debug.RuntimeDebugObjectsManager.DisableDebugObjects();
                });

        LC_Utils.CreateKeyCodeAction(KeyCode.R,
                () => {
                    if (LC_Utils.IsBuildForProduction()) return;
                    LC_Debug.RuntimeDebugObjectsManager.EnableDebugObjects();
                });

        LC_Utils.CreateKeyCodeAction(KeyCode.Backspace,
                () => {
                    if (LC_Utils.IsBuildForProduction()) return;
                    CameraSwitcher.ReturnToOriginalCamera();
                });
    }
}
