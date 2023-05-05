using FreschGames.Core.Input;
using MailSnail.Board;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MailSnail.Input
{
    public class GameInputSystem : InputComponent
    {
        protected override IInputActionCollection2 GetInputAssetObject()
        {
            return new GameInput();
        }
    }
}