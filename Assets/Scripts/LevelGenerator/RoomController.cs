using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomController : MonoBehaviour
{
    private CinemachineConfiner _cmConfiner;
    [SerializeField] private Collider2D _confiner;

    public void Initialize(CinemachineConfiner cmConfiner)
    {
        _cmConfiner = cmConfiner;
    }

    public void RegisterExit(Exit exit)
    {
        exit.ToComeIn += SetRoomActive;
    }

    public void SetRoomActive()
    {
        _cmConfiner.m_BoundingShape2D = _confiner;
    }
}
