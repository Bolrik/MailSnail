using FreschGames.Core.UI;
using MailSnail.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MailSnail.UI
{
    public partial class GameScreen : UIElement
    {
        [field: SerializeField, Header("Data")] private GameData GameData { get; set; }

        [field: SerializeField, Header("Components")] public InventoryPanel InventoryPanelComponent { get; private set; }

        private void Awake()
        {
            this.InventoryPanelComponent.Initialize(this, this.Document.rootVisualElement);
            this.InventoryPanelComponent.SetData(this.GameData);
        }
    }
}