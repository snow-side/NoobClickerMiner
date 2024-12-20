using System.Runtime.InteropServices;
using UnityEngine;

public class JsPlugin : MonoBehaviour
{
    [SerializeField]
    GameObject UI_Joysticks;

    [DllImport("__Internal")]
    private static extern bool IsMobile();

    public static bool Mobile { get; private set; }

    static void CheckMobile()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        Mobile = IsMobile();
#else
        Mobile = UnityEngine.Device.SystemInfo.deviceType != DeviceType.Desktop;
#endif
        Debug.Log($"IsMobile {Mobile}");
    }

    void Awake()
    {
        CheckMobile();
    }
}
