
using UnityEngine;
using UnityEngine.UI;

public class BtnBaseUI : MonoBehaviour
{
    void Awake()
    {
        var btn = GetComponent<Button>();
        btn.onClick ??= new();
        btn.onClick.AddListener(() => OnBtnClick());
    }

    public virtual void OnBtnClick() => GameAudio.Instance.PlaySfx("click");

}
