using FreschGames.Core.UI;
using MailSnail.Game;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace MailSnail.UI
{
    public partial class MenuScreen
    {
        [System.Serializable]
        public class ButtonPanel : UIComponent
        {
            [field: SerializeField, Header("Names")] public string StartName { get; private set; } = "Start";
            [field: SerializeField] public string SeedName { get; private set; } = "Seed";
            [field: SerializeField] public string QuitName { get; private set; } = "Quit";


            private Button StartElement { get; set; }
            private TextField SeedElement { get; set; }
            private Button QuitElement { get; set; }


            private GameData GameData { get; set; }



            public override void Initialize(UIElement root, VisualElement visualElement)
            {
                this.StartElement = visualElement.Q<Button>(this.StartName);
                this.SeedElement = visualElement.Q<TextField>(this.SeedName);
                this.QuitElement = visualElement.Q<Button>(this.QuitName);

                this.StartElement.clicked += this.StartElement_Clicked;
                this.QuitElement.clicked += this.QuitElement_Clicked;
            }

            public void SetData(GameData gameData)
            {
                this.GameData = gameData;
            }

            private void StartElement_Clicked()
            {
                int seed = 0;
                string seedText = this.SeedElement.text;

                if (string.IsNullOrWhiteSpace(seedText))
                {
                    // Use Random Seed
                    seed = new System.Random().Next(int.MinValue, int.MaxValue);
                }
                // Parse if only numbers
                else if (!int.TryParse(seedText, out seed))
                {
                    // Get Hash for String
                    seed = seedText.GetStableHash();
                }

                Debug.Log($"Start Game With Seed '{seed}'.");

                this.GameData.GameManager.NewGame(seed);
            }

            private void QuitElement_Clicked()
            {
                Debug.Log($"Quit Game");

                Application.Quit();
            }
        }
    }
}