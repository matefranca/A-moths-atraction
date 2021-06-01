using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Paineis.")]
    public GameObject painelInicial;
    public GameObject painelControles;
    public GameObject painelCreditos;

    [Header("Slider Volume.")]
    public Slider sliderMusica;

    [Header("Sprites da Mariposa.")]
    public Image rendererDaMariposa;
    public Sprite imagemMariposaNova;
    public Sprite imagemMariposaPadrao;

    [Header("Animacao Play.")]
    public Animator animacaoFinal;

    private void Start()
    {
        painelInicial.SetActive(true);
        painelControles.SetActive(false);
        painelCreditos.SetActive(false);

        AudioManager.instance.Play("Ambiente");
        rendererDaMariposa.sprite = imagemMariposaPadrao;
    }

    public void PlayButton()
    {
        StartCoroutine(PlayGame());
    }

    IEnumerator PlayGame()
    {
        rendererDaMariposa.sprite = imagemMariposaNova;
        animacaoFinal.SetTrigger("fade");
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Game");
    }

    public void VolumeMusica()
    {
        float volume = sliderMusica.value;
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void SomBotao()
    {
        AudioManager.instance.Play("Botao");
    }

    public void LigarDesligarSlider()
    {
        sliderMusica.gameObject.SetActive(!sliderMusica.gameObject.activeSelf);
    }
}
