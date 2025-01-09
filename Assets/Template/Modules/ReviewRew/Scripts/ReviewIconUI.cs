using UnityEngine;

namespace YGTemplate.Review
{
    public class ReviewIconUI : IconUI
    {

        public override void OnIconClicked()
        {
            ReviewManager.Instance.ShowWindow();
        }

        public override void Start()
        {
            base.Start();
            window = ReviewManager.Instance.reviewWindowUI;
            notificationMark.gameObject.SetActive(false);
            SetActive(ReviewManager.Instance.canShowAskForReview);
            ReviewManager.Instance.OnReviewAvailable.AddListener(SetActive);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            ReviewManager.Instance?.OnReviewAvailable.RemoveListener(SetActive);
        }
    }
}
