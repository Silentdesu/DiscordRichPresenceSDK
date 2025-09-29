using UnityEngine;

/// <summary>
/// Don't forget to take Application Id from created application in discord's developers website
/// </summary>
[HelpURL("https://discord.com/developers/applications")]
[CreateAssetMenu(fileName = "Discord SDK Config", menuName = "Discord SDK Config")]
public sealed class DiscordSDKConfig : ScriptableObject
{
    [field: SerializeField] public ulong AppId { get; private set; }
}