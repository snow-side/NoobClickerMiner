using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using YGTemplate.Utils;

namespace YGTemplate.InGameTimeReward
{
    public class InGameTimeRewManager : Singleton<InGameTimeRewManager>
    {
        [SerializeField] private InGameTimeRewData[] listOfInGameTimeRewData;
        [SerializeField] private bool spawnWindowForInGameTimeRew;
        [SerializeField] private InGameTimeWindowUI prefab_InGameTimeRewWindowUI;
        [SerializeField] private bool spawnWindowIcons;
        [SerializeField] private InGameTimeWindowIconUI prefab_InGameTimeRewWindowIconUI;

        public InGameTimeWindowUI inGameTimeWindowUI;

        public float crntInGameTime { get; private set; }

        [Space(15)]
        public UnityEvent<float> OnGameTimeChanged;
        public UnityEvent<bool> OnAvailableReward;
        public UnityEvent<string> OnReward;

        //    public event Action<bool> OnHavingNotification;

        private float deltaTimeMultiplier = 1.0f;

        private Dictionary<string, InGameTimeRew> dictOfInGameTimeRew;

#if UNITY_EDITOR
        [SerializeField] private List<InGameTimeRew> listOfInGameTimeRew;
#endif

        public override void Awake()
        {
            base.Awake();
            crntInGameTime = 0f;
            SpawnInGameTimeRew();
        }

        public void Start()
        {

        }

        public void LateUpdate()
        {
            crntInGameTime += Time.deltaTime * deltaTimeMultiplier;
            OnGameTimeChanged?.Invoke(crntInGameTime);
        }

        public void InitUI()
        {
            if (!spawnWindowForInGameTimeRew)
            {
                SpawnOrLinkIcons();
            }
        }


        #region rewards
        public void GetReward(string data)
        {
            OnReward?.Invoke(data);

            OnAvailableReward?.Invoke(IsHaveAvailableRewards());
        }

        public void AvailableReward(string InGameTimeRewID)
        {
            OnAvailableReward?.Invoke(true);

            Debug.Log("[" + this.GetType().Name + "] " + "InGameTimeRew become available with id" + InGameTimeRewID);
        }

        public bool IsHaveAvailableRewards()
        {
            if (dictOfInGameTimeRew == null) return false;

            foreach (KeyValuePair<string, InGameTimeRew> kvp in dictOfInGameTimeRew)
            {
                if (kvp.Value.IsAvailable())
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region spawn
        public void SpawnInGameTimeRew()
        {
#if UNITY_EDITOR
            listOfInGameTimeRew = new List<InGameTimeRew>();
#endif
            dictOfInGameTimeRew = new Dictionary<string, InGameTimeRew>();

            for (int i = 0; i < listOfInGameTimeRewData.Length; i++)
            {
                InGameTimeRew newIGTR = new InGameTimeRew(listOfInGameTimeRewData[i]);

                dictOfInGameTimeRew.Add(newIGTR.inGameTimeRewID, newIGTR);
#if UNITY_EDITOR
                listOfInGameTimeRew.Add(newIGTR);
#endif
            }
        }

        public void SpawnWindow(Transform parent)
        {
            if (!spawnWindowForInGameTimeRew) return;

            inGameTimeWindowUI = Instantiate(prefab_InGameTimeRewWindowUI, parent);
            SpawnOrLinkIcons();
        }

        public void SpawnOrLinkIcons()
        {
            if (spawnWindowIcons)
            {
                inGameTimeWindowUI.SpawnIcons(prefab_InGameTimeRewWindowIconUI, listOfInGameTimeRewData);
            }
            else
            {
                inGameTimeWindowUI.LinkIconsToData();
            }
        }

        #endregion

        #region dev_mode_related
        public void SetDeltaTimeMultiplier(float value)
        {
            if (!TemplateManager.DEV_MODE) return;

            deltaTimeMultiplier = value;
        }

        #endregion

        #region dict_manipulation
        public InGameTimeRew GetInGameTimeRew(string inGameTimeRewID)
        {
            if (dictOfInGameTimeRew == null)
            {
                Debug.Log("[" + this.GetType().Name + "] " + "Try get access when there are no dictionary  " + inGameTimeRewID);
                return null;
            }
            if (!dictOfInGameTimeRew.ContainsKey(inGameTimeRewID))
            {
                Debug.Log("[" + this.GetType().Name + "] " + "Try get InGameTimeRew with nonexists id " + inGameTimeRewID);
                return null;
            }

            return dictOfInGameTimeRew[inGameTimeRewID];
        }
        #endregion

        public void ShowWindow()
        {
            inGameTimeWindowUI.SetActive(true);
        }
    }
}