using FreschGames.Core.UI;
using MailSnail.Game;
using MailSnail.Items;
using MailSnail.TurnOrder;
using MailSnail.Units;
using UnityEngine;
using UnityEngine.UIElements;

namespace MailSnail.UI
{
    public partial class GameScreen
    {
        [System.Serializable]
        public class InventoryPanel : UIComponent
        {
            [field: SerializeField, Header("Names")] public string InventorySlotName { get; private set; } = "Inventory-Slot";

            private Button InventorySlotElement { get; set; }

            private GameData GameData { get; set; }

            UnitController PlayerController { get; set; }
            Unit PlayerUnit { get; set; }

            public override void Initialize(UIElement root, VisualElement visualElement)
            {
                this.InventorySlotElement = visualElement.Q<Button>(this.InventorySlotName);

                this.InventorySlotElement.clicked += this.InventorySlotElement_Clicked;
            }

            public void SetData(GameData gameData)
            {
                this.GameData = gameData;

                this.GameData.UnitControllerManager.OnNewUnitController += UnitControllerManager_OnNewUnitController;

                //this.PlayerController = this.GameData.UnitControllerManager.Get(0);
                //this.PlayerController.OnTurnTokenSet += PlayerController_OnTurnTokenSet;
            }

            private void UnitControllerManager_OnNewUnitController(UnitController unitController)
            {
                if (unitController.Faction == 0)
                {
                    this.PlayerController = unitController;
                    this.PlayerController.OnTurnTokenSet += PlayerController_OnTurnTokenSet;

                    this.SetUnit();
                }
            }

            private void PlayerController_OnTurnTokenSet(TurnToken turnToken)
            {
                this.SetUnit();
            }

            private void SetUnit()
            {
                if (this.PlayerController == null)
                    return;

                if (this.PlayerUnit != null)
                {
                    this.PlayerUnit.ItemSlot.OnItemChanged -= PlayerUnit_ItemSlot_OnItemChanged;
                }

                this.PlayerUnit = this.PlayerController.Unit;

                if (this.PlayerUnit != null)
                {
                    this.PlayerUnit.ItemSlot.OnItemChanged += PlayerUnit_ItemSlot_OnItemChanged;
                }
            }

            private void PlayerUnit_ItemSlot_OnItemChanged(ItemData itemData)
            {
                Sprite sprite = null;

                if (itemData != null)
                {
                    sprite = itemData.Sprite;
                }

                this.InventorySlotElement.style.backgroundImage = new StyleBackground(sprite);
            }

            private void InventorySlotElement_Clicked()
            {
                if (this.PlayerUnit == null)
                    return;

                if (this.PlayerUnit.ItemSlot.Item == null)
                    return;

                this.PlayerUnit.DropItem();
            }
        }
    }
}