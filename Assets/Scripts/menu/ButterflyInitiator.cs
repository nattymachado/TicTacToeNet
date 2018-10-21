using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyInitiator : MonoBehaviour {

    public int _numberOfButterflies = 1;
    public GameObject ButterflyTemplate = null;


    void Start()
    {
        for (int i = 0; i < _numberOfButterflies; i++)
        {
            Instantiate(ButterflyTemplate, new Vector3(i * 2.0F, 0, 0), Quaternion.identity);
        }
    }
}
