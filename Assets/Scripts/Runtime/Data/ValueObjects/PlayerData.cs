using System;
using Unity.Mathematics;
using UnityEngine.Serialization;

namespace Runtime.Data.ValueObjects
{
    [Serializable]
    public struct PlayerData
    {
        public PlayerMovementData MovementData;
        public PlayerMeshData MeshData;
        public PlayerForceData ForceData;

    }

    [Serializable]
    public struct PlayerMovementData
    {
        public float ForwardSpeed;
        public float SidewaySpeed;
    }

    [Serializable]
    public struct PlayerMeshData
    {
        public float ScaleCounter;
    }

    [Serializable]
    public struct PlayerForceData
    {
        [FormerlySerializedAs("ForceParametres")] public float3 ForceParameters;
    }
}