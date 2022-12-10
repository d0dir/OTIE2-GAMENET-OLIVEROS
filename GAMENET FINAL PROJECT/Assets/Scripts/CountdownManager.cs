using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CountdownManager : MonoBehaviourPunCallbacks
{
    public Text timerText;

    public float timeToStartGame = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("ar"))
        {
            timerText = GameManager.instance.timeText;
        }
        else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("od"))
        {
            timerText = GameManager.instance.timeText;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (timeToStartGame > 0)
            {
                timeToStartGame -= Time.deltaTime;
                photonView.RPC("SetTime", RpcTarget.AllBuffered, timeToStartGame);
            }

            else if (timeToStartGame < 0)
            {
                photonView.RPC("StartGame", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    public void SetTime(float time)
    {
        if (time > 0)
        {
            timerText.text = time.ToString("F1");
        }

        else
        {
            timerText.text = " ";
        }
    }

    [PunRPC]
    public void StartGame()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("ar"))
        {
            GetComponent<PlayerMovement>().isControlEnabled = true;
            GetComponent<Shoot>().isControlEnabled = true;
        }
        else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("od"))
        {
            GetComponent<PlayerMovement>().isControlEnabled = true;
            GetComponent<Shoot>().isControlEnabled = true;
        }

        this.enabled = false;
    }
}
