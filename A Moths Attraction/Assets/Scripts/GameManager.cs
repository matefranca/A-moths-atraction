using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager instance;

    [Header("Camera do Player.")]
    public CinemachineVirtualCamera cam;

    [Header("Player.")]
    public GameObject playerPrefab;
    public Transform instPos;

    [Header("Animators.")]
    public Animator fadeAnim;
    public Animator cocoonAnim;

    [Header("Ghost Manager.")]
    public GhostManager ghostManager;

    [Header("Vidas.")]
    public int vidasMaximas;
    int vidas;

    [Header("CutsceneFinal.")]
    public GameObject cutsceneFinal;

    GameObject player;
    
    [HideInInspector] public bool podeInst;


    private void Awake()
    {
        instance = this;
        player = FindObjectOfType<PlayerController2>().gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        vidas = 0;
        podeInst = false;
        cutsceneFinal.SetActive(false);
        AudioManager.instance.Play("Ambiente");
    }

    private void Update()
    {
        InstantiatePlayer();
    }

    public void InstantiatePlayer()
    {
        if(player != null)
        {
            return;
        }

        if (podeInst)        
        {
            vidas++;
            StartCoroutine(WaitAnim());
            if(vidas < vidasMaximas)
            {
                fadeAnim.SetTrigger("FadeOut");
            }
            podeInst = false;
        }

        else if(vidas >= vidasMaximas)
        {
            ActivateEndCutscene();
        }
    }
    IEnumerator WaitAnim()
    {
        yield return new WaitForSeconds(2f);
        fadeAnim.SetTrigger("FadeIn");

        yield return new WaitForSeconds(0.3f);
        player = Instantiate(playerPrefab, instPos.position, playerPrefab.transform.rotation);
        ghostManager.player = player.transform;
        cam.Follow = player.transform;

        yield return new WaitForSeconds(0.2f);
        ghostManager.playing = true;
        cocoonAnim.SetTrigger("restart");

        yield return new WaitForSeconds(1f);
        cocoonAnim.SetTrigger("start");

        yield return new WaitForSeconds(0.5f);
        player.GetComponent<PlayerController2>().isMove = true;

        ghostManager.recording = true;
    }

    void ActivateEndCutscene()
    {
        cutsceneFinal.SetActive(true);
    }
}
