using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TutorialManager : MonoBehaviour, IMessageHandle
{
    [SerializeField] private RectTransform _handRect;
    [SerializeField] private TextMeshProUGUI _tutorialText;
    [SerializeField] private int _firstBoltIndex;
    [SerializeField] private int _secondBoltIndex;
    private BoltObject _firstBoltObject;
    private BoltObject _secondBoltObject;

    private bool _hasSelected = false;
    private bool _hasSorted = false;

    void OnEnable()
    {
        MessageManager.AddSubscriber(GameMessageType.OnNutSelected, this);
        MessageManager.AddSubscriber(GameMessageType.OnNutSorted, this);
        MessageManager.AddSubscriber(GameMessageType.OnLevelStart, this);
    }

    void OnDisable()
    {
        MessageManager.RemoveSubscriber(GameMessageType.OnNutSelected, this);
        MessageManager.RemoveSubscriber(GameMessageType.OnNutSorted, this);
        MessageManager.RemoveSubscriber(GameMessageType.OnLevelStart, this);
    }

    public void Init()
    {
        int currentLevelIndex = LevelManager.Instance.CurrentLevelIndex;
        if (currentLevelIndex != 0) Destroy(gameObject);
        else
        {
            List<BoltObject> boltObjects = SortManager.Instance.AllBoltObjects;
            _firstBoltObject = boltObjects[_firstBoltIndex];
            _secondBoltObject = boltObjects[_secondBoltIndex];
        }

    }

    public void Handle(Message message)
    {
        switch (message.type)
        {
            case GameMessageType.OnNutSelected:
                {
                    var boltObject = (BoltObject)message.data[0];
                    OnNutSelected(boltObject);
                    break;
                }
            case GameMessageType.OnNutSorted:
                {
                    OnNutSorted();
                    break;
                }
            case GameMessageType.OnLevelStart:
                var currentLevelIndex = (int)message.data[0];
                if (currentLevelIndex == 0)
                {
                    _handRect.gameObject.SetActive(true);
                    _tutorialText.gameObject.SetActive(true);
                }
                break;
        }
    }

    private void OnNutSelected(BoltObject boltObject)
    {
        if (_hasSelected) return;
        if (boltObject != _firstBoltObject) return;
        _hasSelected = true;
        Vector3 worldTargetPos = _secondBoltObject.transform.position;
        Vector3 screenTargetPos = Camera.main.WorldToScreenPoint(worldTargetPos);
        Vector3 handScreenPos = _handRect.position;
        Vector3 newScreenPos = new Vector3(screenTargetPos.x, handScreenPos.y, 0);
        _handRect.position = newScreenPos;
    }

    private void OnNutSorted()
    {
        if (_hasSorted) return;
        _hasSorted = true;
        _handRect.gameObject.SetActive(false);
    }
}
