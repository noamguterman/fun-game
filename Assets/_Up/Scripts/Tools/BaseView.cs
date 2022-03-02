using UnityEngine;

namespace Assets._Scripts.Tools
{
    public class BaseView : MonoBehaviour
    {
        private float CameraWidth = 1280;
        private float CameraHeight = 720;

        protected virtual void Start()
        {

            var cameraSize = GetComponent<Camera>().orthographicSize;
            CameraWidth = CameraWidth/4*cameraSize;
            CameraHeight = CameraHeight/4*cameraSize;
            Resources.UnloadUnusedAssets();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            GetComponent<Camera>().orthographicSize = cameraSize;
            GetComponent<Camera>().aspect = CameraWidth/CameraHeight;

        }
    }
}
