
using UnityEngine;

public class DialogBaseUI : MonoBehaviour
{
    [SerializeField]
    protected GameObject Root;

    [SerializeField]
    protected Transform[] NeedHideShowDialogShow;

    public virtual void UpdateData(object data = null)
    {

    }

    public virtual void Show()
    {
        if (Root.activeSelf)
            Hide();
        else
        {
            Root.SetActive(true);
            ToogleAnotherUI(false);
        }
    }

    public virtual void Hide()
    {
        Root.SetActive(false);
        ToogleAnotherUI(true);
    }

    private void ToogleAnotherUI(bool val)
    {
        if (NeedHideShowDialogShow != null)
        {
            foreach (var item in NeedHideShowDialogShow)
                item.gameObject.SetActive(val);
        }
    }
}
