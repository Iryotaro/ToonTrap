using UnityEngine;

namespace Ryocatusn.Lights
{
    [RequireComponent(typeof(SpotLight))]
    public class HpLight : MonoBehaviour
    {
        private SpotLight spotLight;

        [SerializeField]
        private Transform extraPosition;

        private void Start()
        {
            spotLight = GetComponent<SpotLight>();
        }
        private void Update()
        {
            spotLight.TurnOnExtra(extraPosition.position);
        }
    }
}
