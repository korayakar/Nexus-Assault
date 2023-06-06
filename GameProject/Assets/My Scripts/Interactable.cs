using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class Interactable : MonoBehaviour // Inherit from interact
{
    // Add or remove an Interactionevents component to this gameobject
    public bool useEvents;
    public string promptMessage;

    public void BaseInteract()
    {
        if (useEvents)
        {
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        }
        Interact();
    }

    protected virtual void Interact()
    {
        // Overriden by subclass
    }

}
