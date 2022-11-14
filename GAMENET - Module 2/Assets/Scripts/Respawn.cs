using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public static Respawn instance;

    public GameObject[] spawnPoints;

    void Awake()
    {
        instance = this;
    }
}
