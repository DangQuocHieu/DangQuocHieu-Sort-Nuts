using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : PersistentSingleton<LevelManager>, IMessageHandle, ISaveable
{
    [SerializeField] private GameObject[] _levelPrefab;
    [SerializeField] private int _currentLevelIndex = 0;
    public int CurrentLevelIndex => _currentLevelIndex;

    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }
    void OnEnable()
    {
        MessageManager.AddSubscriber(GameMessageType.OnLevelCompleted, this);
    }

    void OnDisable()
    {
        MessageManager.RemoveSubscriber(GameMessageType.OnLevelCompleted, this);
    }

    private IEnumerator GoToNextLevel()
    {
        _currentLevelIndex = (_currentLevelIndex + 1) % _levelPrefab.Count();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetUpLevel(Transform container)
    {
        GameObject currentLevelObject = _levelPrefab[_currentLevelIndex];
        Instantiate(currentLevelObject, container);
        MessageManager.SendMessage(new Message(GameMessageType.OnLevelStart, new object[] { _currentLevelIndex }));
    }

    public void Handle(Message message)
    {
        switch (message.type)
        {
            case GameMessageType.OnLevelCompleted:

                StartCoroutine(GoToNextLevel());
                break;
        }
    }

    public void SaveData(PlayerData data)
    {
        data.CurrentLevelIndex = _currentLevelIndex;
    }

    public void LoadData(PlayerData data)
    {
        _currentLevelIndex = data.CurrentLevelIndex;
    }
}
