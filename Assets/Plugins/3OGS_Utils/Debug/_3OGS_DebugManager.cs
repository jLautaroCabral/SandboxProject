using _3OGS.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _3OGS.Debug
{
    [DefaultExecutionOrder(-1000)]
    public class _3OGS_DebugManager : MonoBehaviour
    {
        private static _3OGS_DebugManager _sharedInstance;
        public static _3OGS_DebugManager SharedInstance
        { get 
            { 
                if(_sharedInstance == null)
                {
                    /*_3OGS_DebugManager newDebugManager = new GameObject("3OGS_DebugManager", typeof(_3OGS_DebugManager)).GetComponent<_3OGS_DebugManager>();
                    newDebugManager.Settings = _3OGS_DebuggerConfig.CreateDefaultSettings();
                    _sharedInstance = newDebugManager;*/
                }
                return _sharedInstance;
            }
          set { _sharedInstance = value; } 
        }

        public _3OGS_DebuggerConfig Settings;
        [HideInInspector]
        public GameObject EmptyObject;

        private void Awake()
        {
            if (SharedInstance != null)
                Destroy(this);
            else
                SharedInstance = this;

            EmptyObject = new GameObject("LC_EmptyObject");
            EmptyObject.transform.SetParent(this.transform);

            DontDestroyOnLoad(this.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            CameraSwitcher.SetupCurrentCamera(_3OGS_Utils.GetCurrentCamera());
            FunctionUpdater.Create(_3OGS_Debug.RuntimeDebugObjectsManager.HandleDebugObjectsFarFromCamera);
        }
    }
}