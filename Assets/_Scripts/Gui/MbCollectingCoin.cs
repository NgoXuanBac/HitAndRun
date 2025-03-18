//using System;
//using System.Collections.Generic;
//using Cysharp.Threading.Tasks;
//using DG.Tweening;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;
//using Random = UnityEngine.Random;
//using UnityEditor;

//namespace HitAndRun.Gui
//{
//    public class MbCollectingCoin : MonoBehaviour
//    {
//        [Header("Collection")]
//        [SerializeField] private GameObject coinPrefab;

//        [SerializeField] private Transform coinParent;

//        [SerializeField] private Transform spawnLocation;

//        [SerializeField] private Transform endPosition;
//        [SerializeField] private TextMeshProUGUI _coinText;
//        [SerializeField] private float duration;
//        [SerializeField] private int coinAmount;

//        [SerializeField] private float minX;

//        [SerializeField] private float maxX;

//        [SerializeField] private float minY;

//        [SerializeField] private float maxY;

//        List<GameObject> coins = new List<GameObject>();

//        private Tween coinReactionTween;

//        private int coin;

//        [Header("UI Buttons")]
//        [SerializeField] private Button claimButton;
//        [SerializeField] private Button claimAdsButton;
//        void Start()
//        {
//            claimButton.onClick.AddListener(() => CollectCoins());
//            claimAdsButton.onClick.AddListener(() => CollectCoins());
//        }
//        private async void CollectCoins()
//        {
//            // Reset
//            SetCoin(0);
//            for (int i = 0; i < coins.Count; i++)
//            {
//                Destroy(coins[i]);
//            }
//            coins.Clear();
//            // Spawn the coin to a specific location with random value
//            Debug.Log("Coins have been reset.");

//            List<UniTask> spawnCoinTaskList = new List<UniTask>();
//            for (int i = 0; i < coinAmount; i++)
//            {
//                GameObject coinInstance = Instantiate(coinPrefab, coinParent);
//                float xPosition = spawnLocation.position.x + Random.Range(minX, maxX);
//                float yPosition = spawnLocation.position.y + Random.Range(minY, maxY);

//                coinInstance.transform.position = new Vector3(xPosition, yPosition);

//                Debug.Log($"Spawning coin {i + 1} at position: {coinInstance.transform.position}");

//                spawnCoinTaskList.Add(coinInstance.transform.DOPunchPosition(new Vector3(0, 30, 0), Random.Range(0, 1f)).SetEase(Ease.InOutElastic)
//                    .ToUniTask());
//                coins.Add(coinInstance);
//                await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
//            }
//            Debug.Log($"Total {coinAmount} coins spawned.");

//            await UniTask.WhenAll(spawnCoinTaskList);
//            // Move all the coins to the coin label
//            await MoveCoinsTask();
//            // Animation the reaction when collecting coin
//        }

//#if UNITY_EDITOR
//        [CustomEditor(typeof(MbCollectingCoin))]
//        public class ETeam1Inspector : Editor
//        {
//            public override void OnInspectorGUI()
//            {
//                MbCollectingCoin collectingCoin = (MbCollectingCoin)target;

//                DrawDefaultInspector();

//                if (GUILayout.Button("Collecting"))
//                {
//                    collectingCoin.CollectCoins();
//                }
//            }
//        }
//#endif

//        private void SetCoin(int value)
//        {
//            coin = value;
//            _coinText.text = coin.ToString();
//        }
//        private async UniTask MoveCoinsTask()
//        {
//            List<UniTask> moveCoinTask = new List<UniTask>();
//            for (int i = coins.Count - 1; i >= 0; i--)
//            {
//                moveCoinTask.Add(MoveCoinTask(coins[i]));
//                await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
//            }
//        }

//        private async UniTask MoveCoinTask(GameObject coinInstance)
//        {
//            await coinInstance.transform.DOMove(endPosition.position, duration).SetEase(Ease.InBack).ToUniTask();

//            GameObject temp = coinInstance;
//            coins.Remove(coinInstance);
//            Destroy(temp);

//            await ReactToCollectionCoin();
//            SetCoin(coin + 1);
//        }

//        private async UniTask ReactToCollectionCoin()
//        {
//            if (coinReactionTween == null)
//            {
//                coinReactionTween = endPosition.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.1f).SetEase(Ease.InOutElastic);
//                await coinReactionTween.ToUniTask();
//                coinReactionTween = null;
//            }
//        }

//    }
//}
