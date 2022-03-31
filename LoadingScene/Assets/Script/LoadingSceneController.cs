using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{

    public static string nextScene;
    // 함수와 변수를 static으로 선언해두면 LoadingScene으로 넘어오지 않아서 
    // LoadingSceneController스크립트가 부착된 게임 오브젝트가 생성되지 않더라도
    // LoadingSceneController의 클래스 이름으로 호출해서 사용할 수 있게 된다.
    // static함수의 내부에서는 static으로 선언되지 않은 일반 멤버 변수나 함수는 바로 호출할 수 없다.
    // 따라서 쓰기 전에 초기화 작업이 필요하다.

    [SerializeField] // private멤버지만 inspector창에서 접근이 가능하도록 하게 해준다.
    Image progressBar;
    
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        // SceneManager.LoadSceneAsync
        // 우선 비동기란, 동시에 일어나지 않는다는 뜻
        // SceneManager.LoadScene을 하게 되면 해당 데이터를 동시에 가져오기 시작하는데,
        // 여기서 문제는 해당 데이터를 다 가져올 때까지 사용자는 아무런 동작을 할 수 없다.
        // 흔히 '렉'이라고 부르는데 유니티에서는 이를 해결하기위해
        // SceneManager.LoadSceneAsync 함수를 사용하면 함수가
        // AsyncOperation 타입으로 반환해준다. 이를 통해 비동기 코루틴으로 실행하는 것으로
        // 위 문제를 해결할 수 있다.
        // 출처 : https://kumgo1d.tistory.com/11
        
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false; 
        /*
         씬을 비동기로 불러올 때 씬의 로딩이 끝나면 자동으로 불러온 씬으로 이동할 것인지를 설정하는 것인데.
         이 값을 false로 설정하면 씬을 90%까지만 로드한 상태로 다음 씬으로 넘어가지 않고 기다리게 되며
         다시 값을 true로 바꿨을 때 남은 부분을 로딩하고 씬을 불러오게 됩니다. == 계속하려면 아무키나 누르세요.
         이러한 방식을 사용하는 이유
         1. 씬로딩이 생각보다 빠를 수 있기 때문이다. => 우리가 적어놓은 게임 팁이나 스토리가 플레이어에게 전달이 안된다.
         2. 로딩화면에서 불러와야할 것이 씬만 있는게 아니기 때문이다. 
         => 게임의 볼륨이 커지게되면 게임을 통째로 빌드하는 것이 아니라 에셋번들이라는 것으로 나눠서 빌드하고
            이 에셋번들로부터 리소스를 읽어와야 하는 상황이 생긴다. 이 리소스가 먼저 로딩이 끝난다면 문제가 없겠지만,
            리소스 로딩이 끝나기 전에 씬 로딩이 끝나면 씬에 리소스 로딩이 되지 않은 오브젝트들이 마구 깨져서 보이게 된다.
        */
        
        // 씬의 로딩 상황을 진행바로 표시하는 코드를 작성
        // 시간측정에 필요한 Timer 변수
        float timer = 0f;
        while (!op.isDone) // op.isDone이 아닐 때, 즉 씬 로딩이 끝나지 않은 상태라면 계속해서 반복
        {
            yield return null; // while문의 반복문이 실행 될 때 마다 유니티 엔진에 제어권을 넘기도록 한다.
                               // 제어권을 넘기지 않는다면 반복문에 끝나기 전에는 화면이 갱신되지 않아서
                               // 진행바가 차오르는게 보이지 않는다.
                               
            // 90%보다 작다면 이미지 progressBar의 fillAmount를 op.progress로 만들어서 로딩 진행도를 표시하도록 만든다.
            if (op.progress < 0.9f)
                progressBar.fillAmount = op.progress;
            // 90% 이상일 경우 Fake 로딩을 진행하도록 만든다.
            else
            {
                timer += Time.unscaledDeltaTime;
                // delta time은 게임에서 플레이어의 입력과 게임 처리,
                // 화면 렌더링 등의 작업이 한 번 처리되고 그 결과물이
                // 플레이어의 모니터에 그려지는 한 프레임이 진행되는 시간을 의미합니다.
                // 이 delta time에 unscaled를 붙임으로써 크기가 바뀌지 않은 delta time을 의미하게 됩니다.
                //출처: https://wergia.tistory.com/314 [베르의 프로그래밍 노트]
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                // Mathf.Lerp(float a, float b, float t)
                // t라는 시간 안에 변수의 초기값 a가 b가 되게 하는 것.
                if (progressBar.fillAmount >= 1)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }




        }
    }

}
