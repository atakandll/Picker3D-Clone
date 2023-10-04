
using Runtime.Commands.Player;
using Runtime.Controllers.Player;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public byte StageValue;
        
        internal ForceBallsToPoolCommand ForceCommand;
        

        #endregion

        #region Serialized Variables

        [SerializeField] private PlayerMovementController movementController;
        [SerializeField] private PlayerMeshController meshController;
        [SerializeField] private PlayerPhysicsController physicsController;

        #region Private Variables

        private PlayerData _data;

        #endregion

        
        #endregion

        #endregion

        private void Awake()
        {
            _data = GetPlayerData();
            SendDataToControllers();
            Init();
        }
        

        private PlayerData GetPlayerData()
        {
            return Resources.Load<CD_Player>("Data/CD_Player").Data;
        }
        private void SendDataToControllers()
        {
            movementController.SetData(_data.MovementData);
            meshController.SetData(_data.MeshData);
            
        }
        
        private void Init()
        {
            ForceCommand = new ForceBallsToPoolCommand(this, _data.ForceData);
        }

        private void OnEnable()
        {
            SubscribeEvent();
        }

        private void SubscribeEvent()
        {
            InputSignals.Instance.onInputTaken += () => movementController.IsReadyToMove(true);
            InputSignals.Instance.onInputReleased += () => movementController.IsReadyToMove(false);
            InputSignals.Instance.onInputDragged += OnInputDragged;
            UISignals.Instance.onPlay += () => movementController.IsReadyToPlay(true);
            CoreGameSignals.Instance.onLevelSuccesful += () => movementController.IsReadyToPlay(false);
            CoreGameSignals.Instance.onLevelFailed += () => movementController.IsReadyToPlay(false);
            CoreGameSignals.Instance.onStageAreaEntered += () => movementController.IsReadyToPlay(false);
            CoreGameSignals.Instance.onStageAreaSuccesful += OnStageAreaSuccesful;
            CoreGameSignals.Instance.onFinishAreaEntered += OnFinishAreaEntered;
            CoreGameSignals.Instance.onReset += OnReset;
        }
        
        private void OnInputDragged(HorizantalInputParams inputParams)
        {
            movementController.UpdateInputParams(inputParams);
        }

        private void OnStageAreaSuccesful(byte value)
        {
            StageValue = (byte)++value;
            
            movementController.IsReadyToPlay(true);
            
            meshController.ScaleUpPlayer();
            meshController.PlayConfetti();
            meshController.ShowUpText();
        }
        private void OnFinishAreaEntered()
        {
            CoreGameSignals.Instance.onLevelSuccesful?.Invoke();
            //MiniGame yazılmalı
            
        }
        private void OnReset()
        {
            StageValue = 0;
            movementController.OnReset();
            physicsController.OnReset();
            meshController.OnReset();
        }
        private void UnSubscribeEvent()
        {
            InputSignals.Instance.onInputTaken -= () => movementController.IsReadyToMove(true);
            InputSignals.Instance.onInputReleased -= () => movementController.IsReadyToMove(false);
            InputSignals.Instance.onInputDragged -= OnInputDragged;
            UISignals.Instance.onPlay -= () => movementController.IsReadyToPlay(true);
            CoreGameSignals.Instance.onLevelSuccesful -= () => movementController.IsReadyToPlay(false);
            CoreGameSignals.Instance.onLevelFailed -= () => movementController.IsReadyToPlay(false);
            CoreGameSignals.Instance.onStageAreaEntered -= () => movementController.IsReadyToPlay(false);
            CoreGameSignals.Instance.onStageAreaSuccesful -= OnStageAreaSuccesful;
            CoreGameSignals.Instance.onFinishAreaEntered -= OnFinishAreaEntered;
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void OnDisable()
        {
            UnSubscribeEvent();
        }
        
        //isReadyToMove da yatay yönlü hareket true oluyor
        //isReadyToPlay da yatay ve dikey yönlü hareket true oluyor
    }
}