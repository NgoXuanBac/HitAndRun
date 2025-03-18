using System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace HitAndRun.Ads
{
    public class MbRewardAds : MbSingleton<MbRewardAds>
    {
#if UNITY_ANDROID
        private readonly string _rewardedId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
    private readonly string _rewardedId = "ca-app-pub-3940256099942544/1712485313"; 
#endif
        private RewardedAd _rewardedAd;

        private void Start()
        {
            MobileAds.RaiseAdEventsOnUnityMainThread = true;
            MobileAds.Initialize(initStatus =>
            {
                LoadRewardedAd();
            });
        }

        public void ShowRewardedAd(Action<bool> actionReward)
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                _rewardedAd.Show((reward) =>
                {
                    actionReward?.Invoke(true);
                });
            }
            else
            {
                actionReward?.Invoke(false);
            }
        }

        public void LoadRewardedAd()
        {

            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }
            var adRequest = new AdRequest();
            adRequest.Keywords.Add("unity-admob-sample");

            RewardedAd.Load(_rewardedId, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    return;
                }

                _rewardedAd = ad;
            });
        }

    }

}
