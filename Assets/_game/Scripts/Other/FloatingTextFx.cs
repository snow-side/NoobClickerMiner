using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingTextFx : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI Text;

    [SerializeField]
    UnityEngine.UI.Image Image;

    [SerializeField]
    Vector3 Dir;

    [SerializeField]
    Vector3 Offset;

    [SerializeField]
    float Duration = 2;

    Tween Tween;

    public void Spawn(Vector3 pos, string text, Color color, float fontSize = 0.2f,
     Sprite sprite = null, float? duration = null)
    {
        gameObject.SetActive(true);
        Image.gameObject.SetActive(sprite != null);
        Image.sprite = sprite;
        Text.color = color;
        Text.text = text;
        Text.fontSize = fontSize;
        var _p = Vector3.zero;
        Utils.RandomVector(ref _p, Offset * -1, Offset);
        gameObject.transform.position = _p + pos;
        Tween = gameObject.transform.DOMove(Dir, duration ?? Duration).SetRelative(true).SetAutoKill(true).Play();
    }

    void OnDisable() => Tween.Kill();
}
