using _3OGS.Utils;
using _3OGS.Debug.Cameras;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _3OGS.Debug
{
    public static class _3OGS_Debug
    {
        public static void UseFreeLookCamera()
        {
            UseFreeLookCamera(Vector3.zero);
        }
        public static void UseFreeLookCamera(Vector3 position)
        {
            CameraSwitcher.SetupCurrentCamera(_3OGS_FreeLookCamera.CreateFreeLookCamera(position));
        }
        public static void UseRotateAroundTargetCamera(Transform target)
        {
            CameraSwitcher.SetupCurrentCamera(_3OGS_RotateAroundCamera.CreateRotateAroundToTargetCamera(target));
        }
/*
        // World text pop up at mouse position
        public static void WorldTextPopupMouse(string text)
        {
            if (LC_Utils.IsBuildForProduction())
                return;

            LC_GraphicsDebugUtils.CreateWorldTextPopup(text, Utils.LC_Utils.GetMouseWorldPosition());
        }*/

        // Creates a Text pop up at the world position
        
/*
        // Text Updater in World, (parent == null) = world position
        public static *//*FunctionUpdater*//*void TextUpdater(Func<string> GetTextFunc, Vector3 localPosition, Transform parent = null)
        {
            *//*return *//*
            LC_GraphicsDebugUtils.CreateWorldTextUpdater(GetTextFunc, localPosition, parent);
        }
        // Text Updater in UI
        public static FunctionUpdater TextUpdaterUI(Func<string> GetTextFunc, Vector2 position)
        {
            return LC_GraphicsDebugUtils.CreateUITextUpdater(GetTextFunc, position);
        }
        */
        public static GameObject CreateDebugButton(string btnLabel, Vector3 position, Action onClickFunction, bool rotateToCamera = true)
        {
            if (_3OGS_Utils.IsBuildForProduction())
                return _3OGS_Utils.GetDeafultEmptyObject(); // Do nothing

            return LC_GraphicsDebugUtils.CreateDebugButton(null, btnLabel, position, onClickFunction, rotateToCamera);
        }
        public static GameObject CreateDebugButton(Transform parent, string btnLabel, Vector3 position, Action onClickFunction, bool rotateToCamera = true)
        {
            if (_3OGS_Utils.IsBuildForProduction())
                return _3OGS_Utils.GetDeafultEmptyObject(); // Do nothing

            return LC_GraphicsDebugUtils.CreateDebugButton(parent, btnLabel, position, onClickFunction, rotateToCamera);
        }
        public static GameObject CreateButtonPanel(Vector3 position)
        {
            if (_3OGS_Utils.IsBuildForProduction())
                return _3OGS_Utils.GetDeafultEmptyObject(); // Do nothing

            return LC_GraphicsDebugUtils.CreateButtonPanel(null, position);
        }
        public static GameObject CreateButtonPanel(Transform parent, Vector3 position)
        {
            if (_3OGS_Utils.IsBuildForProduction())
                return _3OGS_Utils.GetDeafultEmptyObject(); // Do nothing

            return LC_GraphicsDebugUtils.CreateButtonPanel(parent, position);
        }

        public static GameObject WorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = _3OGS_Utils.sortingOrderDefault)
        {
            if (_3OGS_Utils.IsBuildForProduction())
                return _3OGS_Utils.GetDeafultEmptyObject(); // Do nothing
            if (color == null) color = Color.white;
            return LC_GraphicsDebugUtils.CreateWorldText(parent, text, localPosition, fontSize, color, textAnchor, textAlignment, sortingOrder).gameObject;
        }
        /*public static GameObject WorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            if (LC_Utils.IsBuildForProduction())
                return LC_Utils.GetDeafultEmptyObject(); // Do nothing

            return LC_GraphicsDebugUtils.CreateWorldText(parent, text, localPosition, fontSize, color, textAnchor, textAlignment, sortingOrder).gameObject;
        }
*//*
        // Create a Text Popup in the World, no parent
        public static void TextPopup(string text, Vector3 localPosition)
        {
            LC_GraphicsDebugUtils.CreateWorldTextPopup(null, text, localPosition, 40, Color.white, localPosition + new Vector3(0, .5f), 1f);
        }*/
        public static void TextPopup(string text, Vector3 position)
        {
            if (_3OGS_Utils.IsBuildForProduction())
                return;

            LC_GraphicsDebugUtils.CreateWorldTextPopup(text, position);
        }
        // Create a Text Popup in the World, no parent
        public static void TextPopup(string text, Vector3 localPosition, float popupTime)
        {
            LC_GraphicsDebugUtils.CreateWorldTextPopup(null, text, localPosition, 40, Color.white, localPosition + new Vector3(0, .5f), popupTime);
        }
        /*// Create a Text Popup in the World, no parent
        public static void TextPopup(string text, Vector3 localPosition, Vector3 finalPos)
        {
            LC_GraphicsDebugUtils.CreateWorldTextPopup(null, text, localPosition, 40, Color.white, localPosition + finalPos, 1f);
        }*/

        private static class LC_GraphicsDebugUtils
        {
            /*public static GameObject CreateDebugButton(string btnLabel, Vector3 position, Action onClickFunction)
            {
                return CreateDebugButton(null, btnLabel, position, onClickFunction, true);
            }*/
            public static GameObject CreateDebugButton(Transform parent, string btnLabel, Vector3 position, Action onClickFunction, bool rotateToCamera = true)
            {
                GameObject btnObj = new GameObject("DebugButton_" + btnLabel);
                DebugButton btnDebugComponent = btnObj.AddComponent<DebugButton>();
                BoxCollider btnCollider = btnObj.AddComponent<BoxCollider>();

                btnObj.layer = LayerMask.NameToLayer(_3OGS_Utils.GetSettings().ButtonsLayerMask.ToString());

                if (rotateToCamera)
                    btnObj.AddComponent<LookToCamera>().RotateY = true;
                if (parent != null)
                    btnObj.transform.parent = parent;

                if (onClickFunction != null)
                    btnDebugComponent.OnClickAction = onClickFunction;

                btnCollider.isTrigger = true;
                btnCollider.size = _3OGS_Utils.GetDefaultButtonSize();

                btnObj.transform.position = position;

                TextMesh buttonLabel =
                    CreateWorldText(
                        text: btnLabel,
                        parent: btnObj.transform,
                        localPosition: Vector3.zero,
                        color: Color.black,
                        textAlignment: TextAlignment.Center,
                        textAnchor: TextAnchor.MiddleCenter,
                        fontSize: _3OGS_Utils.GetDefaultFontSize()
                        );

                GameObject buttonBackground =
                    CreateWorldSprite(
                        parent: btnObj.transform,
                        name: "DebugButton_Background",
                        sprite: _3OGS_Utils.GetDefaultSpriteSquare(),
                        position: Vector3.zero,
                        localScale: _3OGS_Utils.GetDefaultButtonSize(),
                        color: _3OGS_Utils.GetDefaultButtonColor()
                        );

                CreateWorldSprite(
                    parent: btnObj.transform,
                    name: "DebugButton_BackgroundBorder",
                    sprite: _3OGS_Utils.GetDefaultSpriteSquare(),
                    position: Vector3.zero + btnObj.transform.forward * 0.01f,
                    localScale: _3OGS_Utils.GetDefaultButtonSize() + new Vector3(.05f, .05f, .05f),
                    color: Color.gray
                    );

                btnDebugComponent.buttonLabel = buttonLabel;
                btnDebugComponent.backgroundSprite = buttonBackground.GetComponent<SpriteRenderer>();
                btnDebugComponent.backgroundColor = _3OGS_Utils.GetDefaultButtonColor();
                btnDebugComponent.backgroundOnClickColor = _3OGS_Utils.GetDefaultButtonColorOnClick();
                btnDebugComponent.backgroundOnOverColor = _3OGS_Utils.GetDefaultButtonColorOnOver();

                btnDebugComponent.SetButtonLabelText(btnLabel);

                _3OGS_Debug.RuntimeDebugObjectsManager.debugObjects.Add(btnObj);

                return btnObj;
            }

            public static GameObject CreateButtonPanel(Transform parent, Vector3 position)
            {
                GameObject btnPanelObj = new GameObject("DebugPanel", typeof(DebugButtonPanel));

                btnPanelObj.AddComponent<LookToCamera>().RotateY = true;

                if (parent != null)
                    btnPanelObj.transform.parent = parent;

                btnPanelObj.transform.position = position;

                _3OGS_Debug.RuntimeDebugObjectsManager.debugObjects.Add(btnPanelObj);

                return btnPanelObj;
            }


            // Creates a Text Mesh in the World and constantly updates it
            public static /*FunctionUpdater*/void CreateWorldTextUpdater(Func<string> GetTextFunc, Vector3 localPosition, Transform parent = null)
            {
                TextMesh textMesh = CreateWorldText(GetTextFunc(), parent, localPosition);
                /*return */
                FunctionUpdater.Create(() => {
                    textMesh.text = GetTextFunc();
                    return false;
                }, "WorldTextUpdater");
            }

            // Create a Text Popup in the World, no parent
            public static void CreateWorldTextPopup(string text, Vector3 localPosition)
            {
                CreateWorldTextPopup(null, text, localPosition, 40, Color.white, localPosition + new Vector3(0, .5f), 1f);
            }
            // Create a Text Popup in the World, no parent
            public static void CreateWorldTextPopup(string text, Vector3 localPosition, float popupTime)
            {
                CreateWorldTextPopup(null, text, localPosition, 40, Color.white, localPosition + new Vector3(0, .5f), popupTime);
            }
            // Create a Text Popup in the World, no parent
            public static void CreateWorldTextPopup(string text, Vector3 localPosition, Vector3 finalPos)
            {
                CreateWorldTextPopup(null, text, localPosition, 40, Color.white, localPosition + finalPos, 1f);
            }

            // Create a Text Popup in the World
            public static void CreateWorldTextPopup(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, Vector3 finalPopupPosition, float popupTime)
            {
                TextMesh textMesh = CreateWorldText(parent, text, localPosition, fontSize, color, TextAnchor.LowerLeft, TextAlignment.Left, _3OGS_Utils.sortingOrderDefault);
                Transform transform = textMesh.transform;
                Vector3 moveAmount = (finalPopupPosition - localPosition) / popupTime;
                textMesh.gameObject.AddComponent<LookToCamera>();
                FunctionUpdater.Create(delegate () {

                    transform.position += moveAmount * Time.deltaTime;
                    popupTime -= Time.deltaTime;

                    if (popupTime <= 0f)
                    {
                        UnityEngine.Object.Destroy(transform.gameObject);
                        _3OGS_Debug.RuntimeDebugObjectsManager.debugObjects.Remove(transform.gameObject);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }, "WorldTextPopup");
            }


            public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = _3OGS_Utils.sortingOrderDefault)
            {
                return CreateWorldText(parent, text, localPosition, fontSize, color, textAnchor, textAlignment, sortingOrder);
            }
            // Create Text in the World
            public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color? color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
            {
                if (color == null) color = Color.white;

                GameObject gameObject = new GameObject("LC_WorldText", typeof(TextMesh));
                Transform transform = gameObject.transform;

                #region Custom conf - jLautaroCabral

                transform.localScale = Vector3.one * 0.1f;

                #endregion Custom conf - jLautaroCabral

                transform.SetParent(parent, false);
                transform.localPosition = localPosition;

                TextMesh textMesh = gameObject.GetComponent<TextMesh>();
                textMesh.anchor = textAnchor;
                textMesh.alignment = textAlignment;
                textMesh.text = text;
                textMesh.fontSize = fontSize;
                textMesh.color = (Color)color;
                textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

                _3OGS_Debug.RuntimeDebugObjectsManager.debugObjects.Add(gameObject);

                return textMesh;
            }


            public static GameObject CreateWorldSprite(Transform parent, string name, Sprite sprite, Vector3 position, Vector3 localScale, Color color)
            {
                return CreateWorldSprite(parent, name, sprite, position, localScale, 0, color);
            }

            // Create a Sprite in the World, no parent
            public static GameObject CreateWorldSprite(string name, Sprite sprite, Vector3 position, Vector3 localScale, Color color)
            {
                return CreateWorldSprite(null, name, sprite, position, localScale, 0, color);
            }

            // Create a Sprite in the World, no parent
            public static GameObject CreateWorldSprite(string name, Sprite sprite, Vector3 position, Vector3 localScale, int sortingOrder, Color color)
            {
                return CreateWorldSprite(null, name, sprite, position, localScale, sortingOrder, color);
            }

            // Create a Sprite in the World
            public static GameObject CreateWorldSprite(Transform parent, string name, Sprite sprite, Vector3 localPosition, Vector3 localScale, int sortingOrder, Color color)
            {
                GameObject gameObject = new GameObject(name, typeof(SpriteRenderer));
                Transform transform = gameObject.transform;
                transform.SetParent(parent, false);
                transform.localPosition = localPosition;
                transform.localScale = localScale;
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite;
                spriteRenderer.sortingOrder = sortingOrder;
                spriteRenderer.color = color;

                _3OGS_Debug.RuntimeDebugObjectsManager.debugObjects.Add(gameObject);
                return gameObject;
            }

            public static Text DrawTextUI(string textString, Vector2 anchoredPosition, int fontSize, Font font)
            {
                return DrawTextUI(textString, _3OGS_Utils.GetCanvasTransform(), anchoredPosition, fontSize, font);
            }

            public static Text DrawTextUI(string textString, Transform parent, Vector2 anchoredPosition, int fontSize, Font font)
            {
                GameObject textGo = new GameObject("LC_UIText", typeof(RectTransform), typeof(Text));
                textGo.transform.SetParent(parent, false);
                Transform textGoTrans = textGo.transform;
                textGoTrans.SetParent(parent, false);
                textGoTrans.localPosition = Vector3.zero;
                textGoTrans.localScale = Vector3.one;

                RectTransform textGoRectTransform = textGo.GetComponent<RectTransform>();
                textGoRectTransform.sizeDelta = new Vector2(0, 0);
                textGoRectTransform.anchoredPosition = anchoredPosition;

                Text text = textGo.GetComponent<Text>();
                text.text = textString;
                text.verticalOverflow = VerticalWrapMode.Overflow;
                text.horizontalOverflow = HorizontalWrapMode.Overflow;
                text.alignment = TextAnchor.MiddleLeft;
                if (font == null) font = _3OGS_Utils.GetDefaultFont();
                text.font = font;
                text.fontSize = fontSize;

                _3OGS_Debug.RuntimeDebugObjectsManager.debugObjects.Add(textGo);

                return text;
            }

            // Create Text Updater in UI
            public static FunctionUpdater CreateUITextUpdater(Func<string> GetTextFunc, Vector2 anchoredPosition)
            {
                Text text = DrawTextUIAnchoredPos(GetTextFunc(), anchoredPosition, 20, _3OGS_Utils.GetDefaultFont());
                return FunctionUpdater.Create(() => {
                    text.text = GetTextFunc();
                    return false;
                }, "UITextUpdater");
            }

            /*

            public static void CreateUITextPopup(string textString, Vector3 position)
            {
                CreateUITextPopup(textString, position, position + new Vector3(0, 20), Color.white);
            }

            public static void CreateUITextPopup(string textString, Vector3 position, Vector3 finalPopupPosition, Color color, int fontSize = 40, float popupTime = 1f)
            {
                Text text = DrawTextUI(textString, position, fontSize, GetDefaultFont());
                Transform transform = text.transform;
                Vector3 moveAmount = (finalPopupPosition - position) / popupTime;

                FunctionUpdater.Create(delegate () {
                    transform.position += moveAmount * Time.deltaTime;
                    popupTime -= Time.deltaTime;
                    if (popupTime <= 0f)

                    {
                        UnityEngine.Object.Destroy(transform.gameObject);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }, "UITextPopup"); ;
            }

            */

            public static Text DrawTextUIAnchoredPos(string textString, Vector2 anchoredPosition, int fontSize, Font font)
            {
                return DrawTextUIWithAnchoredPos(textString, _3OGS_Utils.GetCanvasTransform(), anchoredPosition, fontSize, font);
            }

            public static Text DrawTextUIWithAnchoredPos(string textString, Transform parent, Vector2 anchoredPosition, int fontSize, Font font)
            {
                GameObject textGo = new GameObject("Text", typeof(RectTransform), typeof(Text));
                textGo.transform.SetParent(parent, false);
                Transform textGoTrans = textGo.transform;
                textGoTrans.SetParent(parent, false);
                textGoTrans.localPosition = Vector3.zero;
                textGoTrans.localScale = Vector3.one;

                RectTransform textGoRectTransform = textGo.GetComponent<RectTransform>();
                textGoRectTransform.sizeDelta = new Vector2(0, 0);
                textGoRectTransform.anchoredPosition = anchoredPosition;

                Text text = textGo.GetComponent<Text>();
                text.text = textString;
                text.verticalOverflow = VerticalWrapMode.Overflow;
                text.horizontalOverflow = HorizontalWrapMode.Overflow;
                text.alignment = TextAnchor.MiddleLeft;
                if (font == null) font = _3OGS_Utils.GetDefaultFont();
                text.font = font;
                text.fontSize = fontSize;

                return text;
            }

        }
        public static class RuntimeDebugObjectsManager
        {
            public static List<GameObject> debugObjects = new List<GameObject>();
            private static bool hidingObjects;
            public static void DisableDebugObjects()
            {
                hidingObjects = true;
                foreach (GameObject debugObj in debugObjects)
                    debugObj.SetActive(false);
            }
            public static void EnableDebugObjects()
            {
                hidingObjects = false;
                foreach (GameObject debugObj in debugObjects)
                    debugObj.SetActive(true);
            }
            public static void HandleDebugObjectsFarFromCamera()
            {
                foreach (GameObject debugObj in debugObjects)
                {
                    if(Vector3.Distance(debugObj.transform.position, CameraSwitcher.CurrentCamera.transform.position) > _3OGS_Utils.GetSettings().DebugObjectsDrawDistance)
                    {
                        debugObj.SetActive(false);
                    } else
                    {
                        if (!hidingObjects) debugObj.SetActive(true);
                    }
                        
                }
            }
        }

        /*
        // Text Updater always following mouse
        public static void MouseWorldTextUpdater(Func<string> GetTextFunc, Vector3 positionOffset = default(Vector3))
        {
            GameObject gameObject = new GameObject();
            FunctionUpdater.Create(() => {
                gameObject.transform.position = Utils.LC_UtilsClass.GetMouseWorldPosition() + positionOffset;
                return false;
            });
            TextUpdater(GetTextFunc, Vector3.zero, gameObject.transform);
        }
        */
        /*
                public static GameObject CreateDebugButton(string btnLabel, Vector3 position, Action onClickFunction, bool rotateToCamera = true)
                {
                    return CreateDebugButton(null, btnLabel, position, onClickFunction, rotateToCamera);
                }

                public static GameObject CreateDebugButton(Transform parent, string btnLabel, Vector3 position, Action onClickFunction, bool rotateToCamera = true)
        */
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
        /*        
        // World text pop up at mouse position
        public static void UITextPopupMouse(string text)
        {
            Utils.LC_Utils.CreateUITextPopup(text, Utils.LC_Utils.GetMousePosition());
        }
        */
        /*
        public static void UIAnchoredTextPopupMouse(string text)
        {
            Utils.LC_Utils.DrawTextUIAnchoredPos(text, Utils.LC_Utils.GetMousePosition());
        }
        */
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

    }
}

