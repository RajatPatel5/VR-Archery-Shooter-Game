using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yudiz.VRArchery.CoreGameplay
{
    public class SpaceShip : MonoBehaviour
    {                       
        void Update()
        {
            transform.position += 1f * Time.deltaTime * Vector3.forward;
        }
    }
}
