using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SortManager : Singleton<SortManager>
{
    [SerializeField] private List<BoltObject> _allBoltObjects = new List<BoltObject>();
    [SerializeField] private NutObject _currentNutObject;
    [SerializeField] private BoltObject _currentBoltObject;

    protected override void Awake()
    {
        base.Awake();
        _allBoltObjects = GetComponentsInChildren<BoltObject>().ToList();
    }

    public void OnBoltSelected(BoltObject boltSelected)
    {
        if (_currentNutObject == null)
        {
            SelectNewNut(boltSelected);
        }
        else
        {
            _currentBoltObject.OnNutSorted(boltSelected);
        }
    }

    public void SelectNewNut(BoltObject boltSelected)
    {
        if (boltSelected.NutObjects.Count == 0) return;
        _currentBoltObject = boltSelected;
        _currentNutObject = _currentBoltObject.NutOnTop;
        _currentBoltObject.SelectNutOnTop();
        Debug.Log("SELECT NUT ON TOP");
    }

    public void ResetCurrentObject()
    {
        _currentBoltObject = null;
        _currentNutObject = null;
    }

    public void OnBoltCompleted(BoltObject completedBolt)
    {
        _allBoltObjects.Remove(completedBolt);
        for (int i = 0; i < _allBoltObjects.Count; i++)
        {
            if (_allBoltObjects[i].NutObjects.Count != 0) return;
        }
        MessageManager.SendMessage(new Message(GameMessageType.OnLevelCompleted));
        Debug.Log("LEVEL COMPLETE");
    }


}
