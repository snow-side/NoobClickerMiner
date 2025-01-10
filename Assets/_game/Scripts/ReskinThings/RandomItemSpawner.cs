using System.Collections.Generic;
using UnityEngine;

public class RandomItemSpawner : MonoBehaviour
{
    [SerializeField] private bool spawnOnAwake;
    [Range(0, 1)]
    [SerializeField] private float spawnChance;
    [SerializeField] private List<Sprite> itemsPrefabs;
    [SerializeField] private Vector3 rotOffset = Vector3.zero;

    SpriteRenderer rend;

    public void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        if (spawnOnAwake) SpawnSomething(spawnChance);
    }

    public void SpawnSomething(float spawnChance = 1f)
    {
        if (itemsPrefabs == null) {
            Destroy(gameObject);
            return;
        };
        float randChance = UnityEngine.Random.Range(0f, 1f);
        if (randChance >= spawnChance) {
            Destroy(gameObject);
            return;
        };
        int randInd = UnityEngine.Random.Range(0, itemsPrefabs.Count);
        rend.sprite = itemsPrefabs[randInd];

    }
}
