using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using System;
using HitAndRun;


public class MbAdmobAds : MbSingleton<MbAdmobAds>
{

#if UNITY_ANDROID
    private readonly string _bannerId = "ca-app-pub-1385093244148841/2952458907";
    private readonly string _interId = "ca-app-pub-3940256099942544/1033173712";
    private readonly string _rewardedId = "ca-app-pub-3940256099942544/5224354917";
    private readonly string _nativeId = "ca-app-pub-3940256099942544/2247696110";


    private readonly string _storeUrl79k = "https://play.google.com/store/apps/details?id=com.yourgame.package";

#elif UNITY_IPHONE
    private readonly string _bannerId = "ca-app-pub-3940256099942544/2934735716";
    private readonly string _interId = "ca-app-pub-3940256099942544/4411468910";
    private readonly string _rewardedId = "ca-app-pub-3940256099942544/1712485313"; // ID qu?ng c�o video
    private readonly string _nativeId = "ca-app-pub-3940256099942544/3986624511";

  
    private readonly string _storeUrl79k = "https://apps.apple.com/app/id123456789";
#endif

    private BannerView _bannerView;
    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;
    private NativeAd _nativeAd;


    private void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus =>
        {
            LoadBannerAd();
            LoadRewardedAd();
        });
    }

    #region Banner

    private void LoadBannerAd()
    {
        CreateBannerView();

        ListenToBannerEvents();

        if (_bannerView == null)
        {
            CreateBannerView();
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        _bannerView.LoadAd(adRequest);
    }

    private void CreateBannerView()
    {

        if (_bannerView != null)
        {
            DestroyBannerAd();
        }
        _bannerView = new BannerView(_bannerId, AdSize.MediumRectangle, AdPosition.Bottom);
    }
    private void ListenToBannerEvents()
    {
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + _bannerView.GetResponseInfo());
        };

        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Banner view paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }
    private void DestroyBannerAd()
    {

        if (_bannerView != null)
        {
            print("Destroying banner Ad");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }
    #endregion

    #region Interstitial

    public void LoadInterstitialAd()
    {

        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        InterstitialAd.Load(_interId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                print("Interstitial ad failed to load" + error);
                return;
            }

            _interstitialAd = ad;
            InterstitialEvent(_interstitialAd);
        });

    }
    public void ShowInterstitialAd()
    {

        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            _interstitialAd.Show();
        }
    }
    public void InterstitialEvent(InterstitialAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Interstitial ad paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    #endregion

    #region Rewarded

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
            RewardedAdEvents(_rewardedAd);
        });
    }

    public void RewardedAdEvents(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Rewarded ad paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            LoadRewardedAd();
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    #endregion
    #region Store

    public void OpenStoreFor79k()
    {
        Debug.Log("Opening store for 79k package...");
        Application.OpenURL(_storeUrl79k);
    }

    #endregion

    #region Native

    public Image img;

    public void RequestNativeAd()
    {
        AdLoader adLoader = new AdLoader.Builder(_nativeId).ForNativeAd().Build();

        adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;

        adLoader.LoadAd(new AdRequest()); // Updated line
    }

    private void HandleNativeAdLoaded(object sender, NativeAdEventArgs e)
    {
        this._nativeAd = e.nativeAd;

        Texture2D iconTexture = this._nativeAd.GetIconTexture();
        Sprite sprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), Vector2.one * .5f);

        img.sprite = sprite;

    }


    #endregion

}
