using System.Collections.Generic;
using UnityEngine;

public class SetRandomSkin : MonoBehaviour
{
    [SerializeField] private List<Material> listOfSkins;
    [SerializeField] private List<Material> listOfSkinsPickaxe;
    [SerializeField] private GameObject go_Pickaxe;

    public void Awake()
    {
        if (listOfSkins != null && listOfSkins.Count > 0) SetSkin(
            UnityEngine.Random.Range(0, listOfSkins.Count)
            );

        if (listOfSkins != null && listOfSkins.Count > 0) SetSkinPickaxe(
            UnityEngine.Random.Range(0, listOfSkinsPickaxe.Count)
            );
    }

    public void SetSkin(int ind)
    {
        if (listOfSkins[ind] == null) return;
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = listOfSkins[ind];
        }
    }

    public void SetSkinPickaxe(int ind)
    {
        if (go_Pickaxe == null) return;
        MeshRenderer pickMeshRenderer = go_Pickaxe.GetComponent<MeshRenderer>();
        if (pickMeshRenderer == null) return;
        pickMeshRenderer.material = listOfSkinsPickaxe[ind];
    }
}
