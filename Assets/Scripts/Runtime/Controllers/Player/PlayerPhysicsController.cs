﻿using System;
using DG.Tweening;
using Runtime.Controllers.Pool;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private new Collider collider;

        #endregion

        #region Private Variables

        private readonly string _stageArea = "StageArea";
        private readonly string _finish = "FinishArea";
        private readonly string _miniGame = "MiniGameArea";

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_stageArea))
            {
                manager.ForceCommand.Execute(); 
                CoreGameSignals.Instance.onStageAreaEntered?.Invoke();
                InputSignals.Instance.onDisableInput?.Invoke(); // karakter artık hareket etmeyecek

                DOVirtual.DelayedCall(3, () =>
                {
                    var result = other.transform.parent.GetComponentInChildren<PoolController>()
                        .TakeResult(manager.StageValue);

                    if (result) // başarılı olma durumu
                    {
                        CoreGameSignals.Instance.onStageAreaSuccesful?.Invoke(manager.StageValue);
                        InputSignals.Instance.onEnableInput?.Invoke();
                    }
                    else // başarısız olma durumu
                    {
                        CoreGameSignals.Instance.onLevelFailed?.Invoke();
                    }
                    
                });
                return;

            }

            if (other.CompareTag(_finish))
            {
                CoreGameSignals.Instance.onFinishAreaEntered?.Invoke();
                InputSignals.Instance.onDisableInput?.Invoke();
                CoreGameSignals.Instance.onLevelSuccesful?.Invoke();
                return;
            }

            if (other.CompareTag(_miniGame))
            {
                //write the mini game
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var transform1 = manager.transform;
            var position1 = transform1.position;
            
            Gizmos.DrawSphere(new Vector3(position1.x, position1.y - 1f, position1.z + .9f), 1.7f);
        }

        public void OnReset()
        {
            
        }
    }
}