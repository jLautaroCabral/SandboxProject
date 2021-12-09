using LC.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LC.Debug
{
    public static class LC_Debug
    {
/*
        // Creates a Button in the World
        public static World_Sprite Button(Transform parent, Vector3 localPosition, string text, System.Action ClickFunc, int fontSize = 30, float paddingX = 5, float paddingY = 5)
        {
            return World_Sprite.CreateDebugButton(parent, localPosition, text, ClickFunc, fontSize, paddingX, paddingY);
        }

        // Creates a Button in the UI
        public static UI_Sprite ButtonUI(Vector2 anchoredPosition, string text, Action ClickFunc)
        {
            return UI_Sprite.CreateDebugButton(anchoredPosition, text, ClickFunc);
        }

        // Creates a World Text object at the world position
        public static void Text(string text, Vector3 localPosition = default(Vector3), Transform parent = null, int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = UtilsClass.sortingOrderDefault)
        {
            UtilsClass.CreateWorldText(text, parent, localPosition, fontSize, color, textAnchor, textAlignment, sortingOrder);
        }
*/

        // World text pop up at mouse position
        public static void WorldTextPopupMouse(string text)
        {
            Utils.LC_Utils.CreateWorldTextPopup(text, Utils.LC_Utils.GetMouseWorldPosition());
        }
        
        // World text pop up at mouse position
        public static void UITextPopupMouse(string text)
        {
            Utils.LC_Utils.CreateUITextPopup(text, Utils.LC_Utils.GetMousePosition());
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
            Utils.LC_Utils.CreateWorldTextPopup(text, position);
        }

        // Text Updater in World, (parent == null) = world position
        public static FunctionUpdater TextUpdater(Func<string> GetTextFunc, Vector3 localPosition, Transform parent = null)
        {
            return Utils.LC_Utils.CreateWorldTextUpdater(GetTextFunc, localPosition, parent);
        }

        // Text Updater in UI
        public static FunctionUpdater TextUpdaterUI(Func<string> GetTextFunc, Vector2 position)
        {
            return Utils.LC_Utils.CreateUITextUpdater(GetTextFunc, position);
        }

        // Text Updater always following mouse
        public static void MouseWorldTextUpdater(Func<string> GetTextFunc, Vector3 positionOffset = default(Vector3))
        {
            GameObject gameObject = new GameObject();
            FunctionUpdater.Create(() => {
                gameObject.transform.position = Utils.LC_Utils.GetMouseWorldPosition() + positionOffset;
                return false;
            });
            TextUpdater(GetTextFunc, Vector3.zero, gameObject.transform);
        }
    }
}

