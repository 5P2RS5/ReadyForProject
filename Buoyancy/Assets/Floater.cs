using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f; // 잠기기 전에 깊이
    public float displacementAmount = 3f;
    public int floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;
    
    void FixedUpdate()
    {
        rigidBody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);
        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);

        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier =
                Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            // Clamp = 최소 최대값을 설정하여 float값이 범위 이외의 값을 넘지 못하도록 함
            // Clamp = 0에서 1의 값을 돌려줍니다. value 인수가 0 이하이면 0, 이상이면 1입니다.
            rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), 
                transform.position, ForceMode.Acceleration);
            // Physics.gravity = 씬의 모든 리지드바디(rigidbody)에 적용되는 중력을 나타냅니다.
            // ForceMode.Acceleration
            // 질량을 무시하고, 리지드바디에(rigidbody)에 연속적인 가속력(Acceleration)을 가합니다.
            // 해당 시간동안 매 FixedUpdate마다 가속력을 적용합니다. ForceMode.Force와 대조적으로,
            // Acceleration은 질량의 차이와 관계없이 모든 리지드바디를 똑같이 이동시킵니다. 
            // AddForceAtPosition /position/에 /force/를 적용합니다. 그 결과로 오브젝트에 토크와 힘을 적용할 것입니다.
            
            rigidBody.AddForce(displacementMultiplier * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime,
                ForceMode.VelocityChange);
            rigidBody.AddTorque(displacementMultiplier * -rigidBody.velocity * waterAngularDrag * Time.fixedDeltaTime,
                ForceMode.VelocityChange);
            
        }
    }
}
