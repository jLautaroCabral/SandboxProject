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
        public static void TextPopupMouse(string text)
        {
            UtilsClass.CreateWorldTextPopup(text, UtilsClass.GetMouseWorldPosition());
        }

        // Creates a Text pop up at the world position
        public static void TextPopup(string text, Vector3 position)
        {
            UtilsClass.CreateWorldTextPopup(text, position);
        }

        // Text Updater in World, (parent == null) = world position
        public static FunctionUpdater TextUpdater(Func<string> GetTextFunc, Vector3 localPosition, Transform parent = null)
        {
            return UtilsClass.CreateWorldTextUpdater(GetTextFunc, localPosition, parent);
        }

        // Text Updater in UI
        public static FunctionUpdater TextUpdaterUI(Func<string> GetTextFunc, Vector2 anchoredPosition)
        {
            return UtilsClass.CreateUITextUpdater(GetTextFunc, anchoredPosition);
        }

        // Text Updater always following mouse
        public static void MouseTextUpdater(Func<string> GetTextFunc, Vector3 positionOffset = default(Vector3))
        {
            GameObject gameObject = new GameObject();
            FunctionUpdater.Create(() => {
                gameObject.transform.position = UtilsClass.GetMouseWorldPosition() + positionOffset;
                return false;
            });
            TextUpdater(GetTextFunc, Vector3.zero, gameObject.transform);
        }
    }
}

