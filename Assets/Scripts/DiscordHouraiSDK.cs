using System;
using Discord;
using UnityEngine;

public class DiscordHouraiSDK : MonoBehaviour
{
    [SerializeField] private DiscordSDKConfig config;

    private Discord.Discord _discord;
    private ActivityManager _activityManager;

    private void Awake()
    {
        _discord = new Discord.Discord((long)config.AppId, (ulong)CreateFlags.NoRequireDiscord);
        _activityManager = _discord.GetActivityManager();

        _activityManager.OnActivityJoin += OnInviteJoinCallback;
    }

    private void Update() => _discord.RunCallbacks();

    private void OnDestroy()
    {
        if (_activityManager != null)
        {
            _activityManager.OnActivityJoin -= OnInviteJoinCallback;
        }
    }

    private void OnInviteJoinCallback(string secret)
    {
        // Game's join logic by joinSecret
    }

    private void UpdateRichPresence(string details, string state)
    {
        var activity = new Activity()
        {
            Details = details,
            State = state,
        };

        _activityManager.UpdateActivity(activity, OnRichPresenceUpdatedCallback);
    }

    /// <summary>
    /// For game invitation feature to send invite via discord's chat
    /// </summary>
    /// <param name="details"></param>
    /// <param name="state"></param>
    /// <param name="partyId">Id has to be different to prevent discord hook up any old game invitation in chat</param>
    /// <param name="currentPartySize"></param>
    /// <param name="maxPartySize"></param>
    /// <param name="joinSecret">Your lobby's id/code</param>
    private void UpdateGameInvitation(string details, string state, string partyId, int currentPartySize,
        int maxPartySize, string joinSecret)
    {
        var activity = new Activity()
        {
            Details = details,
            State = state,

            Party =
            {
                Id = partyId,
                Size =
                {
                    CurrentSize = currentPartySize,
                    MaxSize = maxPartySize
                }
            },

            Secrets =
            {
                Join = joinSecret
            }
        };
        
        _activityManager.UpdateActivity(activity, OnRichPresenceUpdatedCallback);
    }

    private void OnRichPresenceUpdatedCallback(Result result) =>
        Debug.Log(
            result == Result.Ok ? $"Rich Presence Updated Successfully" : $"Couldn't update Discord Rich Presence");
}