using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocoonOpening : MonoBehaviour
{
    PlayerController2 player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController2>();
    }

    public void ChangeIsMove()
    {
        player.isMove = true;
    }
}
