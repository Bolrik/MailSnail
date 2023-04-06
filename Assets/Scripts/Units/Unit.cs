using MailSnail.Board;
using MailSnail.Game;
using MailSnail.Items;
using System;
using UnityEngine;

namespace MailSnail.Units
{
    public class Unit : MonoBehaviour
    {
        [field: SerializeField] public GameData GameData { get; private set; }

        [field: SerializeField] public int X { get; private set; }
        [field: SerializeField] public int Y { get; private set; }


        [field: SerializeField] private Transform VisualRoot { get; set; }

        BoardManager BoardManager { get; set; }
        BoardState BoardState { get; set; }
        public TileState Tile { get; private set; }

        public ItemSlot ItemSlot { get; private set; } = new ItemSlot();

        [field: SerializeField] public int Faction { get; private set; }


        public Action<TileState, TileState> OnTileChanged { get; set; }

        bool IsInitialized { get; set; }


        private void Start()
        {
            this.Initialize();
        }

        public void Initialize()
        {
            if (this.IsInitialized)
                return;

            this.BoardManager = this.GameData.BoardManager;

            // this.Board = this.GameData.BoardManager.Board;
            this.GameData.TurnManager.Add(this);

            this.IsInitialized = true;
        }

        public void ChangeBoardState(BoardState boardState)
        {
            if (this.BoardState != null)
            {
                this.BoardState.SetUnit(this.X, this.Y, null);
            }

            this.BoardState = boardState;
            Debug.Log($"Now on Board '{boardState.ID}'");
        }

        public bool Move(MoveInput moveInput)
        {
            int x = this.X + moveInput.X;
            int y = this.Y + moveInput.Y;

            if (moveInput.X < 0)
            {
                this.VisualRoot.localScale = new Vector3(1, 1, 1);
            }
            else if (moveInput.X > 0)
            {
                this.VisualRoot.localScale = new Vector3(-1, 1, 1);
            }

            return this.MoveTo(x, y);
        }

        public bool MoveTo(int x, int y)
        {
            if (!this.BoardState.IsValid(x, y, out TileState tile))
            {
                return false;
            }

            if (!this.CanWalkTo(tile))
            {
                return false;
            }

            this.BoardState.SetUnit(this.X, this.Y, null);

            this.X = x;
            this.Y = y;

            this.BoardState.SetUnit(this.X, this.Y, this);

            tile = this.BoardState.Get(this.X, this.Y);

            this.Tile = tile;

            this.transform.position = new Vector3(this.X, this.Y);

            this.OnTileChanged?.Invoke(this.Tile, tile);

            return true;
        }

        private bool CanWalkTo(TileState tileState)
        {
            if (tileState.TileContentData?.IsWalkable == false)
                return false;

            if (tileState.Unit != null)
                return false;

            return true;
        }

        public void DropItem()
        {
            if (this.ItemSlot.Item == null)
                return;

            this.ItemSlot.Take(this.Tile);
        }
    }

    public class MoveInput
    {
        public int X { get; private set; }
        public int Y { get; private set; }


        public MoveInput(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

}