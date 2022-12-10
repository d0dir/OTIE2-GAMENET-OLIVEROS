using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSelection : MonoBehaviour
{
    public GameObject[] SelectablePlayers;
    public int playerSelectionNumber;
    // Start is called before the first frame update
    void Start()
    {
        playerSelectionNumber = 0;
        ActivatePlayer(playerSelectionNumber);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //activation of which vehicle was selected. Adding a hashtable to set the properties of the vehicle selected
    private void ActivatePlayer(int x)
    {
        foreach (GameObject go in SelectablePlayers)
        {
            go.SetActive(false);
        }
        SelectablePlayers[x].SetActive(true);

        //Setting the player slection for the vehicle
        ExitGames.Client.Photon.Hashtable playerSelectionProperties = new ExitGames.Client.Photon.Hashtable() { { Constants.PLAYER_SELECTION_NUMBER, playerSelectionNumber } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProperties);
    }

    public void goToNextPlayer()
    {
        playerSelectionNumber++;

        if (playerSelectionNumber >= SelectablePlayers.Length)
        {
            playerSelectionNumber = 0;
        }
        ActivatePlayer(playerSelectionNumber);
    }

    public void goToPrevPlayer()
    {
        playerSelectionNumber--;

        if (playerSelectionNumber < 0)
        {
            playerSelectionNumber = SelectablePlayers.Length - 1;
        }
        ActivatePlayer(playerSelectionNumber);
    }
}
