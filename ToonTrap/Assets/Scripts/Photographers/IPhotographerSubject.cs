using UnityEngine;

namespace Ryocatusn.Photographers
{
    public interface IPhotographerSubject
    {
        public int priority { get; }
        public int photographerCameraSize { get; }
        public GameObject gameObject { get; }
        public Vector3 GetPosition();
    }
}
