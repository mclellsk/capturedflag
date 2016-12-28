using UnityEngine;
using UnityEngine.Advertisements;
using CapturedFlag.Engine;
using System.Collections.Generic;

namespace CapturedFlag.UnityAds
{
    /// <summary>
    /// This component should be attached to an empty gameobject. A simple wrapper for the UnityAds plugin,
    /// has close and finish callbacks and can set the number of games that can be played between ads.
    /// </summary>
    public class AdManager : MonoBehaviour
    {
        public const string DEFAULT_VIDEO = "defaultVideoAndPictureZone";
        public const string REWARD_VIDEO = "rewardedVideoZone";

        /// <summary>
        /// Dictionary containing all instances available of the different Ad Manager types, where the key is the zone type.
        /// </summary>
        public static Dictionary<string, AdManager> instances = new Dictionary<string, AdManager>();

        /// <summary>
        /// The different ad types that are available with UnityAds. Skippable and unskippable ads.
        /// </summary>
        public static string[] zones = new string[] { DEFAULT_VIDEO, REWARD_VIDEO };

        /// <summary>
        /// Ad closed by user callback.
        /// </summary>
        public event System.Action OnClose;
        /// <summary>
        /// Ad played through completely before closing callback.
        /// </summary>
        public event System.Action OnFinish;

        /// <summary>
        /// Ad type to display in this particular ad.
        /// </summary>
        [HideInInspector]
        public int zoneIndex = 0;
        /// <summary>
        /// Maximum number of games to wait before next ad is delivered.
        /// </summary>
        public int maxGamesBetweenAds = 5;

        /// <summary>
        /// The UnityAd game id associated with this application, found on the user control panel of the UnityAds site.
        /// </summary>
        public string gameId = "23892";

        /// <summary>
        /// Determines whether the ad should be played on the next update.
        /// </summary>
        private bool _bPlayAd = false;
        /// <summary>
        /// Determines whether the ad has already played (whether skipped or not).
        /// </summary>
        private bool _bAdPlayed = false;

        /// <summary>
        /// Games that have been played by the user so far.
        /// </summary>
        public virtual int GamesPlayed { get; set; }

        /// <summary>
        /// Returns the number of games remaining to be played before the next ad is delivered.
        /// </summary>
        public int GamesBeforeAd
        {
            get
            {
                return maxGamesBetweenAds - GamesPlayed;
            }
        }

        /// <summary>
        /// Returns whether the ad of the specified zone type in the manager is ready for delivery and if there are no more games left to play before the next ad.
        /// </summary>
        public bool IsReady
        {
            get
            {
                return (GamesBeforeAd == 1 && Advertisement.isReady(zones[zoneIndex]));
            }
        }

        public virtual void Awake()
        {
            if (!instances.ContainsKey(zones[zoneIndex]))
            {
                instances.Add(zones[zoneIndex], this);
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public virtual void Start()
        {
            if (Advertisement.isSupported)
            {
                Advertisement.allowPrecache = true;
                Advertisement.Initialize(gameId);
                LogTool.LogDebug("Advertisements precached.");
            }
            else
            {
                LogTool.LogWarning("Platform not supported");
            }
        }

        public virtual void Update()
        {
            if (zones.Length > 0)
            {
                if (!_bAdPlayed && _bPlayAd)
                {
                    if (IsReady)
                    {
                        Advertisement.Show(
                            zones[zoneIndex],
                            new ShowOptions
                            {
                                pause = true,
                                resultCallback = result =>
                                {
                                    switch (result)
                                    {
                                        case ShowResult.Finished:
                                            if (OnFinish != null)
                                            {
                                                OnFinish();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            });

                        _bAdPlayed = true;
                    }
                }

                if (!Advertisement.isShowing && _bAdPlayed)
                {
                    if (OnClose != null)
                    {
                        OnClose();
                    }

                    _bPlayAd = false;
                    _bAdPlayed = false;
                }
            }
        }

        /// <summary>
        /// Flags the ad manager to deliver an ad if the manager is ready to deliver and an ad is not already being played.
        /// </summary>
        public bool PlayAd()
        {
            if (!IsReady)
            {
                _bPlayAd = false;
                _bAdPlayed = false;
            }
            else
            {
                _bPlayAd = true;
            }

            IncrementGamesPlayed();

            return _bPlayAd;
        }

        public void IncrementGamesPlayed()
        {
            if (maxGamesBetweenAds > 0)
                GamesPlayed = (GamesPlayed + 1) % maxGamesBetweenAds;
            else
                GamesPlayed = 0;
        }
    }
}
