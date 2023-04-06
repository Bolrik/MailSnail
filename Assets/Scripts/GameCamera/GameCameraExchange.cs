using UnityEngine;

namespace MailSnail.GameCamera
{
    [System.Serializable]
    public class GameCameraExchange
    {
        public UnityEngine.Camera Camera { get; private set; }
        public Transform[] Targets { get; private set; } = new Transform[0];

        public void SetCamera(UnityEngine.Camera camera)
        {
            this.Camera = camera;
        }

        public void SetTargets(Transform[] targets)
        {
            this.Targets = targets ?? new Transform[0];
        }
    }
}
