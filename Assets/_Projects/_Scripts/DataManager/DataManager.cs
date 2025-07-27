using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public interface ISaveable
{
    public void SaveData(PlayerData data);
    public void LoadData(PlayerData data);
}
public class DataManager : PersistentSingleton<DataManager>
{

    [SerializeField] private PlayerData _playerData;
    public PlayerData PlayerData => _playerData;
    [SerializeField] private string _saveName;

    private List<ISaveable> _saveables = new List<ISaveable>();

    private bool _isLoaded = false;
    public bool IsLoaded => _isLoaded;

    void Start()
    {
        _saveables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveable>().ToList();
        LoadGame();
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + _saveName + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _playerData = JsonUtility.FromJson<PlayerData>(json);
        }
        if (_playerData == null) _playerData = new PlayerData();
        foreach (var saveable in _saveables)
        {
            saveable.LoadData(_playerData);
        }
        _isLoaded = true;
    }

    public void SaveGame()
    {

        foreach (var saveable in _saveables)
        {
            saveable.SaveData(_playerData);
        }
        string json = JsonUtility.ToJson(_playerData);
        File.WriteAllText(Application.persistentDataPath + _saveName + ".json", json);
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }
}


