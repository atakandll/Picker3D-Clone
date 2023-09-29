using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CameraSignals : MonoBehaviour
    {
        #region Singleton

        public static CameraSignals Instance;
        

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        #endregion
        
        public UnityAction onSetCameraTarget = delegate {  };
    }
}