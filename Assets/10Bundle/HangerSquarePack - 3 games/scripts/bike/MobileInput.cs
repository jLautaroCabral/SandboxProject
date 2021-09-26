using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileInput : MonoBehaviour {

    public static bool IsAccelareting;
    public static bool IsBraking;

    public void OnRun()
    {
        IsAccelareting = true;
    }

    public void OnNotRun()
    {
        IsAccelareting = false;
    }

    public void OnBrake()
    {
        IsBraking = true;
    }

    public void OnNotBrake()
    {
        IsBraking = false;
    }
}
