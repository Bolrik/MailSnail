using MailSnail.Board;
using System;

namespace MailSnail.Items
{
    public class ItemSlot
    {
        public ItemData Item { get; private set; }
        
        public Action<ItemData> OnItemChanged { get; set; }

        public void Take(TileState to)
        {
            ItemData thisItem = this.Item;
            ItemData toItem = to.Item;
            // this.Item = to.Item;
            to.SetItem(thisItem);
            this.Set(toItem);
        }

        public void Set(ItemData itemData)
        {
            this.Item = itemData;
            this.OnItemChanged?.Invoke(this.Item);
        }
    }
}
