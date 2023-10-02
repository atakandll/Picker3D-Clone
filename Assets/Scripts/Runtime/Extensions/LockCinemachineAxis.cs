using Cinemachine;
using UnityEngine;

namespace Runtime.Extensions
{
    [ExecuteInEditMode]
    [SaveDuringPlay]
    [AddComponentMenu("")]
    public class LockCinemachineAxis : CinemachineExtension
    {
        [Tooltip("Lock the Cinemachine Virtual Camera's X Axis position with this specific value")]
        public float XClampValue;
        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Body)
            {
                var position = state.RawPosition;
                position.x = XClampValue;
                state.RawPosition = position;
            }
           
           
        }
    }
}