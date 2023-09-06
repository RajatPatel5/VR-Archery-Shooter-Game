using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreOnCube : MonoBehaviour
{
    [SerializeField] int itsScore;

    public void UpdateScore()
    {
        ScoreManager.instance.AddScore(itsScore);
    }
}
