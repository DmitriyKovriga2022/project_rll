using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlaceInteractScript : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject tower;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Deselect()
    {
        _spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void Interact()
    {
        Debug.Log("Interact");
        Instantiate(tower, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }

    public void Select()
    {
        _spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
