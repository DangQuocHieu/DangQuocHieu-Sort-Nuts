using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColorManager : Singleton<ColorManager>
{
    [SerializeField] private NutColor[] _nutColors;
    [SerializeField] private Color[] _colors;
    private Dictionary<NutColor, Color> _nutColorDictionary = new Dictionary<NutColor, Color>();


    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < _nutColors.Length; i++)
        {
            _nutColorDictionary.Add(_nutColors[i], _colors[i]);
        }
    }

    public Color GetNutColor(NutColor nutColor)
    {
        return _nutColorDictionary.ContainsKey(nutColor) ? _nutColorDictionary[nutColor] : Color.white;
    }
}
