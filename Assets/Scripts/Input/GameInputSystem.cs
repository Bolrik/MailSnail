using MailSnail.Board;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MailSnail.Input
{
    public class GameInputSystem : FreschGames.Core.Input.InputManagerBase
    {
        protected override IInputActionCollection2 GetInputAssetObject()
        {
            return new GameInput();
        }
    }
}