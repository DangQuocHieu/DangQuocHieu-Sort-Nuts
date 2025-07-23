using UnityEngine;

public class UIScreenManager : MonoBehaviour, IMessageHandle
{
    [SerializeField] private GameplayScreenHUD _gameplayScreen;
    [SerializeField] private LevelCompleteScreenHUD _levelCompleteScreen;

    void OnEnable()
    {
        MessageManager.AddSubscriber(GameMessageType.OnLevelCompleted, this);
    }

    void OnDisable()
    {
        MessageManager.RemoveSubscriber(GameMessageType.OnLevelCompleted, this);
    }

    public void Handle(Message message)
    {
        switch (message.type)
        {
            case GameMessageType.OnLevelCompleted:
                _levelCompleteScreen.gameObject.SetActive(true);
                break;
        }
    }
}
