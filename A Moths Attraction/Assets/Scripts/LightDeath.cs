using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDeath : MonoBehaviour
{
    public ParticleSystem flareParticle;
    public GhostManager ghost;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.Play("MariposaMorrendo");
            ghost.recording = false;
            Destroy(other.gameObject);
            GameManager.instance.podeInst = true;
            flareParticle.Play();
        }
    }
}
