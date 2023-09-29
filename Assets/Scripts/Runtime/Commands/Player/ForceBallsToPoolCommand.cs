using Runtime.Data.ValueObjects;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Commands.Player
{
    public class ForceBallsToPoolCommand : MonoBehaviour
    {
        private PlayerManager _manager;
        private PlayerForceData _forceData;
        public ForceBallsToPoolCommand(PlayerManager manager, PlayerForceData forceData)
        {
            _manager = manager;
            _forceData = forceData;
        }

        internal void Execute()
        {
            
        }
    }
}