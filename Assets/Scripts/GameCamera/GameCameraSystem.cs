using FreschGames.Core.Managers;
using System.Linq;
using UnityEngine;

namespace MailSnail.GameCamera
{
    public class GameCameraSystem : ManagerSystem<GameCameraManager>
    {
        [field: SerializeField, Header("Access Data")] public UnityEngine.Camera Camera { get; private set; }

        [field: SerializeField] private Transform[] Targets { get; set; } = new Transform[0];

        
        protected override void PostAwake()
        {
            this.Manager.Exchange.SetCamera(this.Camera);
        }

        protected override void PostUpdate()
        {
            if (this.Targets.Length > 0)
            {
                this.Manager.Exchange.SetTargets(this.Manager.Exchange.Targets.Concat(this.Targets).ToArray());
                this.Targets = new Transform[0];
            }
        }
    }
}
