using FreschGames.Core.UI;
using MailSnail.Game;
using MailSnail.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MailSnail.UI
{
    public partial class MenuScreen : UIElement
    {
        [field: SerializeField, Header("Data")] private GameData GameData { get; set; }

        [field: SerializeField, Header("Components")] public ButtonPanel ButtonPanelComponent { get; private set; }

        [field: SerializeField] private AudioManager AudioManager { get; set; }

        [field: SerializeField] private AudioClip Clip { get; set; }

        private void Awake()
        {
            this.ButtonPanelComponent.Initialize(this, this.Document.rootVisualElement);
            this.ButtonPanelComponent.SetData(this.GameData);
        }

        private void Start()
        {
            this.AudioManager.Play(this.Clip);
        }
    }
}