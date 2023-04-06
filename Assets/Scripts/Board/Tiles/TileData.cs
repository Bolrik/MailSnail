using UnityEngine;

namespace MailSnail.Board
{
    [CreateAssetMenu(fileName = "Tile Data", menuName = "Data/Board/new Tile Data")]
    public class TileData : ScriptableObject
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}