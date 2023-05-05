using FreschGames.Core.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MailSnail.GameCamera
{
    [CreateAssetMenu(fileName = "Game Camera Manager", menuName = "Managers/Camera/new Game Camera Manager")]
    public class GameCameraManager : Manager
    {
        [field: SerializeField, Header("Settings")] private GameCameraSettings Settings { get; set; }
        [field: SerializeField, Header("Exchange")] public GameCameraExchange Exchange { get; set; }

        private float CameraSizeMin => this.Settings.CameraSizeMin;

        // Access Data
        private UnityEngine.Camera Camera { get => this.Exchange.Camera; }
        private Transform[] Targets { get => this.Exchange.Targets; }



        public override void SystemAwake()
        {
            this.Exchange = new GameCameraExchange();
        }

        public override void SystemLateUpdate()
        {
            if (this.Targets.Length == 0) return;

            Vector3 center = this.GetCenterPoint();
            float size = this.GetRequiredSize(center);
            this.Camera.transform.position = new Vector3(center.x + this.Settings.Offset.x, center.y + this.Settings.Offset.y, this.Camera.transform.position.z);
            this.Camera.orthographicSize = Mathf.Max(size, this.CameraSizeMin);
        }


        private Vector3 GetCenterPoint()
        {
            if (this.Targets.Length == 1)
                return this.Targets[0].position;

            Bounds bounds = new Bounds(this.Targets[0].position, Vector3.zero);

            for (int i = 0; i < this.Targets.Length; i++)
            {
                bounds.Encapsulate(this.Targets[i].position);
            }

            return bounds.center;
        }

        private float GetRequiredSize(Vector3 center)
        {
            float size = 0f;

            Vector3 centerLocalPos = this.Camera.transform.InverseTransformPoint(center);

            for (int i = 0; i < this.Targets.Length; i++)
            {
                Vector3 targetLocalPos = this.Camera.transform.InverseTransformPoint(this.Targets[i].position);
                Vector3 distanceToTarget = targetLocalPos - centerLocalPos;
                size = Mathf.Max(size, Mathf.Abs(distanceToTarget.y));
                size = Mathf.Max(size, Mathf.Abs(distanceToTarget.x) / this.Camera.aspect);
            }

            size += this.Settings.Padding;
            return size;
        }
    }
}
