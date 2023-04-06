using UnityEngine;

namespace MailSnail.Board
{
    [CreateAssetMenu(fileName = "Tile Settings", menuName = "Settings/Board/new Tile Settings")]
    public class TileSettings : ScriptableObject
    {
        [field: SerializeField, Header("Misc")] public bool ShowTileInfo { get; private set; } = false;
    }
}