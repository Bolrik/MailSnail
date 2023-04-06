using UnityEngine;

namespace MailSnail.Game
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Settings/Game/new Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [field: SerializeField, Header("Misc")] public int FallbackSeed { get; private set; }
    }
}