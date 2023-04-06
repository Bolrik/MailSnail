using FreschGames.Core.Managers;
using MailSnail.Game;
using MailSnail.TurnOrder;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MailSnail.Units
{
    [CreateAssetMenu(fileName = "Unit Controller Manager", menuName = "Manager/Unit/new Unit Controller Manager")]
    public class UnitControllerManager : ManagerComponent
    {
        [field: SerializeField, Header("Data")] public GameData GameData { get; private set; }


        private List<UnitController> UnitControllers { get; set; }

        private UnitController ActiveController { get; set; }
        private TurnToken ActiveTurnToken { get; set; }

        public Action<UnitController> OnNewUnitController { get; set; }

        public override void DoAwake()
        {
            this.ActiveTurnToken = null;
            this.UnitControllers = new List<UnitController>();
        }

        public override void DoStart()
        {
            this.GameData.TurnManager.OnNewToken += this.TurnManager_OnNewToken;
        }

        public override void DoUpdate()
        {
            //TurnToken current = this.GameData.TurnManager.CurrentToken;

            //if (this.ActiveTurnToken != current)
            //{
            //    // Changed?
            //}
        }

        public override void DoLateUpdate()
        {

        }

        public UnitController Get(int faction)
        {
            foreach (var unitController in this.UnitControllers)
            {
                if (unitController.Faction == faction)
                    return unitController;
            }

            return null;
        }

        public void Add(UnitController unitController)
        {
            this.UnitControllers.Add(unitController);

            this.OnNewUnitController?.Invoke(unitController);
        }

        private void TurnManager_OnNewToken(TurnToken turnToken)
        {
            if (this.ActiveTurnToken != null)
            {
                this.ActiveController.ClearToken();
                this.ActiveController = null;
            }

            this.ActiveTurnToken = turnToken;

            foreach (var controller in this.UnitControllers)
            {
                if (controller.Faction == this.ActiveTurnToken.Unit.Faction)
                {
                    this.ActiveController = controller;
                    this.ActiveController.SetToken(this.ActiveTurnToken);
                    return;
                }
            }
        }
    }

}