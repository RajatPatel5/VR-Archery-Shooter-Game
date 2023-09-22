using UnityEngine;
using Yudiz.VRArchery.Managers;

namespace Yudiz.VRArchery.CoreGameplay
{

    public class ScoreOnCube : MonoBehaviour
    {
        [SerializeField] int itsScore;

        public void UpdateScore()
        {
            ScoreManager.instance.AddScore(itsScore);
        }
    }
}
