using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;
using UnityEditor;

namespace HitAndRun.Gui
{
    public class MbGiftScreen : MonoBehaviour
    {
        [Header("UI Elements")]
        public GameObject giftScreen;
        public GameObject Dimed;
        private bool shouldDoubleCoins = false;

        [Header("Animation Settings")]
        [SerializeField] private float settingTweenTime = 0.3f;
        [SerializeField] private float waitTimeBeforeShow = 0.1f;

        [Header("Collection")]
        [SerializeField] private GameObject coinPrefab;

        [SerializeField] private Transform coinParent;

        [SerializeField] private Transform spawnLocation;

        [SerializeField] private Transform endPosition;
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private float duration;
        [SerializeField] private int coinAmount;

        [SerializeField] private float minX;

        [SerializeField] private float maxX;

        [SerializeField] private float minY;

        [SerializeField] private float maxY;

        List<GameObject> coins = new List<GameObject>();

        private Tween coinReactionTween;

        private int coin;

        // [Header("UI Buttons")]
        // [SerializeField] private Button claimButton;
        // [SerializeField] private Button claimAdsButton;

        private float initialPositionX;

        void Start()
        {
            if (giftScreen != null)
            {
                ShowGift();
            }
        }

        protected virtual void Reset()
        {
            giftScreen = transform.Find("Reward/OfflineReward")?.gameObject;
            Dimed = transform.Find("Reward/Dimed")?.gameObject;

            coinPrefab = Resources.Load<GameObject>("Prefabs/Coin_2D");

            coinParent = transform.parent?.Find("Gift");

            spawnLocation = transform.Find("SpawnCoin");

            var ticketTransform = transform.root.Find("Tickets");
            endPosition = ticketTransform?.Find("Icon");
            _coinText = ticketTransform?.Find("Ticket_Count")?.GetComponent<TextMeshProUGUI>();
            duration = 1;
            coinAmount = 10;
            minX = -100f;
            maxX = 100f;
            minY = -200f;
            maxY = 200f;
            // claimButton = transform.Find("Reward/OfflineReward/Bg/Button_Claim")?.GetComponent<Button>();
            // claimAdsButton = transform.Find("Reward/OfflineReward/Bg/Button_x2Claim")?.GetComponent<Button>();

        }


        public void ShowGift()
        {
            StartCoroutine(TweenShowPanel(giftScreen));
        }

        public void HideGift()
        {
            StartCoroutine(TweenHidePanel(giftScreen));
        }

        public void Collecting()
        {
            AdmobAds.instance.CorrectDoupleCoin((shouldDouble) =>
            {
                shouldDoubleCoins = shouldDouble; 
                CollectCoins(); 
            });
        }

        private IEnumerator TweenShowPanel(GameObject targetPanel)
        {
            yield return new WaitForSeconds(waitTimeBeforeShow);

            targetPanel.SetActive(true);
            Dimed.SetActive(true);
            Dimed.GetComponent<CanvasGroup>().DOFade(1f, settingTweenTime).SetEase(Ease.OutElastic);
            targetPanel.transform.localScale = Vector3.zero;
            targetPanel.transform.DOScale(Vector3.one, settingTweenTime).SetEase(Ease.OutElastic);
        }

        private IEnumerator TweenHidePanel(GameObject targetPanel)
        {
            Dimed.GetComponent<CanvasGroup>().DOFade(0f, settingTweenTime).SetEase(Ease.InBack);

            targetPanel.transform.DOScale(Vector3.zero, settingTweenTime).SetEase(Ease.InBack)
                .OnComplete(() => targetPanel.SetActive(false));

            yield return new WaitForSeconds(settingTweenTime);

            Dimed.SetActive(false);
        }

        string FormatTickets(int tickets)
        {
            if (tickets >= 1000)
                return (tickets / 1000f).ToString("0.#") + "k";
            return tickets.ToString();
        }

        private async void CollectCoins()
        {
            SetCoin(0);
            for (int i = 0; i < coins.Count; i++)
            {
                Destroy(coins[i]);
            }
            coins.Clear();

            
            int effectiveCoinAmount = shouldDoubleCoins ? coinAmount * 2 : coinAmount;

            List<UniTask> spawnCoinTaskList = new List<UniTask>();
            for (int i = 0; i < effectiveCoinAmount; i++)
            {
                GameObject coinInstance = Instantiate(coinPrefab, coinParent);
                float xPosition = spawnLocation.position.x + Random.Range(minX, maxX);
                float yPosition = spawnLocation.position.y + Random.Range(minY, maxY);

                coinInstance.transform.position = new Vector3(xPosition, yPosition);

                spawnCoinTaskList.Add(coinInstance.transform.DOPunchPosition(new Vector3(0, 30, 0), Random.Range(0, 1f)).SetEase(Ease.InOutElastic)
                    .ToUniTask());
                coins.Add(coinInstance);
                await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
            }

            await UniTask.WhenAll(spawnCoinTaskList);
            await MoveCoinsTask();
            // Animation the reaction when collecting coin
            // HideGift();
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(MbGiftScreen))]
        public class ETeam1Inspector : Editor
        {
            public override void OnInspectorGUI()
            {
                MbGiftScreen collectingCoin = (MbGiftScreen)target;

                DrawDefaultInspector();

                if (GUILayout.Button("Collecting"))
                {
                    collectingCoin.CollectCoins();
                }
            }
        }
#endif

        private void SetCoin(int value)
        {
            coin = value;
            _coinText.text = coin.ToString();
        }
        private async UniTask MoveCoinsTask()
        {
            List<UniTask> moveCoinTask = new List<UniTask>();
            for (int i = coins.Count - 1; i >= 0; i--)
            {
                moveCoinTask.Add(MoveCoinTask(coins[i]));
                await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
            }
        }

        private async UniTask MoveCoinTask(GameObject coinInstance)
        {
            await coinInstance.transform.DOMove(endPosition.position, duration).SetEase(Ease.InBack).ToUniTask();

            GameObject temp = coinInstance;
            coins.Remove(coinInstance);
            Destroy(temp);

            await ReactToCollectionCoin();
            SetCoin(coin + 1);
        }

        private async UniTask ReactToCollectionCoin()
        {
            if (coinReactionTween == null)
            {
                coinReactionTween = endPosition.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.1f).SetEase(Ease.InOutElastic);
                await coinReactionTween.ToUniTask();
                coinReactionTween = null;
            }
        }
    }
}
