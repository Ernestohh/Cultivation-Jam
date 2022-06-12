using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public LayerMask interactableLayer;
    public GameObject interactableCursor;
    SphereCollider ourSphereCollider;
    UnityEvent OnInteract;
    // Start is called before the first frame update
    void Start()
    {
        ourSphereCollider = GetComponent<SphereCollider>();
          
    }

    // Update is called once per frame
    void Update()
    {
        RaycastAndInteract();
    }
    void RaycastAndInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, 1.5f, interactableLayer))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.GetComponent<Interactable>() != false)
                {
                    OnInteract = hit.collider.GetComponent<Interactable>().onInteract;
                    OnInteract.Invoke();
                }
            }
        }
    }
    void ColliderInteract()
    {

    }
}
