using _Project.Scripts.Enums;
using _Project.Scripts.Gameplay;
using _Project.Scripts.Sounds;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.Items
{
    public class Item : MonoBehaviour, IBeginDragHandler, IEndDragHandler,IDragHandler
    {
        [SerializeField] private Image _itemImage;
        [SerializeField] private Image _shadowImage;
        [SerializeField] private ItemSO[] _configs;
        
        [SerializeField] private float _smoothness = 15f;
        
        private Vector3 _targetRotation = new Vector3(0f, 0f, 15f);
        private Sequence _rotationSequence;
        
        private Canvas _canvas;
        
        private Vector3 _dragPosition = Vector3.zero;
        private bool _isDragging = false;

        public ItemType Type { get; set; }
        public Cell CurrentCell { get; set; }

        private bool _isCorrect = false;

        private void OnEnable()
        {
            RackManager.OnFinished += Finish;
            RackManager.OnRestart += Restart;
        }

        private void OnDisable()
        {
            RackManager.OnFinished -= Finish;
            RackManager.OnRestart -= Restart;
        }

        public void Initialize(Cell cell)
        {
            _canvas = GetComponent<Canvas>();
            
            Type = cell.Type;
            CurrentCell = cell;
            
            var config = GetConfigByType(Type);
            _itemImage.sprite = config.ItemImage;
            _shadowImage.sprite = config.ShadowImage;

            Return();
            
            _rotationSequence = DOTween.Sequence().SetAutoKill(false).Pause();
            _rotationSequence.Append(_itemImage.transform.DOLocalRotate(_targetRotation, 0.25f));
            _rotationSequence.Append(_itemImage.transform.DOLocalRotate(-_targetRotation, 0.25f));
            _rotationSequence.Append(_itemImage.transform.DOLocalRotate(Vector3.zero, 0.25f));
            _rotationSequence.OnComplete(() => _rotationSequence.Rewind());
        }

        private void Update()
        {
            if (!_isDragging)
            {
                return;
            }

            transform.position = Vector3.Lerp(transform.position, _dragPosition, _smoothness * Time.deltaTime);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            SoundType.Take.Play();
            
            _isDragging = true;
            _shadowImage.enabled = false;
            _itemImage.raycastTarget = false;
            _canvas.sortingOrder = 2;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)transform, eventData.position, Camera.main, out _dragPosition);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            _shadowImage.enabled = true;
            _itemImage.raycastTarget = !_isCorrect;
        }

        public void Return()
        {
            transform.DOLocalMove(Vector2.zero, 0.33f).OnComplete(()=> _canvas.sortingOrder = 1);
        }

        private ItemSO GetConfigByType(ItemType type)
        {
            foreach (var config in _configs)
            {
                if (config.Type != type)
                {
                    continue;
                }

                return config;
            }
            
            Debug.LogError($"[Item] No config of type {type} found!");
            return null;
        }

        public void Validate()
        {
            _isCorrect = true;
            _itemImage.raycastTarget = false;
        }

        private void Finish()
        {
            _itemImage.DOFade(0f, 2f);
            _isCorrect = false;
        }
        
        private void Restart()
        {
            _itemImage.DOFade(1f, 0.5f);
            _itemImage.raycastTarget = true;
        }

        public void Shake()
        {
            _rotationSequence.Restart();
        }
    }
}