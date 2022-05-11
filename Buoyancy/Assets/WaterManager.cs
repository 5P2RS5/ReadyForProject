using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RequireComponent를 사용하는 스크립트를 추가하면, 요구되는 컴포너트가 자동으로 해당 게임오브젝트에 추가됩니다.
// 설정 오류를 피하는 데 유용합니다.
// 예를 들어, 리지드 바디 가 요구되는 스크립트가, 항상 같은 게임오브젝트에 첨부되는 경우.
// RequireComponent를 사용하면 이 작업이 자동으로 이루어 지기 때문에, 설정에 대한 실수를 할 염려가 없습니다.

[RequireComponent(typeof(MeshFilter))]
// Mesh Filter는 에셋에서 메쉬를 취득하여, 화면상에서 렌더링하기 위해 Mesh Renderer에 전달합니다.

[RequireComponent(typeof(MeshRenderer))]
// MeshFilter 또는 TextMesh에 의해 삽입되는 메쉬를 렌더링합니다.

public class WaterManager : MonoBehaviour
{
    MeshFilter meshFilter;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    void Update()
    {
        Vector3[] vertices = meshFilter.mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = WaveManager.instance.GetWaveHeight(transform.position.x + vertices[i].x);
        }

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateNormals();
        // 삼각면과 정점으로부터, 메쉬의 노멀(normal)을 다시 계산합니다.
    }
}
