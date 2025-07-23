using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelCompleteScreenHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelCompleteText;
    [SerializeField] private float _scaleDuration;
    [SerializeField] private Ease _ease;

    void OnEnable()
    {
        _levelCompleteText.transform.localScale = Vector3.zero;
        _levelCompleteText.transform.DOScale(1, _scaleDuration).SetEase(_ease);
    }

}
