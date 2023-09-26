using System;
using Data.UnityObjects;
using Data.ValueObjects;
using Signals;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private InputData _data;
        private bool _isAvailableForTouch, _isFirstTimeTouchTaken, _isTouching;

        private float _currentVelocity;
        private float _moveVector;
        private Vector2? _mousePosition;

        #endregion

        #endregion

        private void Awake()
        {
            _data = GetInputData();
        }

        private InputData GetInputData()
        {
            return Resources.Load<CD_Input>("Data/CD_Input").Data;
        }

        private void OnEnable()
        {
            SubscribeEvent();
        }

        private void SubscribeEvent()
        {
            CoreGameSignals.Instance.onReset += OnReset;
            InputSignals.Instance.onEnableInput += OnEnableInput;
            InputSignals.Instance.onDisableInput += OnDisableInput;

        }

        public void OnDisableInput()
        {
            _isAvailableForTouch = false;
        }

        private void OnEnableInput()
        {
            _isAvailableForTouch = true;

        }

        private void OnReset()
        {
            _isAvailableForTouch = false;
            _isFirstTimeTouchTaken = false;
            _isTouching = false;
        }
        private void UnSubscribeEvent()
        {
            CoreGameSignals.Instance.onReset -= OnReset;
            InputSignals.Instance.onEnableInput -= OnEnableInput;
            InputSignals.Instance.onDisableInput -= OnDisableInput;

        }

        private void OnDisable()
        {
            UnSubscribeEvent();
        }

        private void Update()
        {
            if (!_isAvailableForTouch) return;

            if (Input.GetMouseButtonUp(0) && !IsPointerOverUIElement()) // UI kısmına değmiyorsam
            {
                _isTouching = false;
            }
        }
    }
}