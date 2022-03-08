using _3OGS.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _3OGS.Debug
{
    public class DebugButton : MonoBehaviour
    {
        [Header("Button Settings")]
        public Color backgroundColor;
        public Color backgroundOnClickColor;
        public Color backgroundOnOverColor;

        [Header("References")]
        public SpriteRenderer backgroundSprite;
        public TextMesh buttonLabel;

        public Action OnClickAction;

        private void OnMouseDown()
        {
            backgroundSprite.color = backgroundOnClickColor;
        }
        private void OnMouseEnter()
        {
            backgroundSprite.color = backgroundOnOverColor;
        }
        private void OnMouseExit()
        {
            backgroundSprite.color = backgroundColor;
        }
        private void OnMouseUp()
        {
            backgroundSprite.color = backgroundColor;
            if (OnClickAction != null)
                OnClickAction();
            else
                _3OGS_Debug.TextPopup("This button is not assigned a function", transform.position + transform.right * (_3OGS_Utils.GetDefaultButtonSize().x / 2 + _3OGS_Utils.GetDefaultButtonPanelOffset()));
        }
        public void SetButtonLabelText(string text)
        {
            buttonLabel.text = text;
        }
    }
}
