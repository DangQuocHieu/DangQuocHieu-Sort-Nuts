using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "RotateConfig", menuName = "Scriptable Objects/RotateConfig")]
public class RotateConfig : ScriptableObject
{
    [SerializeField] private float _rotateDuration = 0.2f;
    [SerializeField] private Vector3 _rotationAngle;

    public void Execute(Transform target)
    {
        target.DORotate(_rotationAngle, _rotateDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear);
    }
}
