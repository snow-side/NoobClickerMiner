using ToolBox.Pools;
using UnityEngine;

public class Miner : MonoBehaviour
{
    [SerializeField]
    GameObject SparkFx;

    [SerializeField]
    Transform SparkPos;

    public void Mine()
    {
        SparkFx.Reuse(SparkPos.position, Quaternion.identity);
    }
}
