using UnityEngine;
using UnityEngine.UI;

public class SceneUIManager : MonoBehaviour
{
    [SerializeField] private Button settingsButton;

    [SerializeField] private Transform _containerForTemplate;
    [SerializeField] private Button button_EduNext01;
    [SerializeField] private Button button_EduNext02;
    [SerializeField] private Button button_EduNext03;
    [SerializeField] private Button button_EduNext04;

    [SerializeField] private GameObject edu01;
    [SerializeField] private GameObject edu02;
    [SerializeField] private GameObject edu03;
    [SerializeField] private GameObject edu04;

    public Transform containerForTemplate {
        get { return _containerForTemplate; }
    }

    public void Awake()
    {
        settingsButton.onClick.AddListener(() => {
            Debug.LogWarning("sett|_clicked");
            TemplateManager.Instance.ShowSettings();
        });
        button_EduNext01.onClick.AddListener(() =>
        {
            edu01.SetActive(false);
            edu02.SetActive(true);
        });
        button_EduNext02.onClick.AddListener(() =>
        {
            edu02.SetActive(false);
            edu03.SetActive(true);
        });
        button_EduNext03.onClick.AddListener(() =>
        {
            edu03.SetActive(false);
            edu04.SetActive(true);
        });
        button_EduNext04.onClick.AddListener(() =>
        {
            edu04.SetActive(false);
            EndEdu();
        });
        edu01.SetActive(false);
        edu02.SetActive(false);
        edu03.SetActive(false);
        edu04.SetActive(false);
    }

    public void ShowEdu() {
        Debug.LogError("ShowEdu");
        edu01.SetActive(true);
    }

    public void EndEdu() {
        Game.Instance.EduShown();
    }
}
