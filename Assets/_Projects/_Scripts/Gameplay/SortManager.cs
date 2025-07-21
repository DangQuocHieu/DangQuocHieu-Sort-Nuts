using UnityEngine;

public class SortManager : Singleton<SortManager>
{
    [SerializeField] private BoltObject[] _allBoltObjects;
    [SerializeField] private NutObject _currentNutObject;
    [SerializeField] private BoltObject _currentBoltObject;

    protected override void Awake()
    {
        base.Awake();
        _allBoltObjects = GetComponentsInChildren<BoltObject>();
    }

    public void OnBoltSelected(BoltObject currentBolt, NutObject currentNut)
    {
        if (_currentBoltObject != null)
        {
            Debug.Log("CURRENT BOLT IS NOT NULL");
            if (_currentBoltObject == currentBolt)
            {
                Debug.Log("UNDO");
                currentBolt.Undo();
                ResetCurrentObject();
            }
        }
        else
        {
            _currentBoltObject = currentBolt;
            _currentNutObject = currentNut;
        }
    }

    public void ResetCurrentObject()
    {
        _currentBoltObject = null;
        _currentNutObject = null;
    }
}
