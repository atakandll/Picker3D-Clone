using System;
using Runtime.Extensions;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CoreGameSignals : MonoSingleton<CoreGameSignals>
    {
        public UnityAction<byte> onLevelInitialize = delegate{  }; // action olanlar void ve parametre almayan demek, başlangıçta hiçbirşey yapmayan bir delegate e eşitledik.
        public UnityAction onClearActiveLevel = delegate{  };
        public UnityAction onLevelSuccesful = delegate{  };
        public UnityAction onLevelFailed = delegate{  };
        
        public UnityAction onNextLevel = delegate{  };
        public UnityAction onRestartLevel = delegate{  };
        public UnityAction onPlay = delegate { };
        public UnityAction onReset = delegate{  };
        
        public Func<byte> onGetLevelValue = delegate { return 0; }; // değer döndüren(func olarak yazılır) parametre de alıyor func
        public UnityAction onStageAreaEntered = delegate{  };
        public UnityAction<byte> onStageAreaSuccesful = delegate{  };
        public UnityAction onFinishAreaEntered = delegate{  };
    }
}