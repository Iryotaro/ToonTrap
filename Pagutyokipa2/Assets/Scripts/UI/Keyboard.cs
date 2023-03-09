using UnityEngine;
using DG.Tweening;

namespace Ryocatusn.UI
{
    public class Keyboard : MonoBehaviour
    {
        private void Start()
        {
            transform.DOShakePosition(1, 20).SetLoops(-1).SetLink(gameObject);
        }
    }
}
