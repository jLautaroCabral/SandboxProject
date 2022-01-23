using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LC_DebuggerConfig : ScriptableObject
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
}
