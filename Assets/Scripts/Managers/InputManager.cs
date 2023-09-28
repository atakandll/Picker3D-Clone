using System.Collections.Generic;
using Data.UnityObjects;
using Data.ValueObjects;
using Keys;
using Signals;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private InputData _data;
        private bool _isAvailableForTouch, _isFirstTimeTouchTaken, _isTouching;

        private float _currentVelocity;
        private float3 _moveVector;
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
            //_isFirstTimeTouchTaken = false;
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

            if (Input.GetMouseButtonUp(0) && !IsPointerOverUIElement()) // UI kısmına değmiyorsam, elimizi çektiğimiz kısım
            {
                _isTouching = false;
                InputSignals.Instance.onInputReleased?.Invoke(); 
                Debug.LogWarning("Executed -----> OnIputReleased");
                
                
            }

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement()) // ilk tıkladığım an
            {
                _isTouching = true;
                InputSignals.Instance.onInputTaken?.Invoke();
                Debug.LogWarning("Executed -----> OnIputTaken");
                
                if (!_isFirstTimeTouchTaken)
                {
                    _isFirstTimeTouchTaken = true; // bir daha bu döngüye girmemek için
                    InputSignals.Instance.onFirstTimeTouchTaken?.Invoke();
                    Debug.LogWarning("Executed -----> OnFirstTimeTouchTaken");
                    
                }

                _mousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0) && !IsPointerOverUIElement())
            {
                if (_isTouching)
                {
                    if (_mousePosition != null)
                    {
                        Vector2 mouseDeltaPos = (Vector2)Input.mousePosition - _mousePosition.Value; // it calculates the difference in mouse position from the previous frame 
                        
                        if (mouseDeltaPos.x > _data.HorizontalInputSpeed)
                        {
                            _moveVector.x = _data.HorizontalInputSpeed / 10f * mouseDeltaPos.x;
                        }
                        else if (mouseDeltaPos.x < _data.HorizontalInputSpeed)
                        {
                            _moveVector.x = -_data.HorizontalInputSpeed / 10f * mouseDeltaPos.x;
                        }
                        else // hissiyat için yaptık, yavaş yavaş 0 olucak.
                        {
                            _moveVector.x = Mathf.SmoothDamp(_moveVector.x, 0, ref _currentVelocity, _data.ClampSpeed);
                        }

                        _mousePosition = Input.mousePosition;
                        
                        InputSignals.Instance.onInputDragged?.Invoke(new HorizantalInputParams()
                        {
                            HorizantalValue = _moveVector.x,
                            ClampValues =   _data.ClampValues
                            
                        });
                        
                    }
                }
            }
        }

        private bool IsPointerOverUIElement() // UI üzerinde mi
        {
            ///<summary>
            /// This object is used to store data about a pointer event (like a mouse click or touch).
            /// It's initialized with the current EventSystem.
            /// var results = new List<RaycastResult>():
            /// This line creates a new list called results that will store information about what was hit by a raycast.
             ///</summary>>
             
            var eventData = new PointerEventData(EventSystem.current) // Bu nesne, işaretçi olayı hakkında veri saklar (örneğin fare tıklaması veya dokunmatik).
            {
                position = Input.mousePosition // Oluşturulan eventData nesnesinin pozisyonu, mevcut fare pozisyonuna ayarlanır.
            };
            var results = new List<RaycastResult>(); //Bu liste, bir raycast işlemi tarafından vurulan nesneler hakkında bilgi saklar.
            EventSystem.current.RaycastAll(eventData, results); //vurulan nesnelerin bilgisini results listesine ekler.
            return results.Count > 0; // Eğer en az bir nesne vurulmuşsa,
        }
    }
}