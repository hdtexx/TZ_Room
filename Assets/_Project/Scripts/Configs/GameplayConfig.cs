using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "GameplayConfig", menuName = "GameConfigs/GameplayConfig")]
    public class GameplayConfig : ScriptableObject
    {
        [field: Header("Gameplay settings:")]
        [field: SerializeField] public float PlayerSpeed { get; private set; } = 5f;
        [field: SerializeField] public float MouseSensitivity { get; private set; } = 400f;
        [field: SerializeField] public float MinTopViewAngle { get; private set; } = -70f;
        [field: SerializeField] public float MaxTopViewAngle { get; private set; } = 70f;
        [field: SerializeField] public float InteractionDistance { get; private set; } = 2f;
        [field: SerializeField] public float HoldingDistance { get; private set; } = 2.5f;
        [field: SerializeField] public float MaxMagnetDistance { get; private set; } = 4f;
        [field: SerializeField] public float MagnetHorizontalSurfaceOffset { get; private set; } = 0.01f;
        [field: SerializeField] public float MagnetVerticalSurfaceOffset { get; private set; } = 0.03f;
        [field: SerializeField] public float RotationAngle { get; private set; } = 45f;
        [field: SerializeField] public int MaxOverlapColliders { get; private set; } = 10;
        
        [field: Space(10)]
        [field: Header("Color settings:")]
        [field: SerializeField] public Color CanInteractColor { get; private set; } = Color.gray;
        [field: SerializeField] public Color MagnetColor { get; private set; } = Color.green;
        [field: SerializeField] public Color BlockColor { get; private set; } = Color.red;
        [field: SerializeField] public float HoldTransparency { get; private set; } = 0.4f;
    }
}