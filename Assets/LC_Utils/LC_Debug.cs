using LC_Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LC_Utils.Debug
{
    public static class LC_Debug
    {
        // World text pop up at mouse position
        public static void WorldTextPopupMouse(string text)
        {
            LC_UtilsClass.CreateWorldTextPopup(text, LC_UtilsClass.GetMouseWorldPosition());
        }
        
        // World text pop up at mouse position
        public static void UITextPopupMouse(string text)
        {
            LC_UtilsClass.CreateUITextPopup(text, LC_UtilsClass.GetMousePosition());
        }

        /*
        // World text pop up at ray point from mouse position
        public static void TextPopupMouseRay(string text)
        {
            Ray ray = Camera.main.ScreenPointToRay(UtilsClass.GetMousePosition());
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Default"));

            UtilsClass.CreateWorldTextPopup(text, hit.point);
        }
        */

        // Creates a Text pop up at the world position
        public static void TextPopup(string text, Vector3 position)
        {
            LC_UtilsClass.CreateWorldTextPopup(text, position);
        }

        // Text Updater in World, (parent == null) = world position
        public static FunctionUpdater TextUpdater(Func<string> GetTextFunc, Vector3 localPosition, Transform parent = null)
        {
            return LC_UtilsClass.CreateWorldTextUpdater(GetTextFunc, localPosition, parent);
        }

        // Text Updater in UI
        public static FunctionUpdater TextUpdaterUI(Func<string> GetTextFunc, Vector2 position)
        {
            return LC_UtilsClass.CreateUITextUpdater(GetTextFunc, position);
        }

        // Text Updater always following mouse
        public static void MouseWorldTextUpdater(Func<string> GetTextFunc, Vector3 positionOffset = default(Vector3))
        {
            GameObject gameObject = new GameObject();
            FunctionUpdater.Create(() => {
                gameObject.transform.position = LC_UtilsClass.GetMouseWorldPosition() + positionOffset;
                return false;
            });
            TextUpdater(GetTextFunc, Vector3.zero, gameObject.transform);
        }
    }
}

