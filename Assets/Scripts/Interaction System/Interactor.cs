using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private Transform _transform;

    private List<Transform> _interactableTransforms = new();
    private IInteractable _interactable;

    void Start() 
    {
        _transform = gameObject.transform;
    }

    void Update()
    {
        if(_interactableTransforms.Count > 0)
        {
            ChooseInteractable();
        }
        else if(_interactable != null) 
        {
            DeleteInteractable();
        }
    }

    private void ChooseInteractable()
    {
        IInteractable newInteractable = GetNearestInteractable();

        if (_interactable == newInteractable) return;
        
        if (_interactable != null) _interactable.Deselect();

        _interactable = newInteractable;
        _interactable.Select();
    }

    private void DeleteInteractable()
    {
        _interactable.Deselect();
        _interactable = null; 
    }

    private IInteractable GetNearestInteractable()
    {
        Transform interactableObject = null;
        float minDistance = 0;

        foreach(var interactableTransform in _interactableTransforms)
        {
            float distance = Vector2.Distance(_transform.position, interactableTransform.position);
            if (distance < minDistance || minDistance == 0) 
            {
                minDistance = distance;
                interactableObject = interactableTransform;
            }
        }
        return interactableObject.GetComponent<IInteractable>();
    }

    public void Interact()
    {
        if(_interactable != null) { _interactable.Interact(); }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (other.TryGetComponent<IInteractable>(out interactable))
        {
            _interactableTransforms.Add(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (other.TryGetComponent<IInteractable>(out interactable))
        {
            _interactableTransforms.Remove(other.transform);
        }
    }
}
