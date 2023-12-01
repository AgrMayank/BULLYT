﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance = null;

    public GameObject m_fadeStart;

    public static string levelToLoad;

    private bool isLoadingLevel = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Application.targetFrameRate = 120;
        Time.timeScale = 1;
    }

    private void Start()
    {
        GC.Collect();
    }

    public void LoadNextLevel()
    {
        Debug.Log("PRESSED!!!");

        LoadLevel("", 1, SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLevel(string levelName)
    {
        LoadLevel(levelName, 0);
    }

    public void LoadLevel(string levelName, int delay, int levelIndex = -1)
    {
        if (isLoadingLevel)
        {
            return;
        }
        else
        {
            isLoadingLevel = true;
        }

        levelToLoad = levelName;
        Debug.Log("New Level Load : " + levelToLoad);

        StartCoroutine(LoadYourAsyncScene(delay, levelIndex));
    }

    IEnumerator LoadYourAsyncScene(int delay, int levelIndex = -1)
    {
        yield return new WaitForSeconds(delay);

        if (m_fadeStart != null)
            m_fadeStart.SetActive(true);

        AsyncOperation asyncLoad;
        if (levelIndex >= 0)
        {
            asyncLoad = SceneManager.LoadSceneAsync(levelIndex);
        }
        else
        {
            asyncLoad = SceneManager.LoadSceneAsync(levelToLoad);
        }

        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9F)
        {
            yield return null;
        }

        Debug.Log(asyncLoad.progress);
        yield return new WaitForSeconds(0.8f);

        asyncLoad.allowSceneActivation = true;

        // Reset level load bool
        isLoadingLevel = false;
    }

    public void QuitRequest()
    {
        Debug.Log("Quit Requested!");
        Application.Quit();
    }
}
