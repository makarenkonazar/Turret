using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _maxObjects = 100;
    [SerializeField] private float _areaSize = 100f;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < _maxObjects; i++)
        {
            SpawnObject();
        }
    }

    void Update()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt(i);
                SpawnObject();
            }
        }
    }

    void SpawnObject()
    {
        float x = Random.Range(-_areaSize / 2, _areaSize / 2);
        float y = Random.Range(-_areaSize / 2, _areaSize / 2);

        Vector3 localPos = new Vector3(x, y, 0);
        Vector3 worldPos = transform.position + localPos;

        GameObject newObj = Instantiate(_prefab, worldPos, Quaternion.identity);
        spawnedObjects.Add(newObj);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(_areaSize, _areaSize, 0.1f));
    }
}
