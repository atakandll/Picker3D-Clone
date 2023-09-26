using UnityEngine;

namespace Commands.Level
{
    public class OnLevelLoaderCommand
    {
        private Transform _levelHolder;
        internal OnLevelLoaderCommand(Transform levelHolder)
        {
            _levelHolder = levelHolder;
        }

        internal void Execute(byte levelIndex) // level spawn
        { 
            Object.Instantiate(Resources.Load<GameObject>($"Prefabs/LevelPrefabs/level {levelIndex}"), _levelHolder, true); // level holderın childi oldu sondaki yaptığımıx 2 parametre ile.
        }
    }
}