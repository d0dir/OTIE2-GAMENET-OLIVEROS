using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsConnectedAndReady)
        {
            Respawn.instance.spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
            int index = Random.Range(0, Respawn.instance.spawnPoints.Length);

            PhotonNetwork.Instantiate(playerPrefab.name, Respawn.instance.spawnPoints[index].transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
