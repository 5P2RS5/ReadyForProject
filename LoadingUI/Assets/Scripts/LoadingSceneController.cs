using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 이 스크립트가 하는 일 : 기존 씬을 UI로 가리고 다른 씬을 불러오는 작업을 한다.
public class LoadingSceneController : MonoBehaviour
{
    static LoadingSceneController instance;

    public static LoadingSceneController Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<LoadingSceneController>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }
    }

    public static LoadingSceneController Create()
    {
        return Instantiate(Resources.Load<LoadingSceneController>("LoadingUI"));
    }

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Image progressBar;

    string loadSceneName; // 씬 저장 변수

    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadSceneName = sceneName;
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess() // 씬을 로딩하는 과정
    {
        progressBar.fillAmount = 0f;
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledTime * 0.001f;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1, timer);
                if (progressBar.fillAmount >= 1)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) // 씬 로딩이 끝난 시점을 알려준다.
    {
        if (arg0.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    IEnumerator Fade(bool isFadeIn) // 씬 로딩을 시작하거나 끝날때 자연스러운 FadeIn/Out을 위한 것
    {
        float timer = 0f;
        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledTime * 0.1f;
            _canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }
}
