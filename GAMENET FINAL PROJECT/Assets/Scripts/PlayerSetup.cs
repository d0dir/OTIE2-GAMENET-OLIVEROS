using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public Camera camera;
    private Shoot shoot;

    [SerializeField]
    TextMeshProUGUI playerNameText;

    // Start is called before the first frame update
    void Start()
    {
        this.camera = transform.Find("Camera").GetComponent<Camera>();

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("ar"))
        {
            GetComponent<PlayerMovement>().enabled = photonView.IsMine;
            GetComponent<Shoot>().enabled = photonView.IsMine;
            camera.enabled = photonView.IsMine;
        }

        else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("od"))
        {
            GetComponent<PlayerMovement>().enabled = photonView.IsMine;
            GetComponent<Shoot>().enabled = photonView.IsMine;
            camera.enabled = photonView.IsMine;
        }

        playerNameText.text = photonView.Owner.NickName;
    }
}
