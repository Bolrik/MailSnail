using UnityEngine;

namespace MailSnail.Board
{
    [CreateAssetMenu(fileName = "Tile Data Pool", menuName = "Pool/Board/new Tile Data Pool")]
    public class TileDataPool : ScriptableObject
    {
        [field: SerializeField] public TileData Grass { get; private set; }
        [field: SerializeField] public TileData Ground { get; private set; }
    }
}