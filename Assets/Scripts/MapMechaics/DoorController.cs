using System;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public DoorController Entrance;
    public event Action ToComeIn, GoOut;
    private Transform _transform;

    private void Awake() => Initialize();

    private void Initialize() => _transform = transform;


    public void TakePlayer(GameObject player, Vector3 offset)
    {
        player.transform.position = transform.position + offset;
        ToComeIn?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GoOut?.Invoke();
            Entrance.TakePlayer(other.gameObject, other.transform.position - transform.position);
        }
    }
}
