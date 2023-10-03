using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Runtime.Signals;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Runtime.Controllers.Pool
{
    public class PoolController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private List<DOTweenAnimation> tweens = new List<DOTweenAnimation>();
        [SerializeField] private TextMeshPro poolText;
        [SerializeField] private byte stageID;
        [SerializeField] private new Renderer renderer;
        [SerializeField] private float3 poolAfterColor = new float3(0.1607843f, 0.3144797f, 0.6039216f);

        #endregion

        #region Private Variables

        private PoolData _data;
        private byte _collectableCount;
        private readonly string collectable = "Collectable";

        #endregion

        #endregion

        private void Awake()
        {
            _data = GetPoolData();
        }

        private PoolData GetPoolData()
        {
            return Resources.Load<CD_Level>("Data/CD_Level")
                .Levels[(int)CoreGameSignals.Instance.onGetLevelValue?.Invoke()].Pools[stageID];

        }

        private void OnEnable()
        {
            SubscribeEvent();
        }

        private void SubscribeEvent()
        {
            CoreGameSignals.Instance.onStageAreaSuccesful += OnActiveTweens;
            CoreGameSignals.Instance.onStageAreaSuccesful += OnChangePoolColor;
        }
        

        private void OnActiveTweens(byte stageValue)
        {
            if(stageValue != stageID) return;
            
            foreach (var tween in tweens)
            {
                tween.DOPlay();
            }

        }
        private void OnChangePoolColor(byte stageValue)
        {
            if(stageValue != stageID) return;
            renderer.material.DOColor(new Color(poolAfterColor.x, poolAfterColor.y,poolAfterColor.z), .5f).SetEase(Ease.Flash).SetRelative(false);

        }

        private void Start()
        {
            SetRequiredAmountText();
        }

        private void SetRequiredAmountText()
        {
            poolText.text = $"0/{_data.RequiredObjectCount}";
        }

        public bool TakeResult(byte managerStageValue)
        {
            if (stageID == managerStageValue)
            {
                return _collectableCount >= _data.RequiredObjectCount;
            }

            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag(collectable)) return;

            IncreaseCollectedAmount();
            SetCollectedAmountToPool();
        }

        private void SetCollectedAmountToPool()
        {
            poolText.text = $"{_collectableCount}/{_data.RequiredObjectCount}";
        }

        private void IncreaseCollectedAmount()
        {
            _collectableCount++;
        }

        private void OnTriggerExit(Collider other)
        {
            if(!other.CompareTag(collectable)) return;
            DecreaseCollectedAmount();
            SetCollectedAmountToPool();
        }

        private void DecreaseCollectedAmount()
        {
            _collectableCount--;
        }
    }
}