using UnityEngine;
using UnityEngine.UI;

public class SceneUIManager : MonoBehaviour
{
    [SerializeField] private Button settingsButton;

    [SerializeField] private Transform _containerForTemplate;
    public Transform containerForTemplate {
        get { return _containerForTemplate; }
    }

    public void Awake()
    {
        settingsButton.onClick.AddListener(() => {
            Debug.LogWarning("sett|_clicked");
            TemplateManager.Instance.ShowSettings();
        });
        
    }
}
