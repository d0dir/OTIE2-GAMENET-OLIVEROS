using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Shooting : MonoBehaviourPunCallbacks
{
    public Camera camera;
    public GameObject hitEffectPrefab;

    [Header("HP Related Stuff")]
    public float startHealth = 100;
    public float health = 0;
    public Image healthBar;

    public bool isControlEnabled;
    public bool eliminated;

    public enum RaiseEventCode
    {
        WhoWasEliminatedEventCode = 0
    }

    private int eliminationOrder;

    void Awake()
    {
        eliminationOrder = PhotonNetwork.CurrentRoom.PlayerCount;
        this.health = startHealth;
        healthBar.fillAmount = this.health / startHealth;

    }

    // Update is called once per frame
    void Update()
    {
        if (isControlEnabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
        }

        CheckRemainingPlayers();
    }

    public void Fire()
    {
        RaycastHit hit;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.4f));
        if (Physics.Raycast(ray, out hit, 200))
        {
            Debug.Log(hit.collider.gameObject.name);

            photonView.RPC("CreateHitEffects", RpcTarget.All, hit.point);

            if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 5);
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage, PhotonMessageInfo info)
    {
        this.health -= damage;
        this.healthBar.fillAmount = health / startHealth;
        Debug.Log(health);

        if (health <= 0)
        {
            this.eliminated = true;
            Eliminated();
            Debug.Log(info.Sender.NickName + " killed " + info.photonView.Owner.NickName);
        }
    }

    [PunRPC]
    public void CreateHitEffects(Vector3 position)
    {
        GameObject hitEffectGameObject = Instantiate(hitEffectPrefab, position, Quaternion.identity);
        Destroy(hitEffectGameObject, 0.2f);
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    //Receiving the custom event and doing something after it was received
    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseEventCode.WhoWasEliminatedEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            string nicknameOfElimnatedPlayer = (string)data[0];
            eliminationOrder = (int)data[1];
            int viewId = (int)data[2];

            Debug.Log(nicknameOfElimnatedPlayer + " was eliminated. Remaining players: " + eliminationOrder);
        }
    }

    //Custom event that we want to call/raise
    public void Eliminated()
    {
        GetComponent<PlayerSetup>().camera.transform.parent = null;
        GetComponent<VehicleMovement>().isControlEnabled = false;
        GetComponent<Shooting>().isControlEnabled = false;

        eliminationOrder--;

        string nickname = photonView.Owner.NickName;
        int viewId = photonView.ViewID;

        object[] data = new object[] { nickname, eliminationOrder, viewId };
        //Raising the custom event we created
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOptions = new SendOptions
        {
            Reliability = false
        };

        PhotonNetwork.RaiseEvent((byte)RaiseEventCode.WhoWasEliminatedEventCode, data, raiseEventOptions, sendOptions);
    }

    [PunRPC]
    public void CheckRemainingPlayers()
    {
        if (eliminationOrder == 1 && this.eliminated == false)
        {
            this.GetComponent<VehicleMovement>().enabled = false;
            this.GetComponent<Shooting>().enabled = false;
            DisplayWin();
        }
    }

    public void DisplayWin()
    {
        GameObject winMessage = GameObject.Find("WinnerText");
        winMessage.GetComponent<Text>().text = "You're the last man standing, you won!";
        winMessage.GetComponent<Text>().color = Color.green;
    }
}
