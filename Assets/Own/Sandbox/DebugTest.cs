using LC.Debug;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //LC_Debug.TextUpdaterUI(delegate () { return "Hello World!"; }, LC_UtilsClass.GetMousePosition());

        if(Input.GetMouseButtonUp(0))
            LC_Debug.UITextPopupMouse("Hello World!");


        //Debug.Log("Input mouse pos: " + LC_UtilsClass.GetMousePosition());
        //Debug.Log("Screen to viewport pos: " + Camera.main.ScreenToViewportPoint(LC_UtilsClass.GetMousePosition()));
        //Debug.Log("Screen to viewport pos: " + Camera.main.Sc);}
        
        /*
        Vector2 localpoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, GetComponentInParent<Canvas>().worldCamera, out localpoint);

        Vector2 normalizedPoint = Rect.PointToNormalized(rectTransform.rect, localpoint);

        Debug.Log(normalizedPoint);
        */
    }
}
