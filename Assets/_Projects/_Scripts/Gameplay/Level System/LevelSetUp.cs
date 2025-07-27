using System.Collections;
using UnityEngine;

public class LevelSetUp : MonoBehaviour
{
    [SerializeField] private SortManager _sortManager;
    [SerializeField] private TutorialManager _tutorialManager;
    void Start()
    {
        StartCoroutine(SetUpLevelRoutine());
    }

    private IEnumerator SetUpLevelRoutine()
    {
        yield return new WaitUntil(() => DataManager.Instance.IsLoaded);
        LevelManager.Instance.SetUpLevel(_sortManager.transform);
        _sortManager.Init();
    }
        
}
