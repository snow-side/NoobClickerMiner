using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonCameraController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Camera Camera;

    [SerializeField]
    float Max;

    [SerializeField]
    float YOffset = 5f;

    bool Pressed;

    void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if ((Max >= 0 && Camera.transform.position.y >= Max) || (Max < 0 && Camera.transform.position.y <= Max))
            Pressed = false;

        if (Pressed)
            Camera.transform.Translate(new Vector3(0, YOffset, 0) * Time.deltaTime);
    }

    public void OnPointerDown(PointerEventData eventData) => Pressed = true;

    public void OnPointerUp(PointerEventData eventData) => Pressed = false;
}
