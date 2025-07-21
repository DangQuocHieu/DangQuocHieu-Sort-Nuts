using System.Collections.Generic;
using UnityEngine;

public class BoltObject : MonoBehaviour
{
    [SerializeField] private List<NutColor> _nutColors = new List<NutColor>();
    [SerializeField] private List<NutObject> _nutObjects = new List<NutObject>();
    [SerializeField] private float _initialNutPositionY = 0.8f;
    [SerializeField] private float _yOffset = 0.2f;
    [SerializeField] private Transform _nutObjectContainer;

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
}
