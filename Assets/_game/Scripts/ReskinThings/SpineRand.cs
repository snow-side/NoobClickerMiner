using UnityEngine;
using UnityEngine.Splines;

public class SpineRand : MonoBehaviour
{
    private SplineAnimate splineAnim;

    public void Awake()
    {
        splineAnim= GetComponent<SplineAnimate>();
        splineAnim.StartOffset = Random.Range(0.0f, 1.0f);
        splineAnim.Play();
    }
}
