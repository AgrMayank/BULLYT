using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIController : MonoBehaviour
{
    public static PlayerUIController Instance = null;

    private BULLYTInput input = null;

    private bool attackPressed = false;

    public GameObject level1Lost;

    public TMP_Text m_title, m_subtitle;

    public AudioSource m_audioSource;

    public AudioClip m_winClip, m_loseClip;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        input = new();
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.AttackSelect.performed += OnAttack;
    }

    private void OnDestroy()
    {
        input.Player.AttackSelect.performed -= OnAttack;
    }

    private void OnAttack(InputAction.CallbackContext value)
    {
        if (level1Lost.activeInHierarchy && !attackPressed)
        {
            attackPressed = true;
            LevelManager.Instance.LoadLevel("01 Home");
        }
    }

    public void LevelLost(string subtitle)
    {
        m_audioSource.clip = m_loseClip;
        m_audioSource.Play();

        Debug.Log("HERE");
        m_subtitle.text = subtitle;
        StartCoroutine(Lost());
    }

    public void LevelWon(string subtitle, string title)
    {
        m_audioSource.clip = m_winClip;
        m_audioSource.Play();
        Debug.Log("THERE");
        m_title.text = title;
        m_subtitle.text = subtitle;
        StartCoroutine(Lost());
    }

    IEnumerator Lost()
    {
        yield return new WaitForSeconds(3f);
        level1Lost.SetActive(true);

        // HealthController.Instance.m_playerLost = true;
    }
}
