using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayScreenHUD : MonoBehaviour
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
        AddButtonListener();
    }

    private void Start()
    {
        UpdateLevelText();
    }
    void OnDisable()
    {
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

    private void UpdateLevelText()
    {
        if (LevelManager.Instance != null)
        {
            _levelText.text = "Level " + (LevelManager.Instance.CurrentLevelIndex + 1);
        }
    }
    private void AddButtonListener()
    {
        _volumeToggleButton.onClick.AddListener(() =>
        {

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
}
