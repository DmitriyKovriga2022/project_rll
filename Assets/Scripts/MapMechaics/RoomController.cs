using System;
using UnityEngine;
using System.Collections.Generic;

public class RoomController : MonoBehaviour
{
    [SerializeField] private Collider2D _confiner;
    public event Action<Collider2D> ActivateRoom;
    private CameraController _cameraController;
    private List<DoorController> _registeredDoors;

    public void Initialize(CameraController cameraController)
    {
        _cameraController = cameraController;
        ActivateRoom += cameraController.SetRoom;
    }

    public void RegisterExit(DoorController door)
    {
        door.ToComeIn += SetRoomActive;
        door.GoOut += _cameraController.DeleteRoom;
    }

    public void SetRoomActive()
    {
        ActivateRoom?.Invoke(_confiner);
    }
}
