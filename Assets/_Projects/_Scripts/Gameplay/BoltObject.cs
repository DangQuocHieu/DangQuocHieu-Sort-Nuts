using System.Collections.Generic;
using UnityEngine;

public class BoltObject : MonoBehaviour
{
    [SerializeField] private List<NutColor> _nutColors = new List<NutColor>();
    [SerializeField] private List<NutObject> _nutObjects = new List<NutObject>();
    public List<NutObject> NutObjects => _nutObjects;
    [SerializeField] private float _initialNutPositionY = 0.8f;
    [SerializeField] private float _yOffset = 0.2f;
    [SerializeField] private Transform _nutObjectContainer;
    [SerializeField] private Transform _onSelectedPoint;

    private NutObject _nutOnTop { get { return _nutObjects[_nutObjects.Count - 1]; } }

    void Start()
    {
        SetUpNutObject();
    }

    private void SetUpNutObject()
    {
        for (int i = 0; i < _nutColors.Count; i++)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, _initialNutPositionY + i * _yOffset, 0f);
            NutObject nutObject = NutObjectSpawner.Instance.GetNutObject(spawnPosition, _nutObjectContainer);
            nutObject.SetNutColor(_nutColors[i]);
            _nutObjects.Add(nutObject);
        }
    }

    public void OnBoltSelected()
    {
        if (_nutObjects.Count == 0)
        {
            return;
        }
        _nutOnTop.transform.position = _onSelectedPoint.position;
        SortManager.Instance.OnBoltSelected(this, _nutOnTop);
    }

    public void Undo()
    {
        if (_nutObjects.Count == 0) return;
        _nutOnTop.transform.position = CalculateTopPosition();
    }

    public Vector3 CalculateTopPosition()
    {
        return new Vector3(transform.position.x, _initialNutPositionY + (_nutColors.Count - 1) * _yOffset, 0f);
    }
}
