using FreschGames.Core.Managers;
using FreschGames.Core.SceneManagement;
using MailSnail.Board;
using MailSnail.Misc;
using MailSnail.Units;
using System;
using UnityEngine;

namespace MailSnail.Game
{
    [CreateAssetMenu(fileName = "Game Manager", menuName = "Manager/Game/new Game Manager")]
    public class GameManager : ManagerComponent
    {
        [field: SerializeField, Header("Data")] public GameData Data { get; private set; }
        [field: SerializeField, Header("Settings")] public GameSettings Settings { get; private set; }

        [field: SerializeField] public SceneManager SceneManager { get; private set; }

        public SessionData SessionData { get; private set; }
        bool IsLoaded { get; set; }
        Unit Player { get; set; }

        public override void DoStart()
        {
            BoardManager boardManager = this.Data.BoardManager;
            Transform[] targets = new[]
            {
                boardManager.Board.Tiles[0, 0].transform,
                boardManager.Board.Tiles[boardManager.Board.ScaleX - 1, boardManager.Board.ScaleY - 1].transform
            };

            this.Data.GameCamera.Exchange.SetTargets(targets);
        }

        public void NewGame(int seed)
        {
            this.SessionData = new SessionData(seed);
            this.IsLoaded = false;

            this.SceneManager.Load(1);
        }

        public override void DoUpdate()
        {
            if (this.Data.Input.Menu.WasPressed)
            {
                this.SceneManager.Load(0);
            }

            this.Load();
        }

        private void Load()
        {
            if (this.IsLoaded)
                return;

            this.IsLoaded = true;

            int seed;

            if (this.SessionData == null)
                this.SessionData = new SessionData(this.Settings.FallbackSeed);

            seed = this.SessionData.Random.Next();

            BoardManager boardManager = this.Data.BoardManager;

            BoardState state = boardManager.Create(seed);
            state.TileStates[state.FeatureSpot.X, state.FeatureSpot.Y].SetTileContentData(this.Data.TileContentDataPool.Town);
            boardManager.Load(seed);

            Tile[] valid = boardManager.Board.SearchAll((Tile tile) => tile.State.Unit == null && tile.State.TileContentData == null);

            int index = this.SessionData?.Random.Next(valid.Length) ?? 0;
            Tile spawnPoint = valid[index];

            this.Player = GameObject.Instantiate(this.Data.PrefabData.Player);
            this.Player.Initialize();
            this.Player.ChangeBoardState(state);
            this.Player.MoveTo(spawnPoint.X, spawnPoint.Y);

            //this.Player = GameObject.Instantiate(this.Data.PrefabData.Player);
            //this.Player.Initialize();
            //this.Player.ChangeBoardState(state);
            //this.Player.MoveTo(spawnPoint.X + 1, spawnPoint.Y);

            this.Expand();
        }

        public void Expand()
        {
            BoardState expand = this.Data.BoardManager.Expand();
            expand.TileStates[expand.FeatureSpot.X, expand.FeatureSpot.Y].SetItem(this.Data.ItemPool.Package);
        }

        public void ActivateLink(BoardLink link)
        {
            BoardState state = this.Data.BoardManager.Load(link.BoardID);
            BoardLink spawn = state.GetLink(link.Direction.Inverse());
            Int2D spawnPoint = spawn.Direction.OffsetDirection(spawn.Positions[1], true);

            this.Player.ChangeBoardState(state);
            this.Player.MoveTo(spawnPoint.X, spawnPoint.Y);
        }
    }

    public class SessionData
    {
        public int Seed { get; private set; }
        public System.Random Random { get; private set; }

        public SessionData(int seed)
        {
            this.Seed = seed;
            this.Random = new System.Random(this.Seed);
        }
    }
}