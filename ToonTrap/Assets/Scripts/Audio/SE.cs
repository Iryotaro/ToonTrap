using System;
using UnityEngine;

namespace Ryocatusn.Audio
{
    [Serializable]
    public class SE : IEquatable<SE>
    {
        [SerializeField]
        private AudioClip audioClip;
        [SerializeField]
        [Range(-3, 3)]
        private float pitch = 1;
        [SerializeField]
        [Range(0, 1)]
        private float volume = 0.5f;
        [SerializeField]
        private bool loop;
        [SerializeField]
        private bool onlyVisible = true;

        public AudioClip AudioClip => audioClip;
        public float Pitch => pitch;
        public float Volume => volume;
        public bool Loop => loop;
        public bool OnlyVisible => onlyVisible;

        [NonSerialized]
        public AudioSource audioSource;

        public SE ChangePitch(float pitch)
        {
            return new SE(audioClip, pitch, volume, loop, onlyVisible);
        }
        public SE ChangeVolume(float volume)
        {
            return new SE(audioClip, pitch, volume, loop, onlyVisible);
        }

        public SE(AudioClip audioClip, float pitch, float volume, bool loop, bool onlyVisible)
        {
            this.audioClip = audioClip;
            this.pitch = pitch;
            this.volume = volume;
            this.loop = loop;
            this.onlyVisible = onlyVisible;
        }

        public bool Equals(SE other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return
                Equals(audioClip, other.audioClip) &&
                Equals(pitch, other.pitch) &&
                Equals(volume, other.volume) &&
                Equals(loop, other.loop) &&
                Equals(onlyVisible, other.onlyVisible);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(obj, this)) return true;
            if (GetType() != obj.GetType()) return false;
            return Equals((SE)obj);
        }
        public override int GetHashCode()
        {
            return (audioClip, pitch, volume, loop, onlyVisible).GetHashCode();
        }
    }
}
