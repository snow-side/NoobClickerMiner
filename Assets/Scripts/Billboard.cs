using UnityEngine;

public class Billboard : MonoBehaviour
{
    RectTransform RectTransform;
    Transform CamTransform;
    Quaternion OriginRotation;

    void Start()
    {
        RectTransform = GetComponent<RectTransform>();
        OriginRotation = RectTransform.rotation;
        CamTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        RectTransform.LookAt(CamTransform.transform);

        //RectTransform.rotation = CamTransform.rotation * OriginRotation;
    }
}
