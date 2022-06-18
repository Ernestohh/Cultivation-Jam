using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public static Interactor Instance = null;
    [SerializeField] Transform interactionPoint;
    [SerializeField] float interactionPointRadius = 0.5f;
    [SerializeField] LayerMask interactableLayerMask;
    [SerializeField] InteractionMessageUI interactionMessageUI;
    public bool isInteractingWithBook;

    readonly Collider[] colliders = new Collider[3];
    int numFound;

    Interactable interactable;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple DetectableTargetManager found. Destroying " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Update()
    {
        numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, colliders, interactableLayerMask);

        if(numFound > 0)
        {
            interactable = colliders[0].GetComponent<Interactable>();

            if (interactable != null)
            {
                if (!interactionMessageUI.isDisplayed && !isInteractingWithBook)// TODO might need better solution
                {
                    interactionMessageUI.Setup(interactable.interactMessage);
                }
                else if (isInteractingWithBook)
                    interactionMessageUI.Close();
              
                if (Input.GetKeyDown(KeyCode.E) && !isInteractingWithBook)
                {
                    interactable.onInteract?.Invoke();
                }
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
