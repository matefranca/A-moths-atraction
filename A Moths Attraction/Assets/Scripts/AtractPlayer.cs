using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtractPlayer : MonoBehaviour
{
    public float atractorSpeed;
    Transform player;

    private void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetPlayer();
            player.position = Vector3.MoveTowards(player.position, transform.position, atractorSpeed * Time.deltaTime);
        }
    }

    void GetPlayer()
    {
        player = FindObjectOfType<PlayerController2>().transform;
    }
}
