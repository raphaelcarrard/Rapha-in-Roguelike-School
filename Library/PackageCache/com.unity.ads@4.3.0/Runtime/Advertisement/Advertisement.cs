using System;
using UnityEngine.Advertisements.Platform;
using UnityEngine.Advertisements.Utilities;

namespace UnityEngine.Advertisements
{
    /// <summary>
    /// The wrapper class used to interact with the Unity Ads SDK.
    /// </summary>
    public static class Advertisement
    {
        private static IPlatform s_Platform;

        static Advertisement()
        {
            if (s_Platform == null)
            {
                s_Platform = CreatePlatform();
            }
        }

        /// <summary>
        /// Returns <c>true</c> if the SDK is initialized successfully, and <c>false</c> if it isn't.
        /// </summary>
        public static bool isInitialized => s_Platform.IsInitialized;

        /// <summary>
        /// Returns <c>true</c> if the SDK is supported on the current platform, and <c>false</c> if it isn't.
        /// </summary>
        public static bool isSupported => IsSupported();

        /// <summary>
        /// Returns <c>true</c> if the SDK is is in debug mode, and <c>false</c> if it isn't. Debug mode controls the level of logging from the SDK.
        /// </summary>
        public static bool debugMode
        {
            get => s_Platform.DebugMode;
            set => s_Platform.DebugMode = value;
        }

        /// <summary>
        /// Returns the current SDK version.
        /// </summary>
        public static string version => s_Platform.Version;

        /// <summary>
        /// Returns <c>true</c> if an ad is currently showing, and <c>false</c> if it isn't.
        /// </summary>
        public static bool isShowing => s_Platform.IsShowing;

        /// <summary>
        /// Initializes the SDK with a specified <a href="../manual/MonetizationResourcesDashboardGuide.html#project-settings">Game ID</a>.
        /// </summary>
        /// <param name="gameId">The platform-specific Unity game identifier for your Project, found on the <a href="https://operate.dashboard.unity3d.com/">developer dashboard</a>.</param>
        public static void Initialize(string gameId)
        {
            Initialize(gameId, false);
        }

        /// <summary>
        /// Initializes the SDK with a specified Game ID and test mode setting.
        /// </summary>
        /// <param name="gameId">The platform-specific Unity game identifier for your Project, found on the <a href="https://operate.dashboard.unity3d.com/">developer dashboard</a>.</param>
        /// <param name="testMode"><a href="../manual/MonetizationResourcesDashboardGuide.html#project-settings">Test mode</a> allows you to test your integration without serving live ads. Use <c>true</c> to initialize in test mode.</param>
        public static void Initialize(string gameId, bool testMode)
        {
            Initialize(gameId, testMode, null);
        }

        /// <summary>
        /// Initializes the SDK with a specified Game ID, test mode setting, and Placement load setting.
        /// </summary>
        /// <param name="gameId">The platform-specific Unity <a href="../manual/MonetizationResourcesDashboardGuide.html#project-settings">game identifier</a> for your Project, found on the <a href="https://operate.dashboard.unity3d.com/">developer dashboard</a>.</param>
        /// <param name="testMode"><a href="../manual/MonetizationResourcesDashboardGuide.html#project-settings">Test mode</a> allows you to test your integration without serving live ads. Use <c>true</c> to initialize in test mode.</param>
        /// <param name="initializationListener">Listener for IUnityAdsInitializationListener callbacks</param>
        public static void Initialize(string gameId, bool testMode, IUnityAdsInitializationListener initializationListener)
        {
            if (initializationListener == null)
            {
                Debug.LogError("initializationListener is null, you will not receive any callbacks");
            }
#if UNITY_2020_1_OR_NEWER && ENABLE_CLOUD_SERVICES_ADS && (UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR)
            if (testMode == false && UnityAdsSettings.testMode)
            {
                Debug.Log("Unity Ads is initializing in test mode since test mode is enabled in Service Window.");
            }
            s_Platform.Initialize(gameId, UnityAdsSettings.testMode || testMode, new UnityAdsInitializationListenerMainDispatch(initializationListener, s_Platform.UnityLifecycleManager));
#else
            s_Platform.Initialize(gameId, testMode, new UnityAdsInitializationListenerMainDispatch(initializationListener, s_Platform.UnityLifecycleManager));
#endif
        }

        /// <summary>
        /// Loads ad content for a specified Placement.
        /// </summary>
        /// <param name="placementId">The unique identifier for a specific Placement, found on the <a href="https://operate.dashboard.unity3d.com/">developer dashboard</a>.</param>
        /// <seealso cref="Advertisement.Initialize(string, bool, bool)"/>
        /// <seealso cref="Advertisement.Show(string)"/>
        public static void Load(string placementId)
        {
            Load(placementId, null);
        }

