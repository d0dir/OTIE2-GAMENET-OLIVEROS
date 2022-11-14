using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Shooting : MonoBehaviourPunCallbacks
{
    public Camera camera;
    public GameObject hitEffectPrefab;

    [Header("HP Related Stuff")]
    public float startHealth = 100;
    private float health;
    public Image healthBar;

    [Header("Score Related STuff")]
    public int confirmedKills = 0;
    private int winningKills = 10;
    public bool isAlive = true;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        healthBar.fillAmount = health / startHealth;
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Win()
    {
        GameObject winMessage = GameObject.Find("Winner Text");
        winMessage.GetComponent<Text>().text = "You Win!";
    }

    public void Fire()
    {
        RaycastHit hit;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if(Physics.Raycast(ray, out hit, 200))
        {
            Debug.Log(hit.collider.gameObject.name);
            photonView.RPC("CreateHitEffects", RpcTarget.All, hit.point);

            if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                if(hit.collider.gameObject.GetComponent<Shooting>().isAlive)
                {
                    hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 25);

                    if(hit.collider.gameObject.GetComponent<Shooting>().health <= 0)
                    {
                        AddScore();
                    }
                }
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage, PhotonMessageInfo info)
    {
        this.health -= damage;
        this.healthBar.fillAmount = health / startHealth;

        if (health <= 0)
        {
            Die();
            StartCoroutine(KillFeed(info));
            Debug.Log(info.Sender.NickName + " killed " + info.photonView.Owner.NickName);
        }
    }

    [PunRPC]
    public void CreateHitEffects(Vector3 position)
    {
        GameObject hitEffectGameObject = Instantiate(hitEffectPrefab, position, Quaternion.identity);
        Destroy(hitEffectGameObject, 0.2f);
    }

    public void Die()
    {
        if (photonView.IsMine)
        {
            animator.SetBool("isDead", true);
            StartCoroutine(RespawnCountdown());
        }
    }

    IEnumerator RespawnCountdown()
    {
        GameObject respawnText = GameObject.Find("Respawn Text");
        float respawnTime = 5.0f;

        while(respawnTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime--;

            transform.GetComponent<PlayerMovementController>().enabled = false;
            respawnText.GetComponent<Text>().text = "You died. Respawning in " + respawnTime.ToString(".00");
        }

        animator.SetBool("isDead", false);
        respawnText.GetComponent<Text>().text = "";

        Respawn.instance.spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        int index = Random.Range(0, Respawn.instance.spawnPoints.Length);
        this.transform.position = Respawn.instance.spawnPoints[index].transform.position;

        transform.GetComponent<PlayerMovementController>().enabled = true;

        photonView.RPC("RegainHealth", RpcTarget.AllBuffered);
    }

    IEnumerator KillFeed(PhotonMessageInfo info)
    {
        GameObject killFeedText = GameObject.Find("KillFeed Text");
        float messageUpTime = 5.0f;
        while (messageUpTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            messageUpTime--;
            killFeedText.GetComponent<Text>().text = info.Sender.NickName + " killed " + info.photonView.Owner.NickName;
        }
        killFeedText.GetComponent<Text>().text = "";
    }

    [PunRPC]
    public void RegainHealth()
    {
        health = 100;
        healthBar.fillAmount = health / startHealth;
    }

    [PunRPC]
    public void AddScore()
    {
        this.confirmedKills += 1;
        if (confirmedKills >= winningKills)
        {
            Win();
        }
    }
}



