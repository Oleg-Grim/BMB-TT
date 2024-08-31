using System;
using System.Collections.Generic;
using _Project.Scripts.Enums;
using UnityEngine;

namespace _Project.Scripts.Sounds
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundSO[] _sounds;

        private Dictionary<SoundType, AudioClip> _soundRegistry;

        private AudioSource _source;
        
        public static Action<SoundType> OnPlaySound { get; private set; }

        private void OnEnable()
        {
            OnPlaySound += PlaySound;
        }

        private void OnDisable()
        {
            OnPlaySound -= PlaySound;
        }

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            
            CacheSounds();
        }

        private void CacheSounds()
        {
            _soundRegistry = new Dictionary<SoundType, AudioClip>(_sounds.Length);
                
            foreach (var so in _sounds)
            {
                _soundRegistry.Add(so.Type, so.Clip);
            }
        }

        private void PlaySound(SoundType type)
        {
            if (!_soundRegistry.TryGetValue(type, out var clip))
            {
                Debug.LogError($"[Sound Manager] No sound of type {type} found!");
                return;
            }
                
            _source.clip = clip;
            _source.Play();
        }
    }
}