        /// <summary>
        /// Loads ad content for a specified Placement.
        /// </summary>
        /// <param name="placementId">The unique identifier for a specific Placement, found on the <a href="https://operate.dashboard.unity3d.com/">developer dashboard</a>.</param>
        /// <param name="loadListener">A listener for <c>IUnityAdsLoadListener</c> callbacks</param>
        /// <seealso cref="Advertisement.Initialize(string, bool, bool)"/>
        /// <seealso cref="Advertisement.Show(string)"/>
        public static void Load(string placementId, IUnityAdsLoadListener loadListener)
        {
            if (loadListener == null)
            {
                Debug.LogError("loadListener is null, you will not receive any callbacks");
            }
            s_Platform.Load(placementId, new UnityAdsLoadListenerMainDispatch(loadListener, s_Platform.UnityLifecycleManager));
        }


        /// <summary>
        /// Displays an ad in the default <-a href="../manual/MonetizationPlacements.html">Placement</a> if it is ready.
        /// </summary>
        /// <param name="showOptions">A collection of options for modifying ad behaviour.</param>
        [Obsolete("ShowOptions has been deprecated and no longer has callbacks. It's only function currently is to pass gamerSid")]
        public static void Show(ShowOptions showOptions)
        {
            Show(null, showOptions, null);
        }

        /// <summary>
        /// Displays an ad in a specified <a href="../manual/MonetizationPlacements.html">Placement</a> if it is ready.
        /// </summary>
        /// <param name="placementId">The unique identifier for a specific Placement, found on the <a href="https://operate.dashboard.unity3d.com/">developer dashboard</a>.</param>
        public static void Show(string placementId)
        {
            Show(placementId, null, null);
        }

        /// <summary>
        /// Displays an ad in a specified <a href="../manual/MonetizationPlacements.html">Placement</a> if it is ready.
        /// </summary>
        /// <param name="placementId">The unique identifier for a specific Placement, found on the <a href="https://operate.dashboard.unity3d.com/">developer dashboard</a>.</param>
        /// <param name="showOptions">A collection of options for modifying ad behaviour.</param>
        public static void Show(string placementId, ShowOptions showOptions)
        {
            Show(placementId, showOptions, null);
        }

        /// <summary>
        /// Displays an ad in a specified <a href="../manual/MonetizationPlacements.html">Placement</a> if it is ready.
        /// </summary>
        /// <param name="placementId">The unique identifier for a specific Placement, found on the <a href="https://operate.dashboard.unity3d.com/">developer dashboard</a>.</param>
        /// <param name="showListener">A listener for <c>IUnityAdsShowListener</c> callbacks</param>
        public static void Show(string placementId, IUnityAdsShowListener showListener)
        {
            Show(placementId, null, showListener);
        }

        /// <summary>
        /// Displays an ad in a specified <a href="../manual/MonetizationPlacements.html">Placement</a>
        /// </summary>
        /// <param name="placementId">The unique identifier for a specific Placement, found on the <a href="https://operate.dashboard.unity3d.com/">developer dashboard</a>.</param>
        /// <param name="showOptions">A collection of options for modifying ad behaviour.</param>
        /// <param name="showListener">A listener for <c>IUnityAdsShowListener</c> callbacks</param>
        public static void Show(string placementId, ShowOptions showOptions, IUnityAdsShowListener showListener)
        {
            if (showListener == null)
            {
                Debug.LogError("showListener is null, you will not receive any callbacks");
            }
            s_Platform.Show(placementId, showOptions, new UnityAdsShowListenerMainDispatch(showListener, s_Platform.UnityLifecycleManager));
        }

        /// <summary>
        /// Sets various metadata for the SDK.
        /// </summary>
        /// <param name="metaData">A metadata container.</param>
        public static void SetMetaData(MetaData metaData)
        {
            s_Platform.SetMetaData(metaData);
        }

        private static IPlatform CreatePlatform()
        {
            try
            {
                IUnityLifecycleManager unityLifecycleManager = new UnityLifecycleManager();
                INativePlatform nativePlatform;
                INativeBanner nativeBanner;
#if UNITY_EDITOR
                nativeBanner = new Platform.Editor.EditorBanner();
                nativePlatform = new Platform.Editor.EditorPlatform();
#elif UNITY_ANDROID
                nativeBanner = new Platform.Android.AndroidBanner();
                nativePlatform = new Platform.Android.AndroidPlatform();;
#elif UNITY_IOS
                nativeBanner = new Platform.iOS.IosBanner();
                nativePlatform = new Platform.iOS.IosPlatform();
#else
                nativeBanner = new Platform.Unsupported.UnsupportedBanner();
                nativePlatform = new Platform.Unsupported.UnsupportedPlatform();
#endif
                IBanner banner = new Advertisements.Banner(nativeBanner, unityLifecycleManager);
                return new Platform.Platform(nativePlatform, banner, unityLifecycleManager);
            }
            catch (Exception exception)
            {
                try
                {
                    Debug.LogError("Initializing Unity Ads.");
                    Debug.LogError(exception.Message);
                }
                catch (MissingMethodException)
                {}

                var unsupportedPlatform = new Platform.Unsupported.UnsupportedPlatform();
                var coroutineExecutor = new UnityLifecycleManager();
                var unsupportedBanner = new Platform.Unsupported.UnsupportedBanner();
                var genericBanner = new Advertisements.Banner(unsupportedBanner, coroutineExecutor);
                return new Platform.Platform(unsupportedPlatform, genericBanner, coroutineExecutor);
            }
        }

