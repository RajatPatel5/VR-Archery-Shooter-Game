using UnityEngine;
namespace Yudiz.VRArchery.CoreGameplay
{
    public class Lantern : MonoBehaviour
    {
        public bool activated;
        public ParticleSystem hitParticle;
        public Transform flameQuad;
        public void TurnOn()
        {
            if (activated)
                return;
            activated = true;

            hitParticle.Play();
            flameQuad.gameObject.SetActive(true);
        }

    }
}
