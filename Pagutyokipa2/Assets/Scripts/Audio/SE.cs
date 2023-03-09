using System;
using UnityEngine;

namespace Ryocatusn.Audio
{
    [Serializable]
    public class SE : IEquatable<SE>
    {
        public AudioClip audioClip;
        [Range(-3, 3)]
        public float pitch = 1;
        [Range(0, 1)]
        public float volume = 0.5f;
        public bool loop;
        public bool onlyVisible = true;
        
        [NonSerialized]
        public AudioSource audioSource;

        public bool Equals(SE other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return 
                Equals(pitch, other.pitch) &&
                Equals(volume, other.volume);
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
            return (pitch, volume).GetHashCode();
        }
    }
}
