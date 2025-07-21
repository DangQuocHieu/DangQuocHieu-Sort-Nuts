using UnityEngine;

public class NutObjectSpawner : Singleton<NutObjectSpawner>
{
    [SerializeField] private NutObject _nutObjectPrefab;
    public NutObject GetNutObject(Vector3 position, Transform container)
    {
        NutObject nutObject = Instantiate(_nutObjectPrefab, position, _nutObjectPrefab.transform.rotation, container);
        return nutObject;
    }

}
