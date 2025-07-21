using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.TryGetComponent<BoltObject>(out var boltObject))
                {
                    boltObject.OnBoltSelected();
                }
            }
        }
    }
}
