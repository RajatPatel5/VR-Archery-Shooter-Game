using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yudiz.VRArchery.CoreGameplay
{
    public class SpaceShip : MonoBehaviour
    {                       
        void Update()
        {
            transform.position += (Vector3.forward) * 1f* Time.deltaTime;
        }
    }
}
