using FreschGames.Core.Managers;
using MailSnail.Game;
using MailSnail.Misc;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MailSnail.Board
{
    [CreateAssetMenu(fileName = "Board Manager", menuName = "Manager/Board/new Board Manager")]
    public class BoardManager : ManagerComponent
    {
        [field: SerializeField, Header("Data")] public GameData GameData { get; private set; }

        [field: SerializeField, Header("Exchange")] public Board Board { get; private set; }


        [field: SerializeField] public int ScaleX { get; private set; } = 19;
        [field: SerializeField] public int ScaleY { get; private set; } = 11;


        private BoardStatePool BoardStatePool { get; set; }


        [field: SerializeField] public int Debug { get; private set; } = 0;

        [field: SerializeField] public bool SlideShow { get; private set; }
        float Timer { get; set; }


        public override void DoAwake()
        {
            this.BoardStatePool = new BoardStatePool();
            this.Board = new Board();

            this.Board.Initialize(this.ScaleX, this.ScaleY, this.GameData.PrefabData.Tile);
        }

        //private void CreateBoard()
        //{
        //    BoardState state = new BoardStateGenerator(this.Debug, this.ScaleX, this.ScaleY, this.GameData).Create();
        //    // BoardState state = new BoardStateGenerator(this.Debug, 19, 11, this.GameData).Create();

        //    this.Board.LoadState(state);
        //}

        public override void DoUpdate()
        {
            if (!this.SlideShow)
                return;

            this.Timer += Time.deltaTime;

            if (this.Timer > 3)
            {
                this.Debug++;
                this.Create(this.Debug);
                this.Load(this.Debug);
                this.Timer = 0;
            }
        }


        public BoardState Create(int id)
        {
            BoardState state = new BoardStateGenerator(id, this.ScaleX, this.ScaleY, this.GameData).Create();
            this.BoardStatePool.Add(state);

            return state;
        }

        public BoardState Load(int id)
        {
            if (!this.BoardStatePool.TryGet(id, out BoardState state))
            {
                state = this.Create(id);
            }

            this.Board.LoadState(state);

            return state;
        }

        public BoardState Get(int id)
        {
            if (!this.BoardStatePool.TryGet(id, out BoardState state))
            {
                return null;
            }

            return state;
        }

        public BoardState Expand()
        {
            int directionIndex = this.GameData.GameManager.SessionData.Random.Next(4);
            Direction2D direction = (Direction2D)directionIndex;

            int seed;

            do
            {
                seed = this.GameData.GameManager.SessionData.Random.Next();
            } while (this.BoardStatePool.Contains(seed));

            List<BoardState> validBoardStates = new List<BoardState>();

            foreach (var item in this.BoardStatePool.Pool)
            {
                BoardLink link = item.Value.GetLink(direction);

                if (link == null)
                    continue;

                if (link.Direction == direction && !link.IsLinked)
                {
                    validBoardStates.Add(item.Value);
                    continue;
                }
            }

            BoardState to = this.Create(seed);
            BoardState from = validBoardStates[this.GameData.GameManager.SessionData.Random.Next(validBoardStates.Count)];

            from.Link(direction, to.ID);
            to.Link(direction.Inverse(), from.ID);

            return to;
            //BoardLink fromLink = from.GetLink(direction);
            //fromLink.Link(to.ID);

            //BoardLink toLink = to.GetLink(direction.Inverse());
            //toLink.Link(from.ID);
        }
    }

    public class BoardStatePool
    {
        public Dictionary<int, BoardState> Pool { get; set; } = new Dictionary<int, BoardState>();

        public void Add(BoardState boardState)
        {
            this.Pool[boardState.ID] = boardState;
        }

        public bool TryGet(int id, out BoardState state)
        {
            if (this.Pool.ContainsKey(id))
            {
                state = this.Pool[id];
                return true;
            }

            state = null;
            return false;
        }

        public bool Contains(int seed)
        {
            return this.Pool.ContainsKey(seed);
        }
    }
}