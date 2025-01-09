using System;
using UnityEngine;

public static class Utils
{
    public static int[] RandomInts(int num, int sum)
    {
        var array = new int[num];
        var rSum = sum;

        if (sum < num)
            throw new ArgumentOutOfRangeException(nameof(sum));

        for (int i = 0; i < array.Length - 1; i++)
        {
            array[i] = UnityEngine.Random.Range(1, rSum - (array.Length - i - 1));
            rSum -= array[i];
        }

        array[^1] = rSum;
        return array;
    }

    public static T RandomElement<T>(this T[] items)
    {
        var ind = UnityEngine.Random.Range(1, items.Length);
        return items[ind];
    }

    public static void RandomVector(this ref Vector3 myVector, Vector3 min, Vector3 max)
     => myVector = new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));


    public static void RespawnFx(this GameObject go, Vector3 pos)
    {
        go.transform.position = pos;
        var ps = go.GetComponent<ParticleSystem>();
        ps.Simulate(0.0f, true, true);
        ps.Play();
    }
}