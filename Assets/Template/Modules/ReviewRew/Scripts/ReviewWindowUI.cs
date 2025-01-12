using UnityEngine;
using UnityEngine.UI;
using YG;

namespace YGTemplate.Review
{
    public class ReviewWindowUI : WindowUI
    {
        [Header("Review WindowUI")]
        [SerializeField] private Button button_PositiveAnswer;
        [SerializeField] private Button button_NegativeAnswer;

        public override void Awake()
        {
            base.Awake();
            button_PositiveAnswer?.onClick.AddListener(() =>
            {
                YG2.ReviewShow();
#if UNITY_EDITOR

#endif
                PlayClickSound();
                SetActive(false);
            });

            button_NegativeAnswer?.onClick.AddListener(() =>
            {
                PlayClickSound();
                SetActive(false);
            });
        }
    }
}