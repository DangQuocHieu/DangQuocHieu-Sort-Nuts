using UnityEngine;

public class NutObject : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    private NutColor _nutColor;
    public NutColor NutColor => _nutColor;

    private bool _isHidden;
    public bool IsHidden => _isHidden;
    public void Initialize(NutColor nutColor, bool isHidden)
    {
        _nutColor = nutColor;
        _isHidden = isHidden;
        if (_isHidden)
        {
            _meshRenderer.material = ColorManager.Instance.HiddenMaterial;
        }
        else
        {
            _meshRenderer.material.color = ColorManager.Instance.GetNutColor(nutColor);
        }
    }

    public void ShowHiddenColor()
    {
        if (!_isHidden) return;
        _isHidden = false;
        _meshRenderer.material = default;
        _meshRenderer.material.color = ColorManager.Instance.GetNutColor(_nutColor);
    }


}
