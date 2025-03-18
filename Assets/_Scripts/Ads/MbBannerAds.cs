using UnityEngine;
using System;
using GoogleMobileAds.Api;
using System.Collections.Generic;

namespace HitAndRun.Ads
{
    public class MbBannerAds : MonoBehaviour
    {
        private BannerView _bannerView;

        private void Start()
        {
            var requestConfiguration = new RequestConfiguration
            {
                TestDeviceIds = new List<string>
                {
                    AdRequest.TestDeviceSimulator,
                    #if UNITY_IPHONE
                    "96e23e80653bb28980d3f40beb58915c"
                    #elif UNITY_ANDROID
                    "75EF8D155528C04DACBBA6F36F433035"
                    #endif
                }
            };
            MobileAds.SetRequestConfiguration(requestConfiguration);

            MobileAds.Initialize((InitializationStatus status) =>
            {
                RequestBanner();
            });
        }
        private void RequestBanner()
        {
#if UNITY_EDITOR
            string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-3212738706492790/6113697308";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3212738706492790/5381898163";
#else
            string adUnitId = "unexpected_platform";
#endif

            _bannerView?.Destroy();

            // var adaptiveSize =
            //         AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
            // _bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Bottom);

            _bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);


            var adRequest = new AdRequest();

            _bannerView.LoadAd(adRequest);
        }


    }

}
