
using Runtime.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CoreUISignals : MonoBehaviour
    {
        #region Singleton

        public static CoreUISignals Instance;
        

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
        
        public UnityAction<UIPanelTypes, int> onOpenPanel = delegate { };
        public UnityAction<int> onClosePanel = delegate { };
        public UnityAction onCloseAllPanels = delegate { };
    }
}