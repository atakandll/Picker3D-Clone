using System.Linq;
using System.Security.Cryptography;
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
            var transform1 = _manager.transform;
            var position1 = transform1.position;
            var forcePos = new Vector3(position1.x, position1.y + 1f, position1.z + 1f);

            var collider = Physics.OverlapSphere(forcePos, 1.35f);
            
            var collectableColliderList = collider.Where(col => col.CompareTag("Collectable")).ToList();

            foreach (var col in collectableColliderList)
            {
                if(col.GetComponent<Rigidbody>() == null) continue;
                
                var rb = col.GetComponent<Rigidbody>();
                
                rb.AddForce(new Vector3(0, _forceData.ForceParametres.y, _forceData.ForceParametres.z),
                    ForceMode.Impulse);

            }
            collectableColliderList.Clear();




        }
    }
}