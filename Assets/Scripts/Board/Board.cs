using MailSnail.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MailSnail.Board
{
    public class Board
    {
        public Transform TileHost { get; private set; }
        public Tile[,] Tiles { get; set; }

        public int ScaleX { get; private set; }
        public int ScaleY { get; private set; }

        public BoardState State { get; private set; }


        public void Initialize(int scaleX, int scaleY, Tile prefab)
        {
            this.TileHost = this.TileHost ?? new GameObject("TileHost").transform;

            this.ScaleX = scaleX;
            this.ScaleY = scaleY;

            this.Tiles = new Tile[this.ScaleX, this.ScaleY];

            for (int y = 0; y < this.ScaleY; y++)
            {
                for (int x = 0; x < this.ScaleX; x++)
                {
                    Tile tile;
                    this.Tiles[x, y] = tile = GameObject.Instantiate(prefab);
                    tile.transform.SetParent(this.TileHost, false);
                    tile.SetPosition(x, y);
                }
            }
        }

        public void LoadState(BoardState boardState)
        {
            this.State = boardState;

            for (int y = 0; y < this.ScaleY; y++)
            {
                for (int x = 0; x < this.ScaleX; x++)
                {
                    this.Tiles[x, y].SetState(this.State.TileStates[x, y]);
                }
            }
        }

        public void SetUnit(int x, int y, Unit unit)
        {
            if (!this.IsValid(x, y, out Tile tile))
                return;

            tile.State.Unit = unit;
        }

        public Tile GetTileAt(int x, int y)
        {
            if (!this.IsValid(x, y))
                return null;

            return this.GetTile(x, y);
        }

        private Tile GetTile(int x, int y)
        {
            return this.Tiles[x, y];
        }

        public Tile[] SearchAll(Predicate<Tile> check)
        {
            List<Tile> valid = new List<Tile>();

            for (int y = 0; y < this.ScaleY; y++)
            {
                for (int x = 0; x < this.ScaleX; x++)
                {
                    Tile tile = this.Tiles[x, y];
                    if (check(tile))
                        valid.Add(tile);
                }
            }

            return valid.ToArray();
        }

        public bool IsValid(int x, int y, out Tile tileAt)
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