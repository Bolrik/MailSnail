using MailSnail.Items;
using MailSnail.Units;
using System;

namespace MailSnail.Board
{
    public class TileState
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public TileData TileData { get; private set; }
        public TileContentData TileContentData { get; private set; }
        public ItemData Item { get; private set; }
        public Unit Unit { get; set; }

        public int Accessibility { get; set; }

        public Action OnTileContentDataChanged { get; set; }

        public TileState(int x, int y, TileData tileData, TileContentData tileContentData)
        {
            this.X = x;
            this.Y = y;

            this.TileData = tileData;
            this.TileContentData = tileContentData;
        }

        public void SetTileContentData(TileContentData tileContentData)
        {
            this.TileContentData = tileContentData;
            this.OnTileContentDataChanged?.Invoke();
        }

        public void SetItem(ItemData item)
        {
            this.Item = item;
            this.OnTileContentDataChanged?.Invoke();
        }
    }
}