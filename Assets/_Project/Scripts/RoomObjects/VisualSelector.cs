using _Project.Scripts.Configs;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.RoomObjects
{
    public class VisualSelector : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        private MaterialPropertyBlock _propertyBlock;
        private static readonly int HatchColor = Shader.PropertyToID("_HatchColor");
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        private Color _canInteractColor;
        private Color _defaultColor;
        private Color _magnetColor;
        private Color _blockColor;
        private float _savedHatchTransparency;
        private float _holdTransparency;

        [Inject]
        private void Construct(GameplayConfig gameplayConfig)
        {
            _canInteractColor = gameplayConfig.CanInteractColor;
            _magnetColor = gameplayConfig.MagnetColor;
            _blockColor = gameplayConfig.BlockColor;
            _holdTransparency = gameplayConfig.HoldTransparency;
        }

        private void Start()
        {
            _savedHatchTransparency = _canInteractColor.a;
            _propertyBlock = new MaterialPropertyBlock();
            _defaultColor = _meshRenderer.material.color;
        }

        public void SetCanInteractVisibility(bool isVisible)
        {
            if (!_meshRenderer || _meshRenderer.materials.Length <= 1)
            {
                return;
            }

            _meshRenderer.GetPropertyBlock(_propertyBlock, 1);
            _canInteractColor.a = isVisible ? _savedHatchTransparency : 0f;
            _propertyBlock.SetColor(HatchColor, _canInteractColor);
            _meshRenderer.SetPropertyBlock(_propertyBlock, 1);
        }

        public void SetTransparencyOnPickUp()
        {
            if (!_meshRenderer)
            {
                return;
            }

            _meshRenderer.GetPropertyBlock(_propertyBlock, 0);
            Color color = _defaultColor;
            color.a = _holdTransparency;
            _propertyBlock.SetColor(BaseColor, color);
            _meshRenderer.SetPropertyBlock(_propertyBlock, 0);
        }

        public void SetObjectMagnetColor()
        {
            SetObjectColor(_magnetColor);
        }
        
        public void SetObjectBlockColor()
        {
            SetObjectColor(_blockColor);
        }
        
        public void ResetObjectColor()
        {
            if (!_meshRenderer)
            {
                return;
            }

            _propertyBlock.Clear();

            Color color = _defaultColor;
            color.a = 1f;
            _propertyBlock.SetColor(BaseColor, color);
            _meshRenderer.SetPropertyBlock(_propertyBlock, 0);
        }
        
        private void SetObjectColor(Color color)
        {
            if (!_meshRenderer)
            {
                return;
            }

            _meshRenderer.GetPropertyBlock(_propertyBlock, 0);
            _propertyBlock.SetColor(BaseColor, color);
            _meshRenderer.SetPropertyBlock(_propertyBlock, 0);
        }
    }
}