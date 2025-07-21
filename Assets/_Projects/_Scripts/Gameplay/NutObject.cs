using UnityEngine;

public class NutObject : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    private NutColor _nutColor;
    public void SetNutColor(NutColor nutColor)
    {
        _nutColor = nutColor;
        _meshRenderer.material.color = ColorManager.Instance.GetNutColor(nutColor);
    }


}
