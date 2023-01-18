using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectTest : MonoBehaviour, IInteractable
{
    private SpriteRenderer _spriteRenderer;

    private void Start() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interact()
    {
        transform.localScale = transform.localScale + new Vector3(0.2f,0.2f,0.2f);
    }

    public void Deselect()
    {
        _spriteRenderer.color = new Color(1,1,1,1);;
    }

    public void Select()
    {
        _spriteRenderer.color = new Color(0,1,0,1);
    }
}
