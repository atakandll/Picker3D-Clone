
using Runtime.Commands.Level;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class LevelManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private Transform levelHolder;
        [SerializeField] private byte totalLevelCount;

        #endregion

        #region Private Variables

        private OnLevelLoaderCommand _levelLoaderCommand;
        private OnLevelDestroyerCommand _levelDestroyerCommand;
        private short _currentLevel;
        private LevelData _levelData;

        #endregion

        #endregion

        private void Awake()
        {
            _levelData = GetLevelData();
            _currentLevel = GetActiveLevel();
            
            Init();
        }

        private void Init()
        {
            _levelLoaderCommand = new OnLevelLoaderCommand(levelHolder);
            _levelDestroyerCommand = new OnLevelDestroyerCommand(levelHolder);
            
        }
        private LevelData GetLevelData()
        {
            return Resources.Load<CD_Level>( "Data/CD_Level").Levels[_currentLevel];
        }

        private byte GetActiveLevel()
        {
            return (byte)_currentLevel;
        }

        private void OnEnable()
        {
            SubscribeEvent();
        }
        

        private void SubscribeEvent()
        {
            CoreGameSignals.Instance.onLevelInitialize += _levelLoaderCommand.Execute;
            CoreGameSignals.Instance.onClearActiveLevel += _levelDestroyerCommand.Execute;
            
            CoreGameSignals.Instance.onGetLevelValue += OnGetLevelValue;
            CoreGameSignals.Instance.onNextLevel += OnNextLevel;
            CoreGameSignals.Instance.onRestartLevel += OnRestartLevel;
        }
        public byte OnGetLevelValue()
        {
            return (byte)_currentLevel;

        }
        private void OnNextLevel()
        {
            _currentLevel++;
            CoreGameSignals.Instance.onClearActiveLevel?.Invoke(); // mevcut leveli temizledik.
            CoreGameSignals.Instance.onReset?.Invoke();
            CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount)); // yeni level geldi
        }

        private void OnRestartLevel()
        {
            CoreGameSignals.Instance.onClearActiveLevel?.Invoke(); // mevcut level temizledik.
            CoreGameSignals.Instance.onReset?.Invoke();
            CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount)); // yeni level geldi
        }
        
        private void UnSubscribeEvent()
        {
            CoreGameSignals.Instance.onLevelInitialize -= _levelLoaderCommand.Execute;
            CoreGameSignals.Instance.onClearActiveLevel -= _levelDestroyerCommand.Execute;
            
            CoreGameSignals.Instance.onGetLevelValue -= OnGetLevelValue;
            CoreGameSignals.Instance.onNextLevel -= OnNextLevel;
            CoreGameSignals.Instance.onRestartLevel -= OnRestartLevel;
        }

        

        private void OnDisable()
        {
            UnSubscribeEvent();
        }

        private void Start()
        {
            //levelId sürekli artıcağından bizim level sayımızı geçmemesi için böyle bir yöntem oluşturuyoruz.
            CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
        }

        
    }
}