using UnityEngine;

public class UIMoneyPanel : MonoBehaviour, IMessageHandle
{
    [SerializeField] private string _gainMoneyState = "GainMoney";
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        MessageManager.AddSubscriber(GameMessageType.OnBoltCompleted, this);
    }

    void OnDisable()
    {
        MessageManager.RemoveSubscriber(GameMessageType.OnBoltCompleted, this);
    }

    public void Handle(Message message)
    {
        switch (message.type)
        {
            case GameMessageType.OnBoltCompleted:
                _animator.Play(_gainMoneyState, -1, 0f);
                break;
        }
    }
}
