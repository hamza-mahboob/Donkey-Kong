using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject barrel;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnBarrel", 2, Random.Range(3, 6));
    }

    public void SpawnBarrel()
    {
        Instantiate(barrel, transform.position, Quaternion.identity);
    }

}
