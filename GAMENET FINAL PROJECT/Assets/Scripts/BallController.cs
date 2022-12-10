using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class BallController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "GoalTriggerHome")
        {
            GameManager.instance.scoreOne++;
            GameManager.instance.textOne.text = GameManager.instance.scoreOne.ToString();
            photonView.RPC("CheckWinner", RpcTarget.AllBuffered);
        }

        else if (col.gameObject.tag == "GoalTriggerAway")
        {
            GameManager.instance.scoreTwo++;
            GameManager.instance.textTwo.text = GameManager.instance.scoreTwo.ToString();
            photonView.RPC("CheckWinner", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void CheckWinner()
    {
        if (GameManager.instance.scoreOne >= GameManager.instance.scoreToWin)
        {
            GameManager.instance.winnerText.text = "Home Wins";
            GetComponent<PlayerMovement>().isControlEnabled = false;
            GetComponent<Shoot>().isControlEnabled = false;
        }

        if (GameManager.instance.scoreTwo >= GameManager.instance.scoreToWin)
        {
            GameManager.instance.winnerText.text = "Away Wins";
            GetComponent<PlayerMovement>().isControlEnabled = false;
            GetComponent<Shoot>().isControlEnabled = false;
        }
    }
}
