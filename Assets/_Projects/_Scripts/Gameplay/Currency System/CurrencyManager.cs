using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>, IMessageHandle
{
    [SerializeField] private int _totalMoney;
    public int TotalMoney => _totalMoney;

    void OnEnable()
    {
        MessageManager.AddSubscriber(GameMessageType.OnBoltCompleted, this);
    }

    void OnDisable()
    {
        MessageManager.RemoveSubscriber(GameMessageType.OnBoltCompleted, this);
    }

    public void AddMoney(int moneyToAdd)
    {
        _totalMoney += moneyToAdd;
    }

    public void Handle(Message message)
    {
        switch (message.type)
        {
            case GameMessageType.OnBoltCompleted:
                AddMoney(1);
                break;
        }
    }
}