        private static bool IsSupported()
        {
            return Application.isEditor || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        }

        /// <summary>
        /// A static class for implementing banner ads.
        /// </summary>
        public static class Banner
        {
            /// <summary>
            /// Loads the banner ad with the default <a href="../manual/MonetizationPlacements.html">Placement</a>, and no callbacks.
            /// </summary>
            public static void Load()
            {
                Load(null, null);
            }

            /// <summary>
            /// Loads the banner ad with the default <a href="../manual/MonetizationPlacements.html">Placement</a>, but fires the <c>loadCallback</c> callback on successful load, and the <c>errorCallback</c> callback on failure to load.
            /// </summary>
            /// <param name="options">A collection of options that notify the SDK of events when loading the banner.</param>
            public static void Load(BannerLoadOptions options)
            {
                Load(null, options);
            }

            /// <summary>
            /// Loads the banner ad with a specified <a href="../manual/MonetizationPlacements.html">Placement</a>, and no callbacks.
            /// </summary>
            /// <param name="placementId">The unique identifier for a specific Placement, found on the <a href="https://operate.dashboard.unity3d.com/">developer dashboard</a>.</param>
            public static void Load(string placementId)
            {
                Load(placementId, null);
            }

            /// <summary>
            /// Loads the banner ad with a specified <a href="../manual/MonetizationPlacements.html">Placement</a>, but fires the <c>loadCallback</c> callback on successful load, and the <c>errorCallback</c> callback on failure to load.
            /// </summary>
            /// <param name="placementId">The unique identifier for a specific Placement, found on the <a href="https://operate.dashboard.unity3d.com/">developer dashboard</a>.</param>
            /// <param name="options">A collection of options that notify the SDK of events when loading the banner.</param>
            public static void Load(string placementId, BannerLoadOptions options)
            {
                s_Platform.Banner.Load(placementId, options);
            }

            /// <summary>
            /// Displays the banner ad with a specified <a href="../manual/MonetizationPlacements.html">Placement</a>, and no callbacks.
            /// </summary>
            /// <param name="placementId">The unique identifier for a specific Placement, found on the <a href="https://operate.dashboard.unity3d.com/">developer dashboard</a>.</param>
            public static void Show(string placementId)
            {
                Show(placementId, null);
            }

            /// <summary>
            /// Displays the banner ad with a specified <a href="../manual/MonetizationPlacements.html">Placement</a>, but fires the <c>showCallback</c> callback if the banner is visible, and the <c>hideCallback</c> if it isn't.
            /// </summary>
            /// <param name="placementId">The unique identifier for a specific Placement, found on the <a href="https://operate.dashboard.unity3d.com/">developer dashboard</a>.</param>
            /// <param name="options">A collection of options that notify the SDK of events when displaying the banner.</param>
            public static void Show(string placementId, BannerOptions options)
            {
                if (string.IsNullOrEmpty((placementId)))
                {
                    Debug.LogWarning("placementId is empty");
                }
                s_Platform.Banner.Show(string.IsNullOrEmpty(placementId) ? null : placementId, options);
            }

            /// <summary>
            /// Allows you to hide a banner ad, instead of destroying it altogether.
            /// </summary>
            public static void Hide(bool destroy = false)
            {
                s_Platform.Banner.Hide(destroy);
            }

            /// <summary>
            /// <para>Sets the position of the banner ad, using the <a href="../api/UnityEngine.Advertisements.BannerPosition.html"><c>BannerPosition</c></a> enum.</para>
            /// <para>Banner position defaults to <c>BannerPosition.BOTTOM_CENTER</c>.</para>
            /// </summary>
            /// <param name="position">An enum representing the on-screen anchor position of the banner ad.</param>
            public static void SetPosition(BannerPosition position)
            {
                s_Platform.Banner.SetPosition(position);
            }

            /// <summary>
            /// Returns <c>true</c> if a banner is currently available, and <c>false</c> if it isn't.
            /// </summary>
            public static bool isLoaded => s_Platform.Banner.IsLoaded;
        }
    }
}
