using UnityEngine;
using DG.Tweening;

namespace Ryocatusn
{
    public class BreakJankenRoad : MonoBehaviour
    {
        private void Start()
        {
            foreach (Rigidbody2D rigid in GetComponentsInChildren<Rigidbody2D>())
            {
                rigid.AddForce((rigid.transform.position - transform.position) * 100);

                Sequence sequence = DOTween.Sequence();
                sequence
                    .SetLink(gameObject)
                    .Append(rigid.transform.DOScale(Vector3.zero, 0.8f))
                    .OnComplete(() => Destroy(gameObject));
            }
        }
    }
}
