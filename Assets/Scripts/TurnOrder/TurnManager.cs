using FreschGames.Core.Managers;
using MailSnail.Board;
using MailSnail.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MailSnail.TurnOrder
{
    [CreateAssetMenu(fileName = "Turn Manager", menuName = "Manager/Turn Order/new Turn Manager")]
    public class TurnManager : ManagerComponent
    {
        List<Unit> Units { get; set; }
        Queue<Unit> Queue { get; set; }

        public TurnToken CurrentToken { get; private set; }

        public Action<TurnToken> OnNewToken { get; set; }

        public override void DoAwake()
        {
            this.Units = new List<Unit>();
            this.Queue = new Queue<Unit>();

            this.CurrentToken = null;
        }

        public override void DoUpdate()
        {
            if (this.CurrentToken == null)
            {
                this.CreateToken();
                return;
            }

            if (!this.CurrentToken.IsDone)
                return;

            this.CreateToken();
            // Turn is Done, Play Animations etc...
        }

        public void Add(Unit unit)
        {
            this.Units.Add(unit);

            Debug.Log($"Units in Turn Manager: {this.Units.Count}");
        }

        public void Remove(Unit unit)
        {
            this.Units.Remove(unit);
        }

        private void CreateToken()
        {
            if (this.Units.Count <= 0)
                return;

            if (this.Queue.Count == 0)
            {
                this.CreateTurnList();
            }

            Unit next = this.Queue.Dequeue();

            this.SetToken(next);
        }

        private void SetToken(Unit next)
        {
            this.CurrentToken = new TurnToken(next);
            this.OnNewToken?.Invoke(this.CurrentToken);
        }

        private void CreateTurnList()
        {
            this.Queue = new Queue<Unit>(this.Units);
        }
    }
}