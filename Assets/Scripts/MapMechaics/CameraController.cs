using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _cm;
    private CinemachineFramingTransposer _cmTransposer;
    private CinemachineConfiner _cmConfiner;

    private void Awake() 
    {
        _cm = GetComponent<CinemachineVirtualCamera>();
        _cmTransposer = _cm.GetCinemachineComponent<CinemachineFramingTransposer>();
        _cmConfiner = GetComponent<CinemachineConfiner>();
    }

    public void SetRoom(Collider2D confiner)
    {
        _cmConfiner.m_BoundingShape2D = confiner;
        StartCoroutine(ResetDamping());
    }
    
    private IEnumerator ResetDamping()
    {
        yield return new WaitForEndOfFrame();
        SetDamping(1.0f);
    }

    private void SetDamping(float value)
    {
        _cmTransposer.m_XDamping = value;
        _cmTransposer.m_YDamping = value;        
    }

    public void DeleteRoom() => SetDamping(0.0f);
}
