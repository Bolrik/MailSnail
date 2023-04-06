using UnityEngine;

namespace MailSnail.Board
{
    [CreateAssetMenu(fileName = "Tile Content Data", menuName = "Data/Board/new Tile Content Data")]
    public class TileContentData : ScriptableObject
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }

        [field: SerializeField] public Vector2 Offset { get; private set; }

        [field: SerializeField] public bool IsWalkable { get; private set; }
    }
}