using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayScreenHUD : MonoBehaviour, IMessageHandle
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private Button _volumeToggleButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private TextMeshProUGUI _levelText;

    [Header("Toggle Button Sprite")]
    [SerializeField] private Sprite _volumeOnSprite;
    [SerializeField] private Sprite _volumeOffSprite;

    void OnEnable()
    {
        MessageManager.AddSubscriber(GameMessageType.OnLevelStart, this);
        AddButtonListener();
    }

    void OnDisable()
    {
        MessageManager.RemoveSubscriber(GameMessageType.OnLevelStart, this);
        RemoveButtonListener();
    }

    void Update()
    {
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        if (CurrencyManager.Instance != null)
        {
            _moneyText.text = CurrencyManager.Instance.TotalMoney.ToString();
        }

    }
    private void AddButtonListener()
    {
        _volumeToggleButton.onClick.AddListener(() =>
        {
            bool isMuted = AudioManager.Instance.ToggleVolume();
            if (isMuted)
            {
                _volumeToggleButton.image.sprite = _volumeOffSprite;
            }
            else _volumeToggleButton.image.sprite = _volumeOnSprite;
        });
        _restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        });
    }

    private void RemoveButtonListener()
    {
        _volumeToggleButton.onClick.RemoveAllListeners();
        _restartButton.onClick.RemoveAllListeners();
    }

    public void Handle(Message message)
    {
        switch (message.type)
        {
            case GameMessageType.OnLevelStart:
                OnLevelStart((int)message.data[0]);
                break;
        }
    }

    private void OnLevelStart(int currentLevelIndex)
    {
        _levelText.text = "Level " + (currentLevelIndex + 1).ToString();
    }
}
