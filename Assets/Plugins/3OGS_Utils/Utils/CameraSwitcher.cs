using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _3OGS.Utils
{
    public class CameraSwitcher
    {
        public static Camera CurrentCamera { get; private set; }
        private static Camera previousCamera;
        private static Action returnToPreviousCameraAction;
        public static void SetupCurrentCamera(Camera camera)
        {
            CurrentCamera = camera;
            Camera.SetupCurrent(camera);
        }
        public static void SetupReturnAction(Action returnToPreviousCameraAction)
        {
            previousCamera = CurrentCamera;
            CameraSwitcher.returnToPreviousCameraAction = returnToPreviousCameraAction;
        }
        public static void SetupReturnAction(Camera camera, Action returnToPreviousCameraAction)
        {
            previousCamera = camera;
            CameraSwitcher.returnToPreviousCameraAction = returnToPreviousCameraAction;
        }
        public static void ReturnToOriginalCamera()
        {
            SetupCurrentCamera(previousCamera);
            returnToPreviousCameraAction();
        }
        //public 
    }
}

