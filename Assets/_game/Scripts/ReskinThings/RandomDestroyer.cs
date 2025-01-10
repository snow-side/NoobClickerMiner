using UnityEngine;

public class RandomDestroyer : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float destroyChance;
    public void Awake()
    {
        DestroyItself(UnityEngine.Random.Range(0.0f, 1.0f));
    }

    public void DestroyItself(float chance = 1f)
    {
        if (chance <= destroyChance)
        {
            Destroy(gameObject);
            return;
        }
    }
}
