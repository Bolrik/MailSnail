using FreschGames.Core.Input;
using MailSnail.Board;
using MailSnail.GameCamera;
using MailSnail.Items;
using MailSnail.TurnOrder;
using MailSnail.Units;
using UnityEngine;

namespace MailSnail.Game
{
    [CreateAssetMenu(fileName = "Game Data", menuName = "Data/Game/new Game Data")]
    public class GameData : ScriptableObject
    {
        [field: SerializeField, Header("Data")] public PrefabData PrefabData { get; private set; }

        [field: SerializeField, Header("Pool")] public TileDataPool TileDataPool { get; private set; }
        [field: SerializeField] public TileContentDataPool TileContentDataPool { get; private set; }
        [field: SerializeField] public ItemPool ItemPool { get; private set; }

        [field: SerializeField, Header("Manager")] public GameManager GameManager { get; private set; }
        [field: SerializeField] public BoardManager BoardManager { get; private set; }
        [field: SerializeField] public TurnManager TurnManager { get; private set; }
        [field: SerializeField] public UnitControllerManager UnitControllerManager { get; private set; }
        [field: SerializeField] public GameCameraManager GameCamera { get; private set; }


        [field: SerializeField, Header("Input")] public Input Input { get; private set; }
    }

    [System.Serializable]
    public class Input
    {
        [field: SerializeField] public InputValue Move { get; private set; }
        [field: SerializeField] public InputValue Menu { get; private set; }
    }
}