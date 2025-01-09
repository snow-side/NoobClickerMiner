using UnityEngine;
using UnityEngine.UI;

public class MenuDialogUI : DialogBaseUI
{
    [SerializeField]
    Button BtnSfx;

    void Start() => BtnSfx.onClick.AddListener(() => GameAudio.Instance.OnOffSfx());
}
