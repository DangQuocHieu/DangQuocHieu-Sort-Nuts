using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : Singleton<PlayerInputHandler>
{
    private bool _inputEnabled = true;

    void Update()
    {
        if (!_inputEnabled) return;
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Pointer.current.position.ReadValue());
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
