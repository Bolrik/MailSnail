using UnityEngine;

namespace MailSnail.GameCamera
{
    [CreateAssetMenu(fileName = "Game Camera Settings", menuName = "Settings/Camera/new Game Camera Settings")]
    public class GameCameraSettings : ScriptableObject
    {
        [field: SerializeField] public float Padding { get; private set; }
        [field: SerializeField] public Vector2 Offset { get; private set; }

        [field: SerializeField] public float CameraSizeMin { get; private set; } = 1f;
    }
}
