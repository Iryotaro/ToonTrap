using UnityEngine;

namespace Ryocatusn
{
    public class WeatherEffects : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem[] effects;

        public void Play()
        {
            foreach (ParticleSystem effect in effects)
            {
                effect.Play();
            }
        }

        public void Stop()
        {
            foreach (ParticleSystem effect in effects)
            {
                effect.Stop();
            }
        }
    }
}
