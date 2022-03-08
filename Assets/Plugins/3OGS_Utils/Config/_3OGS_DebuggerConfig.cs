using _3OGS.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _3OGS_DebuggerConfig : MonoBehaviour
{
    [Header("Build Settings")]
    public bool IsBuildForProduction;

    [Header("Debug buttons")]
    public Vector3 DefaultButtonSize;
    public float DefaultButtonPanelOffset;
    public Sprite DefaultButtonSprite;
    public Color DefaultButtonColor;
    public Color DefaultButtonColorOnOver;
    public Color DefaultButtonColorOnClick;
    public LayerMask ButtonsLayerMask;

    [Header("Other settings")]
    public float DebugObjectsDrawDistance;
    public Font DefaultFont;
    public int DefaultFontSize;

    public static _3OGS_DebuggerConfig CreateDefaultSettings()
    {
        _3OGS_DebuggerConfig newSettigs = new _3OGS_DebuggerConfig();

        newSettigs.IsBuildForProduction = false;
        newSettigs.DefaultButtonSize = new Vector3(5, 0.6f, 0.1f);
        newSettigs.DefaultButtonPanelOffset = 0.2f;
        newSettigs.DefaultButtonSprite = Resources.Load<Sprite>("DefaultButtonSprite.png");
        newSettigs.DefaultButtonColor = _3OGS_Utils.GetColorFromString("FFFFFF");
        newSettigs.DefaultButtonColorOnOver = _3OGS_Utils.GetColorFromString("EAEAEA"); ;
        newSettigs.DefaultButtonColorOnClick = _3OGS_Utils.GetColorFromString("BFBFBF"); ;
        newSettigs.ButtonsLayerMask = LayerMask.GetMask("Default");

        newSettigs.DebugObjectsDrawDistance = 50f;
        newSettigs.DefaultFont = Resources.Load<Font>("DefaultFont.png");
        newSettigs.DefaultFontSize = 24;

        return newSettigs;
    }
}
