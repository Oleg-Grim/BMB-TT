using _Project.Scripts.Enums;
using UnityEngine;

namespace _Project.Scripts.Sounds
{
    [CreateAssetMenu(menuName = "Gameplay/Create SoundSO", fileName = "SoundSO", order = 0)]
    public class SoundSO : ScriptableObject
    {
        public SoundType Type;
        public AudioClip Clip;
    }
}