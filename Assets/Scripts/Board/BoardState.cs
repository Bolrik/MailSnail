using MailSnail.Misc;
using MailSnail.Units;
using System;
using System.Collections.Generic;

namespace MailSnail.Board
{
    public class BoardState
    {
        public int ID { get; set; }

        public TileState[,] TileStates { get; set; }
        public BoardLink[] Links { get; private set; }
        public Int2D FeatureSpot { get; private set; }

        public int ScaleX { get; private set; }
        public int ScaleY { get; private set; }


        public BoardState(int id, TileState[,] tileStates, BoardLink[] links, Int2D featureSpot)
        {
            this.ID = id;

            this.FeatureSpot = featureSpot;
            this.TileStates = tileStates;
            this.Links = links;

            this.ScaleX = this.TileStates.GetLength(0);
            this.ScaleY = this.TileStates.GetLength(1);
        }

        public BoardLink GetLink(Direction2D direction)
        {
            for (int i = 0; i < this.Links.Length; i++)
            {
                if (this.Links[i].Direction == direction)
                    return this.Links[i];
            }

            return null;
        }

        public void Link(Direction2D direction, int boardID)
        {
            BoardLink link = this.GetLink(direction);
            link.Link(boardID);

            foreach (var removeContent in link.Positions)
            {
                this.TileStates[removeContent.X, removeContent.Y].SetTileContentData(null);
            }
        }



        public void SetUnit(int x, int y, Unit unit)
        {
            if (!this.IsValid(x, y, out TileState tile))
                return;

            tile.Unit = unit;
        }

        public TileState Get(int x, int y)
        {
            if (!this.IsValid(x, y))
                return null;

            return this.GetTile(x, y);
        }

        private TileState GetTile(int x, int y)
        {
            return this.TileStates[x, y];
        }

        public TileState[] SearchAll(Predicate<TileState> check)
        {
            List<TileState> valid = new List<TileState>();

            for (int y = 0; y < this.ScaleY; y++)
            {
                for (int x = 0; x < this.ScaleX; x++)
                {
                    TileState tile = this.TileStates[x, y];

                    if (check(tile))
                        valid.Add(tile);
                }
            }

            return valid.ToArray();
        }

        public bool IsValid(int x, int y, out TileState tileAt)
        {
            tileAt = null;

            if (x < 0 || y < 0 || x >= this.ScaleX || y >= this.ScaleY)
                return false;

            tileAt = this.GetTile(x, y);

            return true;
        }

        public bool IsValid(int x, int y)
        {
            return this.IsValid(x, y, out _);
        }
    }
}