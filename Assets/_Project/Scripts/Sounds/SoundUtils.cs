using _Project.Scripts.Enums;

namespace _Project.Scripts.Sounds
{
    public static class SoundUtils
    {
        public static void Play(this SoundType type)
        {
            SoundManager.OnPlaySound?.Invoke(type);
        }
    }
}