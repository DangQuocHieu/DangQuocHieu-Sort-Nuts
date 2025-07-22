using System.Linq;
using UnityEngine;

public class LevelManager : PersistentSingleton<LevelManager>, IMessageHandle
{
    [SerializeField] private GameObject[] _levelPrefab;
    [SerializeField] private int _currentLevelIndex = 0;
    public int CurrentLevelIndex => _currentLevelIndex;

    void OnEnable()
    {
        MessageManager.AddSubscriber(GameMessageType.OnLevelCompleted, this);
        SetUpLevel();
    }

    void OnDisable()
    {
        MessageManager.RemoveSubscriber(GameMessageType.OnLevelCompleted, this);
    }
    private void GoToNextLevel()
    {
        _currentLevelIndex = (_currentLevelIndex + 1) % _levelPrefab.Count();
    }

    private void SetUpLevel()
    {
        GameObject currentLevelObject = _levelPrefab[_currentLevelIndex];
        Instantiate(currentLevelObject);
    }

    public void Handle(Message message)
    {
        switch (message.type)
        {
            case GameMessageType.OnLevelCompleted:
                GoToNextLevel();
                break;
        }
    }
}
