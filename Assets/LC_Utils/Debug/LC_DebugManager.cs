using LC.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LC.Debug
{
    [DefaultExecutionOrder(-1000)]
    public class LC_DebugManager : MonoBehaviour
    {
        public static LC_DebugManager sharedInstance;
        public LC_DebuggerConfig Settings;
        [HideInInspector]
        public GameObject EmptyObject;

        private void Awake()
        {
            if (sharedInstance != null)
                Destroy(this);
            else
                sharedInstance = this;

            EmptyObject = new GameObject("LC_EmptyObject");
            EmptyObject.transform.SetParent(this.transform);

            DontDestroyOnLoad(this.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            CameraSwitcher.SetupCurrentCamera(LC_Utils.GetCurrentCamera());
            FunctionUpdater.Create(LC_Debug.RuntimeDebugObjectsManager.HandleDebugObjectsFarFromCamera);
        }
    }
}