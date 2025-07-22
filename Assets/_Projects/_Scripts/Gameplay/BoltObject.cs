using System.Collections.Generic;
using UnityEngine;

public class BoltObject : MonoBehaviour
{
    [SerializeField] private List<NutColor> _initialNutColor = new List<NutColor>();
    [SerializeField] private int _maxNutCount = 4;
    public int MaxNutCount => _maxNutCount;
    [SerializeField] private List<NutObject> _nutObjects = new List<NutObject>();
    public List<NutObject> NutObjects => _nutObjects;
    [SerializeField] private float _initialNutPositionY = 0.8f;
    [SerializeField] private float _yOffset = 0.2f;
    [SerializeField] private Transform _nutObjectContainer;
    public Transform NutObjectContainer => _nutObjectContainer;
    [SerializeField] private Transform _nutSelectedPoint;

    private NutObject _nutOnTop { get { return _nutObjects[_nutObjects.Count - 1]; } }
    public NutObject NutOnTop => _nutOnTop;

    private bool _isCompleted = false;

    void Start()
    {
        SetUpNutObject();
    }

    private void SetUpNutObject()
    {
        for (int i = 0; i < Mathf.Min(_initialNutColor.Count, _maxNutCount); i++)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, _initialNutPositionY + i * _yOffset, transform.position.z);
            NutObject nutObject = NutObjectSpawner.Instance.GetNutObject(spawnPosition, _nutObjectContainer);
            nutObject.SetNutColor(_initialNutColor[i]);
            _nutObjects.Add(nutObject);
        }
    }

    public Vector3 CalculateTopPosition()
    {
        return new Vector3(transform.position.x, _initialNutPositionY + (_nutObjects.Count - 1) * _yOffset, transform.position.z);
    }

    public void SelectNutOnTop()
    {
        _nutOnTop.transform.position = _nutSelectedPoint.position;
    }

    public bool HasEnoughNuts()
    {
        return _nutObjects.Count == _maxNutCount;
    }

    public bool HasNoNuts()
    {
        return _nutObjects.Count == 0;
    }

    public void Undo()
    {
        _nutOnTop.transform.position = CalculateTopPosition();
        SortManager.Instance.ResetCurrentObject();
    }

    //Move nutToSort from this Bolt to another Bolt
    public void OnNutSorted(BoltObject another)
    {
        if (this == another)
        {
            Undo(); return;
        }
        if (another.HasEnoughNuts() || (!another.HasNoNuts() && _nutOnTop.NutColor != another.NutOnTop.NutColor))
        {
            Undo();
            SortManager.Instance.SelectNewNut(another);
            return;
        }

        //Add new nut to another bolt
        another.NutObjects.Add(_nutOnTop);
        _nutOnTop.transform.parent = another.NutObjectContainer;
        _nutOnTop.transform.position = another.CalculateTopPosition();

        //Remove selected nut
        _nutObjects.Remove(_nutOnTop);
        SortManager.Instance.ResetCurrentObject();
        another.CheckCompleted();

    }

    public void CheckCompleted()
    {
        for (int i = 1; i < _nutObjects.Count; i++)
        {
            if (_nutObjects[i].NutColor != _nutObjects[i - 1].NutColor) return;
        }
        _isCompleted = _nutObjects.Count == _maxNutCount;
        if (_isCompleted)
        {
            GetComponent<BoxCollider>().enabled = false;
            SortManager.Instance.OnBoltCompleted(this);
        }
    }

}
