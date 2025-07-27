using UnityEngine;

public class CurrencyManager : PersistentSingleton<CurrencyManager>, IMessageHandle, ISaveable
{
    [SerializeField] private int _totalMoney;
    public int TotalMoney => _totalMoney;

    void Start()
    {
        _totalMoney = DataManager.Instance.PlayerData.TotalMoney;
    }

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

    public void SaveData(PlayerData data)
    {
        data.TotalMoney = _totalMoney;
    }

    public void LoadData(PlayerData data)
    {
        _totalMoney = data.TotalMoney;
    }
}
