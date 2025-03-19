using System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace HitAndRun.Ads
{
    public class MbRewardAds : MbSingleton<MbRewardAds>
    {

#if UNITY_ANDROID
        private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
  private string _adUnitId = "unused";
#endif

        private Action _onAdClosed;
        private RewardedAd _rewardedAd;

        public void LoadRewardedAd()
        {
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }

            var adRequest = new AdRequest();

            RewardedAd.Load(_adUnitId, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    if (error != null || ad == null) return;
                    _rewardedAd = ad;
                    RegisterReloadHandler(_rewardedAd);
                });
        }

        public void ShowRewardedAd(Action receive, Action onAdClosed = null)
        {

            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                _onAdClosed = onAdClosed;
                _rewardedAd.Show((Reward reward) =>
                {
                    receive?.Invoke();
                });
            }
        }

        private void RegisterReloadHandler(RewardedAd ad)
        {
            ad.OnAdFullScreenContentClosed += () =>
            {
                _onAdClosed?.Invoke();
                LoadRewardedAd();
            };
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                LoadRewardedAd();
            };
        }
    }

}
