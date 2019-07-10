using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : Singleton<AdManager>
{
    //**** Change Depending on the app *****//
    //private readonly string APP_ID = "ca-app-pub-1249591444731632~2490946798";

    //----- Test Adds ID's -----//
    private readonly string TEST_BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
    private readonly string TEST_INTERSTITIAL_ID = "ca-app-pub-3940256099942544/1033173712";
    private readonly string TEST_REWARDVIDEO_ID = "ca-app-pub-3940256099942544/5224354917";
    private readonly string TEST_DEVICE_ID = "2077ef9a63d2b398840261c8221a0c9b";

    //----- Ad's Variables
    private BannerView bannerAD;
    private InterstitialAd interstitialAD;
    private RewardBasedVideoAd rewardVideoAD;

    //----- Ad's Timer
    private Action timerCounter;
    private float timer = 0;
    private float timerOffset = 30f;

    
    void Start()
    {
        //Uncomment this when you publish the app
        //MobileAds.Initialize(APP_ID);

        RequestBanner();
        RequestInterstitial();
        RequestVideoAd();

        HandleBannerADEvents();
        HandleInterstitialADEvents();
    }

    private void Update()
    {
        timerCounter?.Invoke();
    }

    public void Init()
    {
        timer = 0;
        timerCounter += AddTime;
    }
    public bool CheckIfReady()
    {
        if(timer > timerOffset)
        {
            timer = 0;
            return true;
        }
        return false;
    }
    private void AddTime()
    {
        timer += Time.deltaTime;
    }

    //AD METHODS
    public void Display_Banner()
    {
        bannerAD.Show();
    }
    public void Display_InterstitialAD()
    {
        if (interstitialAD.IsLoaded())
        {
            interstitialAD.Show();
        }
        else Debug.LogWarning("Interstitial AD not loaded");

        RequestInterstitial();
    }
    public void Display_Reward_Video()
    {
        if (rewardVideoAD.IsLoaded())
        {
            rewardVideoAD.Show();
        }
        else Debug.LogWarning("Reward Video not Showing");
    }

    private void RequestBanner()
    {
        bannerAD = new BannerView(TEST_BANNER_ID, AdSize.SmartBanner, AdPosition.Bottom);

        //FOR REAL APP
        //AdRequest adRequest = new AdRequest.Builder().Build();

        //FOR TESTING
        AdRequest adRequest = new AdRequest.Builder()
            .AddTestDevice(TEST_DEVICE_ID)
            .Build();

        bannerAD.LoadAd(adRequest);
    }
    private void RequestInterstitial()
    {
        interstitialAD = new InterstitialAd(TEST_INTERSTITIAL_ID);

        //FOR REAL APP
        //AdRequest adRequest = new AdRequest.Builder().Build();

        //FOR TESTING
        AdRequest adRequest = new AdRequest.Builder()
            .AddTestDevice(TEST_DEVICE_ID)
            .Build();

        interstitialAD.LoadAd(adRequest);
    }
    private void RequestVideoAd()
    {
        rewardVideoAD = RewardBasedVideoAd.Instance;

        //FOR REAL APP
        //AdRequest adRequest = new AdRequest.Builder().Build();

        //FOR TESTING
        AdRequest adRequest = new AdRequest.Builder()
            .AddTestDevice(TEST_DEVICE_ID)
            .Build();

        rewardVideoAD.LoadAd(adRequest, TEST_REWARDVIDEO_ID);
    }

    //----- AD HANDLE EVENTS
    
    //--- Banner
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        //ad is loaded, show it
        Display_Banner();
    }
    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //ad failed to load, load it again
        RequestBanner();
    }
    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }
    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
    
    //--- Interstitial
    public void HandleOnAdClosedInterstitial(object sender, EventArgs args)
    {
        //Resume Time maybe
    }
    
    //--- Set Events
    private void HandleInterstitialADEvents()
    {
        interstitialAD.OnAdClosed += HandleOnAdClosedInterstitial;
    }
    private void HandleBannerADEvents()
    {
        // Called when an ad request has successfully loaded.
        bannerAD.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        bannerAD.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        bannerAD.OnAdOpening += HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        bannerAD.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        bannerAD.OnAdLeavingApplication += HandleOnAdLeavingApplication;
    }
}
