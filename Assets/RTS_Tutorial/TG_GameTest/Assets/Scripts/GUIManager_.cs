using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager_ : MonoBehaviour
{
    public static GUIManager_ sharedInstance;
    public Texture2D blackBoxSemiTrans;

    private void Awake()
    {
        if (sharedInstance == null) sharedInstance = this;
    }
    public Texture2D getBlackBoxSemiTrans()
    {
        return blackBoxSemiTrans;
    }
}
