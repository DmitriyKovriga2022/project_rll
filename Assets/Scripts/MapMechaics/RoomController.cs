using System.Collections;
using System;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private Collider2D _confiner;
    public event Action<Collider2D> ActivateRoom;
    private CameraController _cameraController;

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
