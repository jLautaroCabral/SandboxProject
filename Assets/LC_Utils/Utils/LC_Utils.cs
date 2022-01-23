using LC.Debug;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace LC.Utils
{
    public static class LC_Utils
    {
        /*private static readonly Vector3 Vector3zero = Vector3.zero;
        private static readonly Vector3 Vector3one = Vector3.one;*/
        public const int sortingOrderDefault = 5000;

        // Get Main Canvas Transform
        private static Transform cachedCanvasTransform;
        public static Transform GetCanvasTransform()
        {
            if (cachedCanvasTransform == null)
            {
                Canvas canvas = MonoBehaviour.FindObjectOfType<Canvas>();
                if (canvas != null)
                {
                    cachedCanvasTransform = canvas.transform;
                }
            }
            return cachedCanvasTransform;
        }

        // Get Mouse Position in pixel coordinates
        public static Vector3 GetMousePosition()
        {
            return Input.mousePosition;
        }

        // Get Mouse Position in World with Z = 0f
        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }
        /*
        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }
        */
        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        public static Vector3 GetMouseWorldPosition_Ray()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, LayerMask.GetMask("Default")))
                return raycastHit.point;
            else
                return Vector3.zero;
        }

        public static FunctionUpdater CreateKeyCodeAction(KeyCode keyCode, Action onKeyDown)
        {
            return FunctionUpdater.Create(() => {
                if (Input.GetKeyDown(keyCode))
                {
                    onKeyDown();
                }
                return false;
            });
        }

        public static Camera GetCurrentCamera()
        {
            foreach(Camera cam in Camera.allCameras)
            {
                if (cam == Camera.current)
                    return cam;
            }
            return Camera.main;
        }

        public static GameObject GetDeafultEmptyObject()
        {
            return LC_DebugManager.sharedInstance.EmptyObject;
        }
        public static Font GetDefaultFont()
        {
            return LC_DebugManager.sharedInstance.Settings.DefaultFont;
        }
        public static Sprite GetDefaultSpriteSquare()
        {
            return LC_DebugManager.sharedInstance.Settings.DefaultButtonSprite;
        }
        public static Color GetDefaultButtonColor()
        {
            return LC_DebugManager.sharedInstance.Settings.DefaultButtonColor;
        }
        public static Color GetDefaultButtonColorOnClick()
        {
            return LC_DebugManager.sharedInstance.Settings.DefaultButtonColorOnClick;
        }
        public static Color GetDefaultButtonColorOnOver()
        {
            return LC_DebugManager.sharedInstance.Settings.DefaultButtonColorOnOver;
        }
        public static Vector3 GetDefaultButtonSize()
        {
            return LC_DebugManager.sharedInstance.Settings.DefaultButtonSize;
        }
        public static int GetDefaultFontSize()
        {
            return LC_DebugManager.sharedInstance.Settings.DefaultFontSize;
        }
        public static float GetDefaultButtonPanelOffset()
        {
            return LC_DebugManager.sharedInstance.Settings.DefaultButtonPanelOffset;
        }
        public static LC_DebuggerConfig GetSettings()
        {
            return LC_DebugManager.sharedInstance.Settings;
        }

        public static bool IsBuildForProduction()
        {
            return LC_DebugManager.sharedInstance.Settings.IsBuildForProduction;
        }

        /*
        // Returns 00-FF, value 0->255
        public static string Dec_to_Hex(int value)
        {
            return value.ToString("X2");
        }

        // Returns 0-255
        public static int Hex_to_Dec(string hex)
        {
            return Convert.ToInt32(hex, 16);
        }

        // Returns a hex string based on a number between 0->1
        public static string Dec01_to_Hex(float value)
        {
            return Dec_to_Hex((int)Mathf.Round(value * 255f));
        }

        // Returns a float between 0->1
        public static float Hex_to_Dec01(string hex)
        {
            return Hex_to_Dec(hex) / 255f;
        }

        // Get Hex Color FF00FF
        public static string GetStringFromColor(Color color)
        {
            string red = Dec01_to_Hex(color.r);
            string green = Dec01_to_Hex(color.g);
            string blue = Dec01_to_Hex(color.b);
            return red + green + blue;
        }

        // Get Hex Color FF00FFAA
        public static string GetStringFromColorWithAlpha(Color color)
        {
            string alpha = Dec01_to_Hex(color.a);
            return GetStringFromColor(color) + alpha;
        }

        // Sets out values to Hex String 'FF'
        public static void GetStringFromColor(Color color, out string red, out string green, out string blue, out string alpha)
        {
            red = Dec01_to_Hex(color.r);
            green = Dec01_to_Hex(color.g);
            blue = Dec01_to_Hex(color.b);
            alpha = Dec01_to_Hex(color.a);
        }

        // Get Hex Color FF00FF
        public static string GetStringFromColor(float r, float g, float b)
        {
            string red = Dec01_to_Hex(r);
            string green = Dec01_to_Hex(g);
            string blue = Dec01_to_Hex(b);
            return red + green + blue;
        }

        // Get Hex Color FF00FFAA
        public static string GetStringFromColor(float r, float g, float b, float a)
        {
            string alpha = Dec01_to_Hex(a);
            return GetStringFromColor(r, g, b) + alpha;
        }

        // Get Color from Hex string FF00FFAA
        public static Color GetColorFromString(string color)
        {
            float red = Hex_to_Dec01(color.Substring(0, 2));
            float green = Hex_to_Dec01(color.Substring(2, 2));
            float blue = Hex_to_Dec01(color.Substring(4, 2));
            float alpha = 1f;
            if (color.Length >= 8)
            {
                // Color string contains alpha
                alpha = Hex_to_Dec01(color.Substring(6, 2));
            }
            return new Color(red, green, blue, alpha);
        }
        */

        /*
        // Draw a UI Sprite
        public static RectTransform DrawSpriteUI(Color color, Transform parent, Vector2 pos, Vector2 size, string name = null)
        {
            RectTransform rectTransform = DrawSpriteUI(null, color, parent, pos, size, name);
            return rectTransform;
        }

        // Draw a UI Sprite
        public static RectTransform DrawSpriteUI(Sprite sprite, Transform parent, Vector2 pos, Vector2 size, string name = null)
        {
            RectTransform rectTransform = DrawSpriteUI(sprite, Color.white, parent, pos, size, name);
            return rectTransform;
        }

        // Draw a UI Sprite
        public static RectTransform DrawSpriteUI(Sprite sprite, Color color, Transform parent, Vector2 pos, Vector2 size, string name = null)
        {
            // Setup icon
            if (name == null || name == "") name = "LC_UISprite";
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
            RectTransform goRectTransform = go.GetComponent<RectTransform>();
            goRectTransform.SetParent(parent, false);
            goRectTransform.sizeDelta = size;
            goRectTransform.anchoredPosition = pos;

            Image image = go.GetComponent<Image>();
            image.sprite = sprite;
            image.color = color;

            return goRectTransform;
        }
        */

        /*
        public static SpriteRenderer DrawSpriteWorld(Sprite sprite, Vector3 position, Vector3 scale, Color color)
        {
            GameObject spriteObj = new GameObject("LC_WorldSprite", typeof(SpriteRenderer));
            SpriteRenderer spriteComponent = spriteObj.GetComponent<SpriteRenderer>();
            return spriteComponent;
        }
        */
        /*
        public static Text DrawTextUI(string textString, Transform parent, Vector2 position, int fontSize, Font font)
        {
            GameObject textGo = new GameObject("Text", typeof(RectTransform), typeof(Text));
            textGo.transform.SetParent(parent, false);
            Transform textGoTrans = textGo.transform;
            textGoTrans.SetParent(parent, false);
            textGoTrans.localPosition = Vector3zero;
            textGoTrans.localScale = Vector3one;

            textGoTrans.position = new Vector3(position.x, position.y, 0);

            RectTransform textGoRectTransform = textGo.GetComponent<RectTransform>();
            textGoRectTransform.sizeDelta = new Vector2(0, 0);

            Text text = textGo.GetComponent<Text>();
            text.text = textString;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.alignment = TextAnchor.MiddleLeft;
            if (font == null) font = GetDefaultFont();
            text.font = font;
            text.fontSize = fontSize;

            textGo.AddComponent<Outline>();

            return text;
        }
        */
    }
}