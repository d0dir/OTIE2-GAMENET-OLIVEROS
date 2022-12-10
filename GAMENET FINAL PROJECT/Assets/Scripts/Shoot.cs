using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Shoot : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Transform target;
    public float force = 20;

    public bool isControlEnabled;

    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        if(isControlEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Kick();
            }
        }
    }

    private void Kick()
    {
        Vector3 Shoot = (target.position - this.transform.position).normalized;
        GetComponent<Rigidbody>().AddForce(Shoot * force, ForceMode.Impulse);
    }
}
