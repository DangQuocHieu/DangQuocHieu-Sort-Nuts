using System.Collections;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveConfig", menuName = "Scriptable Objects/AnimationConfig/MoveConfig")]
public class MoveConfig : ScriptableObject
{
    [SerializeField] private float _moveDuration;
    [SerializeField] private Ease _ease;

    public void Execute(Transform target, Vector3 position, TweenCallback onComplete = null)
    {
        target.DOMove(position, _moveDuration).SetEase(_ease).OnComplete(onComplete);
    }

    public IEnumerator ExecuteCoroutine(Transform target, Vector3 position)
    {
        yield return target.DOMove(position, _moveDuration).SetEase(_ease).WaitForCompletion();
    }
}
