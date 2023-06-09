using DG.Tweening;
using UnityEngine;

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
