using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltObject : MonoBehaviour
{
    [SerializeField] private List<NutObjectData> _initialNutObjectData = new List<NutObjectData>();
    [SerializeField] private int _maxNutCount = 4;
    public int MaxNutCount => _maxNutCount;
    [SerializeField] private Stack<NutObject> _nutObjects = new Stack<NutObject>();
    public Stack<NutObject> NutObjects => _nutObjects;

    [SerializeField] private float _initialNutPositionY = 0.8f;
    [SerializeField] private float _yOffset = 0.2f;
    [SerializeField] private Transform _nutObjectContainer;
    public Transform NutObjectContainer => _nutObjectContainer;
    [SerializeField] private Transform _nutSelectedPoint;
    public Transform NutSelectedPoint => _nutSelectedPoint;

    private NutObject _nutOnTop { get { return _nutObjects.Peek(); } }
    public NutObject NutOnTop => _nutOnTop;

    private bool _isCompleted = false;

    [Header("Animation Config")]
    [SerializeField] private MoveConfig _moveConfig;
    [SerializeField] private RotateConfig _rotateConfig;

    [Header("Complete Particle")]
    [SerializeField] private ParticleSystem _completeParticleEffect;


    void Start()
    {
        SetUpNutObject();
    }

    private void SetUpNutObject()
    {
        float spawnPositionY = _initialNutPositionY - _yOffset * (_maxNutCount - _initialNutObjectData.Count);
        for (int i = 0; i < Mathf.Min(_initialNutObjectData.Count, _maxNutCount); i++)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, spawnPositionY - i * _yOffset, transform.position.z);
            NutObject nutObject = NutObjectSpawner.Instance.GetNutObject(spawnPosition, _nutObjectContainer);
            nutObject.Initialize(_initialNutObjectData[i].NutColor, _initialNutObjectData[i].IsHidden);
        }

        NutObject[] _allNutObjects = _nutObjectContainer.GetComponentsInChildren<NutObject>();
        for (int i = _allNutObjects.Length - 1; i >= 0; i--)
        {
            _nutObjects.Push(_allNutObjects[i]);
        }
    }


    public Vector3 CalculateTopPosition()
    {
        return new Vector3(transform.position.x, _initialNutPositionY - (_maxNutCount - _nutObjects.Count) * _yOffset, transform.position.z);
    }

    public void SelectNutOnTop()
    {

        _moveConfig.Execute(_nutOnTop.transform, _nutSelectedPoint.position);
        _rotateConfig.Execute(_nutOnTop.transform);
        // _nutOnTop.transform.position = _nutSelectedPoint.position;
        MessageManager.SendMessage(new Message(GameMessageType.OnNutSelected, new object[] { this }));
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
        _moveConfig.Execute(_nutOnTop.transform, CalculateTopPosition());
        _rotateConfig.Execute(_nutOnTop.transform);
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
        StartCoroutine(SortNutCoroutine(another));

    }
    private IEnumerator SortNutCoroutine(BoltObject another)
    {
        if (another.HasNoNuts())
        {
            NutColor current = _nutOnTop.NutColor;
            while (_nutObjects.Count > 0 && _nutOnTop.NutColor == current)
            {
                yield return MoveNutToAnotherBolt(another);
                if (_nutObjects.Count > 0 && _nutOnTop.IsHidden)
                {
                    ShowHiddenColor(); break;
                }
            }
        }
        else
        {
            NutColor current = another.NutOnTop.NutColor;
            while (_nutObjects.Count > 0 && _nutOnTop.NutColor == current && !another.HasEnoughNuts())
            {
                yield return MoveNutToAnotherBolt(another);
                if (_nutObjects.Count > 0 && _nutOnTop.IsHidden)
                {
                    ShowHiddenColor(); break;
                }
            }
        }
    }

    private IEnumerator MoveNutToAnotherBolt(BoltObject another)
    {
        another.NutObjects.Push(_nutOnTop);
        PlayerInputHandler.Instance.DisableInput();
        _nutOnTop.transform.parent = another.NutObjectContainer;
        if (_nutOnTop.transform.position != _nutSelectedPoint.position)
        {
            yield return _moveConfig.ExecuteCoroutine(_nutOnTop.transform, _nutSelectedPoint.position);
        }
        MessageManager.SendMessage(new Message(GameMessageType.OnNutMoved));
        yield return _moveConfig.ExecuteCoroutine(_nutOnTop.transform, another.NutSelectedPoint.position);
        _rotateConfig.Execute(_nutOnTop.transform);
        yield return _moveConfig.ExecuteCoroutine(_nutOnTop.transform, another.CalculateTopPosition());

        //Remove selected nut
        _nutObjects.Pop();
        SortManager.Instance.ResetCurrentObject();
        another.CheckCompleted();

        PlayerInputHandler.Instance.EnableInput();
        MessageManager.SendMessage(new Message(GameMessageType.OnNutSorted));
    }

    public void CheckCompleted()
    {
        if (_isCompleted) return;
        if (HasNoNuts()) return;
        NutColor color = _nutOnTop.NutColor;
        foreach (var nutObject in _nutObjects)
        {
            if (nutObject.NutColor != color) return;
        }
        _isCompleted = _nutObjects.Count == _maxNutCount;
        if (_isCompleted)
        {
            _completeParticleEffect.Play();
            GetComponent<BoxCollider>().enabled = false;
            SortManager.Instance.OnBoltCompleted(this);
            MessageManager.SendMessage(new Message(GameMessageType.OnBoltCompleted));
        }
    }

    private void ShowHiddenColor()
    {
        if (HasNoNuts()) return;

        _nutOnTop.ShowHiddenColor();
    }

}
