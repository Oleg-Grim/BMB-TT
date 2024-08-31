using _Project.Scripts.Enums;
using _Project.Scripts.Items;
using _Project.Scripts.Sounds;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Gameplay
{
    public class Cell : MonoBehaviour, IDropHandler
    {
        [SerializeField] private ItemType _shelfType;
        public ItemType Type => _shelfType;
        public Item CurrentItem { get; set; }
        
        private bool _isCorrect = false;
        
        private void OnEnable()
        {
            RackManager.OnFinished += Finish;
        }

        private void OnDisable()
        {
            RackManager.OnFinished -= Finish;
        }

        public void Initialize(Item itemPrefab)
        {
            CurrentItem = Instantiate(itemPrefab, transform);
            CurrentItem.Initialize(this);
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            var droppedItem = eventData.pointerDrag.GetComponent<Item>();
            var droppedCell = droppedItem.CurrentCell;

            if (ReferenceEquals(droppedCell,this))
            {
                droppedItem.Return();
                return;
            }

            if (CurrentItem.Type == _shelfType)
            {
                SoundType.Wrong.Play();
                droppedItem.Return();
                CurrentItem.Shake();
                return;
            }

            if (droppedItem.Type != _shelfType)
            {
                SoundType.Wrong.Play();
                droppedItem.Return();
                CurrentItem.Shake();
                return;
            }

            SoundType.Right.Play();
            
            droppedItem.transform.SetParent(transform, true);
            CurrentItem.transform.SetParent(droppedCell.transform, true);
            
            CurrentItem.CurrentCell = droppedCell;
            droppedItem.CurrentCell = this;

            droppedItem.Return();
            CurrentItem.Return();

            droppedCell.CurrentItem = CurrentItem;
            CurrentItem = droppedItem;

            RackManager.OnChanged?.Invoke();
        }

        public void Return()
        {
            CurrentItem.transform.SetParent(transform, true);
            CurrentItem.Return();
        }

        public bool Validate()
        {
            if (CurrentItem.Type == _shelfType)
            {
                _isCorrect = true;
                CurrentItem.Validate();
            }

            return _isCorrect;
        }
        
        private void Finish()
        {
            _isCorrect = false;
        }
    }
}