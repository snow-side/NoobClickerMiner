using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private bool spawnOnAwake;
    [Range(0,1)]
    [SerializeField] private float spawnChance;
    [SerializeField] private List<GameObject> itemsPrefabs;
    [SerializeField] private Vector3 rotOffset = Vector3.zero;

    public void Awake()
    {
        if (spawnOnAwake) SpawnSomething(spawnChance);
    }

    public GameObject SpawnSomething(float spawnChance = 1f,Transform parent=null)
    {
        if (itemsPrefabs == null)
        {
            Destroy(gameObject);
            return null;
        }
        float randChance = UnityEngine.Random.Range(0f, 1f);
        if (randChance >= spawnChance)
        {
            Destroy(gameObject);
            return null;
        }
        int randInd = UnityEngine.Random.Range(0, itemsPrefabs.Count);
        Transform newParent = parent != null ? parent : this.transform;
        GameObject newGO = Instantiate(itemsPrefabs[randInd], newParent);
        newGO.transform.localPosition = Vector3.zero;
        newGO.transform.eulerAngles = rotOffset;
        return newGO;
    }
}
