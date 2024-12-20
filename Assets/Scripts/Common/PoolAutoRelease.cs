using System.Collections;
using ToolBox.Pools;
using UnityEngine;

public class PoolAutoRelease : MonoBehaviour
{
    [SerializeField]
    float Ttl = 5;

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(ReleaseDelay());
    }

    void OnDisable() => StopAllCoroutines();

    IEnumerator ReleaseDelay()
    {
        yield return new WaitForSeconds(Ttl);
        gameObject.Release();
    }
}
