using UnityEngine;
using UnityEngine.Events;
using YG;
using YGTemplate.Utils;

namespace YGTemplate.Review
{
    public class ReviewManager : Singleton<ReviewManager>
    {
        [SerializeField] private bool spawnWindowForReview;
        [SerializeField] private ReviewWindowUI prefab_ReviewWindowUI;

        public ReviewWindowUI reviewWindowUI;

        [SerializeField] private float timeBetweenReviewAsk;
        [SerializeField] private float crntTimeReviewAsk;

        [Space(15)]
        public UnityEvent<bool> OnReviewAvailable;
        public UnityEvent<bool> OnReviewSent;

        public bool canShowAskForReview { get; private set; }

        private float deltaTimeMultiplier = 1.0f;

        public override void Awake()
        {
            base.Awake();
            canShowAskForReview = true;
            crntTimeReviewAsk = 0;
        }

        public void Start()
        {
            UpdateData();
            YG2.onGetSDKData += UpdateData;
            YG2.onReviewSent += ReviewSent;
        }

        public void LateUpdate()
        {
            UpdateShowReviewWindow();
        }

        public void UpdateShowReviewWindow()
        {
            if (!canShowAskForReview) return;

            crntTimeReviewAsk += Time.deltaTime * deltaTimeMultiplier;
            if (crntTimeReviewAsk >= timeBetweenReviewAsk)
            {
                crntTimeReviewAsk = 0;
                ShowReviewWindow();
            }
        }

        public void ReviewSent(bool value)
        {

            OnReviewSent?.Invoke(true);

            UpdateData();
        }

        public void UpdateData()
        {
            canShowAskForReview = YG2.reviewCanShow;
            OnReviewAvailable?.Invoke(canShowAskForReview);
        }

        public void ShowReviewWindow()
        {
            reviewWindowUI?.SetActive(true);
        }

        public void OnDestroy()
        {
            YG2.onGetSDKData -= UpdateData;
            YG2.onReviewSent -= ReviewSent;
        }

        #region Spawn
        public void SpawnWindow(Transform parent)
        {
            if (!spawnWindowForReview) return;

            reviewWindowUI = Instantiate(prefab_ReviewWindowUI, parent);
        }
        #endregion

        #region dev_mode_related
        public void SetDeltaTimeMultiplier(float value)
        {
            if (!TemplateManager.DEV_MODE) return;

            deltaTimeMultiplier = value;
        }
        #endregion

        public void ShowWindow()
        {
            reviewWindowUI.SetActive(true);
        }
    }

}