using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherComponent : MonoBehaviour
{
    void Start()
    {
        // var newGameObject = new GameObject().AddComponent<SingletonComponent>(); 이러한 과정이 필요가 없다.
        SingletonComponent.Instance.name = "1234";
    }

    void Update()
    {
        
    }
}
