using UnityEngine;

namespace Ryocatusn.Characters
{
    public class PooMaker : MonoBehaviour
    {
        [SerializeField]
        private Poo poo;

        private void OnEnable()
        {
            Make();
        }

        private void Make()
        {
            Instantiate(poo, transform.position, Quaternion.identity);
        }
    }
}
