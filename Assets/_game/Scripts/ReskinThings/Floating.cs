using UnityEngine;

public class Floating : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float crntFloatTime;
    [SerializeField] private float floatTimeLoop;
    [SerializeField] private Vector3 initLoc;

    public void Awake()
    {
        initLoc = transform.localPosition;
    }
    public void Update()
    {
        Float();
    }

    public void Float() {
        crntFloatTime = (crntFloatTime + Time.deltaTime) %floatTimeLoop;
        float newY = curve.Evaluate(crntFloatTime/ floatTimeLoop);
        transform.localPosition = new Vector3(initLoc.x, initLoc.y + newY, initLoc.z);
    }
}
