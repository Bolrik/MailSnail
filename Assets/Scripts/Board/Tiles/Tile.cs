using TMPro;
using UnityEngine;

namespace MailSnail.Board
{
    public class Tile : MonoBehaviour
    {
        [field: SerializeField, Header("Visuals")] public SpriteRenderer SpriteRenderer { get; private set; }
        [field: SerializeField] public SpriteRenderer ContentRenderer { get; private set; }

        [field: SerializeField, Header("TileInfo")] public TMP_Text TileInfoText { get; private set; }

        [field: SerializeField, Header("Settings")] public TileSettings TileSettings { get; private set; }

        public TileState State { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        public void SetPosition(int x, int y)
        {
            this.X = x;
            this.Y = y;

            this.SpriteRenderer.color = (this.X + this.Y) % 2 == 0 ? Color.white : new Color(.95f, .95f, .95f);

            this.transform.localPosition = new Vector3(this.X, this.Y, 0);
        }

        public void SetState(TileState tileState)
        {
            if (this.State != null)
            {
                this.State.OnTileContentDataChanged -= this.State_OnTileContentDataChanged;
            }

            this.State = tileState;

            if (this.State != null)
            {
                this.State.OnTileContentDataChanged += this.State_OnTileContentDataChanged;
            }

            this.LoadState();
        }

        private void LoadState()
        {
            if (this.State == null)
                return;

            if (this.State.TileData != null)
            {
                this.SpriteRenderer.sprite = this.State.TileData.Sprite;
            }
            else
            {
                this.SpriteRenderer.sprite = null;
            }

            if (this.State.Item != null)
            {
                this.ContentRenderer.sprite = this.State.Item.Sprite;
                this.ContentRenderer.transform.localPosition = Vector3.zero;
            }
            else if (this.State.TileContentData != null)
            {
                this.ContentRenderer.sprite = this.State.TileContentData.Sprite;
                this.ContentRenderer.transform.localPosition = this.State.TileContentData.Offset;
            }
            else
            {
                this.ContentRenderer.sprite = null;
            }

            this.TileInfoText.enabled = this.TileSettings.ShowTileInfo;
            this.TileInfoText.text = "" + this.State.Accessibility;
        }

        private void Update()
        {
            this.LoadState();
        }

        private void State_OnTileContentDataChanged()
        {
            this.LoadState();
        }
    }
}