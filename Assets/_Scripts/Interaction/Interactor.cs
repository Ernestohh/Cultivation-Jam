using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] Transform interactionPoint;
    [SerializeField] float interactionPointRadius = 0.5f;
    [SerializeField] LayerMask interactableLayerMask;
    [SerializeField] InteractionMessageUI interactionMessageUI;
    public bool isInteractingWithSomething;

    readonly Collider[] colliders = new Collider[3];
    int numFound;

    Interactable interactable;


    // Update is called once per frame
    void Update()
    {
        numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, colliders, interactableLayerMask);

        if(numFound > 0)
        {
            interactable = colliders[0].GetComponent<Interactable>();

            if (interactable != null)
            {
                if (!interactionMessageUI.isDisplayed)// TODO might need better solution
                {
                    interactionMessageUI.Setup(interactable.interactMessage);
                }
              
                if (Input.GetKeyDown(KeyCode.E) /*&& interactable.GetComponent<BookManager>().bookIsClosed*/)
                {
                    interactable.onInteract?.Invoke();
                }
                if(Input.GetKeyDown(KeyCode.F))
                    interactable.onInteract?.Invoke();
            }
        }
        else
        {
            if (interactable != null) interactable = null;
            if (interactionMessageUI.isDisplayed) interactionMessageUI.Close();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
