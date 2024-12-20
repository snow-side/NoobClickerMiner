using UnityEngine;
using UnityEngine.Events;

public class MineBlock : MonoBehaviour
{
    Camera Camera;

    [HideInInspector]
    public UnityEvent<Vector3> OnClick = new();

    void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.ScreenPointToRay(Input.mousePosition);
            var ok = Physics.Raycast(ray, out RaycastHit hit, 50);
            if (ok && hit.collider.gameObject == gameObject)
                OnClick.Invoke(hit.point + new Vector3(0.5f, 0, -2));
        }
    }
}
