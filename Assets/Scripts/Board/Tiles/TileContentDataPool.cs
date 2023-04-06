using UnityEngine;

namespace MailSnail.Board
{
    [CreateAssetMenu(fileName = "Tile Content Data Pool", menuName = "Pool/Board/new Tile Content Data Pool")]
    public class TileContentDataPool : ScriptableObject
    {
        [field: SerializeField] public TileContentData Tree { get; private set; }
        [field: SerializeField] public TileContentData Rock { get; private set; }
        [field: SerializeField] public TileContentData Town { get; private set; }
    }


}