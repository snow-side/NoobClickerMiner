using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "SO/ResourceData", order = 1)]
public class ResourceData : ScriptableObject
{
    public int Level;
    public Material Material;
    public string Name;
}