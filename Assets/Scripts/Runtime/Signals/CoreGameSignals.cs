using System;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CoreGameSignals : MonoBehaviour
    {
        #region Singleton

        public static CoreGameSignals Instance;
        

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
        
        public UnityAction<byte> onLevelInitialize = delegate{  }; // action olanlar void ve parametre almayan demek, başlangıçta hiçbirşey yapmayan bir delegate e eşitledik.
        public UnityAction onClearActiveLevel = delegate{  };
        public UnityAction onNextLevel = delegate{  };
        public UnityAction onRestartLevel = delegate{  };
        public UnityAction onReset = delegate{  };
        
        public Func<byte> onGetLevelValue = delegate { return 0; }; // değer döndüren(func olarak yazılır) parametre de alıyor func
    }
}