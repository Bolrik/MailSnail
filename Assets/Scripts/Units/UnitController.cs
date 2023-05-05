using FreschGames.Core.Input;
using MailSnail.Board;
using MailSnail.Game;
using MailSnail.TurnOrder;
using System;
using UnityEngine;

namespace MailSnail.Units
{
    public class UnitController : MonoBehaviour
    {
        [field: SerializeField, Header("Data")] public GameData GameData { get; private set; }

        BoardManager BoardManager { get; set; }

        [field: SerializeField] public InputValue Move { get; private set; }
        [field: SerializeField] public Unit Unit { get; private set; }

        [field: SerializeField] public int Faction { get; private set; }

        private TurnToken TurnToken { get; set; }

        public Action<TurnToken> OnTurnTokenSet { get; set; }
        float AutoMoveTimer { get; set; }
        bool AutoMove => this.AutoMoveTimer <= 0;

        private void Start()
        {
            this.BoardManager = this.GameData.BoardManager;
            this.GameData.UnitControllerManager.Add(this);
        }

        private void Update()
        {
            if (this.TurnToken == null)
                return;

            if (this.Move.IsPressed)
                this.AutoMoveTimer = (this.AutoMoveTimer - Time.deltaTime).ClampMin(0);
            else
                this.AutoMoveTimer = 0.1f;

            if (!this.Move.WasPressed && !this.AutoMove)
                return;

            if (this.AutoMove)
                this.AutoMoveTimer = .1f;

            var input = this.Move.Read<Vector2>();

            bool result = this.Unit.Move(new MoveInput((int)input.x, (int)input.y));

            if (result)
            {
                this.TurnToken.SetIsDone(result);
            }
        }

        public void SetToken(TurnToken turnToken)
        {
            this.TurnToken = turnToken;

            this.SetUnit(this.TurnToken.Unit);

            this.OnTurnTokenSet?.Invoke(this.TurnToken);
        }

        public void ClearToken()
        {
            this.TurnToken = null;
            this.SetUnit(null);
        }

        private void SetUnit(Unit unit)
        {
            if (this.Unit != null)
            {
                this.Unit.OnTileChanged -= this.Unit_OnTileChanged;
            }

            this.Unit = unit;

            if (this.Unit != null)
            {
                this.Unit.OnTileChanged += this.Unit_OnTileChanged;
            }
        }

        private void Unit_OnTileChanged(TileState from, TileState to)
        {
            int x = to.X;
            int y = to.Y;

            if (this.Unit.ItemSlot.Item == null)
            {
                if (to.Item != null)
                {
                    this.Unit.ItemSlot.Take(to);
                }
            }

            // Only trigger for player
            if (this.Faction > 0)
                return;

            if (to.TileContentData == this.GameData.TileContentDataPool.Town &&
                this.Unit.ItemSlot.Item != null)
            {
                this.Unit.ItemSlot.Set(null);
                this.GameData.GameManager.Expand();
            }

            BoardLink[] links = this.BoardManager.Board.State.Links;

            foreach (var link in links)
            {
                foreach (var position in link.Positions)
                {
                    if (position.X == x && position.Y == y)
                    {
                        this.GameData.GameManager.ActivateLink(link);
                    }
                }
            }
        }
    }

}