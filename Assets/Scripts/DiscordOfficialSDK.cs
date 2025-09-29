using Discord.Sdk;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class DiscordOfficialSDK : MonoBehaviour
{
    [SerializeField] private DiscordSDKConfig config;

    private Client _client;

    private void Awake()
    {
        _client = new Client();
        _client.SetApplicationId(config.AppId);
        // _client.RegisterLaunchSteamApplication(config.AppId, STEAM_APP_ID);

        _client.SetActivityJoinWithApplicationCallback(OnInviteJoinCallback);
    }

    private void OnInviteJoinCallback(ulong applicationId, string joinSecret)
    {
        // Game's join logic by joinSecret
    }

    private void UpdateRichPresence(string details, string state)
    {
        var activity = new Activity();
        activity.SetDetails(details);
        activity.SetState(state);

        _client.UpdateRichPresence(activity, OnRichPresenceUpdatedCallback);
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
        var activity = new Activity();
        activity.SetDetails(details);
        activity.SetState(state);

        var party = new ActivityParty();
        party.SetId(partyId);
        party.SetCurrentSize(currentPartySize);
        party.SetMaxSize(maxPartySize);
        activity.SetParty(party);
        
        var secrets = new ActivitySecrets();
        secrets.SetJoin(joinSecret);
        activity.SetSecrets(secrets);
        
        _client.UpdateRichPresence(activity, OnRichPresenceUpdatedCallback);
    }

    private void OnRichPresenceUpdatedCallback(ClientResult result)
    {
        if (result.Successful())
        {
            Debug.Log($"Rich Presence Updated Successfully");
        }
        else
        {
            Debug.Log($"Couldn't update Discord Rich Presence: {result.Error()}");
        }
    }
}