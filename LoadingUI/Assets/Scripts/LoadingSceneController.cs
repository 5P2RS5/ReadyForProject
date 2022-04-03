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

    public static LoadingSceneController Instance // 어느 곳에서든 접근이 가능하도록 정적으로 선언
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<LoadingSceneController>();
                if (obj != null)
                {
                    instance = obj; // 기존에 생성이 되어있다면 객체 넣기
                }
                else
                {
                    instance = Create(); // 생성 안되어있으면 새로 만들기
                }
            }
            return instance;
        }
    }

    public static LoadingSceneController Create() // 만드는 것은 만들어둔 prefab을 사용해서 불러와서 인스턴스화로 한다.
    {                                             // 인스턴스화 하는 이유는 게임 중에 오브젝트를 생성할 수 없기 때문에
        return Instantiate(Resources.Load<LoadingSceneController>("LoadingUI"));
    }

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject); // 인스턴스로 만들어진 객체가 자기 자신인지 확인하여 일치하지 않으면 삭제
        }
        DontDestroyOnLoad(gameObject); // 씬 전환 중 삭제가 안되게 하기 위해서
    }

    [SerializeField] CanvasGroup _canvasGroup; // private이지만 inspector창에서 접근 가능 
    [SerializeField] Image progressBar;

    string loadSceneName; // 씬 저장 변수

    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded = 씬이 바뀔때 마다 불러와진다. 델리게이트
        loadSceneName = sceneName;
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess() // 씬을 로딩하는 과정
    {
        progressBar.fillAmount = 0f;
        yield return StartCoroutine(Fade(true));

        // 비동기 방식으로 구현하는 이유 : 동기식일 경우 씬 로딩이 100퍼센트가 되자마자 다음 씬으로 넘어가게 되어 팁이나 스토리를 플레이어에게 제공이 불가능
        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false; // 값이 false라면 씬 로딩 90퍼 지점에 멈춘다.

        float timer = 0f;
        while (!op.isDone) // 씬 로딩이 끝나지 않은 상태라면 계속해서 반복시킨다.
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
            yield return null; // 1프레임을 호출자에게 양보하란 뜻 Update()함수가 끝나고 난 후에 아래 작성된 코드를 실행
            timer += Time.unscaledTime * 0.1f;
            _canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }
}
