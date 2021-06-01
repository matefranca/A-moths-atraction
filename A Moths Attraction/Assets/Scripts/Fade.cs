using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public Animator cocoonAnim;

    public void StartCocoon() 
    {
        cocoonAnim.SetTrigger("start");
    }
}
