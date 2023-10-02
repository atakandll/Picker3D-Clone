using System;
using Runtime.Enums;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class UIManager : MonoBehaviour
    {
        private void OnEnable()
        {
            SubscribeEvent();
        }

        private void SubscribeEvent()
        {
            CoreGameSignals.Instance.onLevelInitialize += OnLevelInitialize;
            CoreGameSignals.Instance.onLevelSuccesful += OnLevelSuccesful;
            CoreGameSignals.Instance.onLevelFailed += OnLevelFailed;
            CoreGameSignals.Instance.onReset += OnReset;
        }

        private void OnLevelFailed()
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Fail, 2); // buradaki sayılar layerları temsil ediyor. Layer artıkça ekranda önde oluyor canvaslarda
        }

        private void OnLevelSuccesful()
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Win, 2);
        }

        private void OnLevelInitialize(byte arg0)
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Level, 0); // levelin layerı 0 olarak atadık en arkada level ekranı gozukucek
            UISignals.Instance.onSetLevelValue?.Invoke((byte)CoreGameSignals.Instance.onGetLevelValue?.Invoke());
            
        }

        private void OnReset()
        {
            CoreUISignals.Instance.onCloseAllPanels?.Invoke();
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start, 1);
        }
        private void UnSubscribeEvent()
        {
            CoreGameSignals.Instance.onLevelInitialize -= OnLevelInitialize;
            CoreGameSignals.Instance.onLevelSuccesful -= OnLevelSuccesful;
            CoreGameSignals.Instance.onLevelFailed -= OnLevelFailed;
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void OnDisable()
        {
            UnSubscribeEvent();
        }
        public void Play()
        {
            UISignals.Instance.onPlay?.Invoke();
            CoreUISignals.Instance.onClosePanel?.Invoke(1); // startı kapattık
            InputSignals.Instance.onEnableInput?.Invoke(); // artık input alabiliriz
            CameraSignals.Instance.onSetCameraTarget?.Invoke();
            
        }

        public void NextLevel() // buttonlar için
        {
            CoreGameSignals.Instance.onNextLevel?.Invoke();
        }

        public void RestartLevel()
        {
            CoreGameSignals.Instance.onRestartLevel?.Invoke();
        }
    }
}