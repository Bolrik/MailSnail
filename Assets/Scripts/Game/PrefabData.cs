using MailSnail.Board;
using MailSnail.Units;
using UnityEngine;

namespace MailSnail.Game
{
    [CreateAssetMenu(fileName = "Prefab Data", menuName = "Data/Game/new Prefab Data")]
    public class PrefabData : ScriptableObject
    {
        [field: SerializeField, Header("Board")] public Tile Tile { get; private set; }

        [field: SerializeField, Header("Units")] public Unit Player { get; private set; }
    }
}