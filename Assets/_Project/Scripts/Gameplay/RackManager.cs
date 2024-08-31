using System;
using System.Collections.Generic;
using _Project.Scripts.Enums;
using _Project.Scripts.Items;
using _Project.Scripts.Sounds;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay
{
    public class RackManager : MonoBehaviour
    {
        [SerializeField] private List<Cell> _cells;
        [SerializeField] private Item _itemPrefab;

        public static Action OnRestart;
        public static Action OnChanged;
        public static Action OnFinished;

        private void OnEnable()
        {
            OnRestart += StartGame;
            OnChanged += Validate;
        }

        private void OnDisable()
        {
            OnRestart -= StartGame;
            OnChanged -= Validate;
        }

        private void Start()
        {
            foreach (var cell in _cells)
            {
                cell.Initialize(_itemPrefab);
            }
            
            StartGame();
        }

        private void StartGame()
        {
            Invoke(nameof(Shuffle), 1f);
        }

        private void Shuffle()
        {
            for (int i = 0; i < _cells.Count - 1; i++)
            {
                var rnd = Random.Range(i, _cells.Count);
                (_cells[i].CurrentItem, _cells[rnd].CurrentItem) = (_cells[rnd].CurrentItem, _cells[i].CurrentItem);
                (_cells[i].CurrentItem.CurrentCell, _cells[rnd].CurrentItem.CurrentCell) 
                    = (_cells[rnd].CurrentItem.CurrentCell, _cells[i].CurrentItem.CurrentCell);
                
                _cells[i].Return();
            }

            _cells[^1].Return();
            
            Invoke(nameof(Validate), 0f);
        }

        private void Validate()
        {
            var correctCounter = 0;
            foreach (var cell in _cells)
            {
                if (cell.Validate())
                {
                    correctCounter++;
                }
            }

            if (correctCounter == _cells.Count)
            {
                SoundType.Win.Play();
                OnFinished?.Invoke();
            }
        }
    }
}