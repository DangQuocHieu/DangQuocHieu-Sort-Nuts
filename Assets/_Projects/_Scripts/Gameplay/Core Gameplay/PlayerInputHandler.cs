using UnityEngine;

public class PlayerInputHandler : Singleton<PlayerInputHandler>
{
    private bool _inputEnabled = true;

    void Update()
    {
        if (!_inputEnabled) return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.TryGetComponent<BoltObject>(out var boltObject))
                {
                    SortManager.Instance.OnBoltSelected(boltObject);
                }
            }
        }
    }

    public void DisableInput()
    {
        _inputEnabled = false;
    }

    public void EnableInput()
    {
        _inputEnabled = true;
    }
}
