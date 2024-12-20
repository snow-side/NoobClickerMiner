using UnityEngine;
using UnityEngine.UI;

public class CanvasAdapder : MonoBehaviour
{
    [Range(0.5f, 1)]
    [SerializeField]
    float MobileReScale;

    void Start()
    {
        if (JsPlugin.Mobile)
        {
            var cs = GetComponent<Canvas>().GetComponent<CanvasScaler>();
            cs.referenceResolution *= MobileReScale;
        }
    }
}
