using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onInteract;
    public string interactMessage;
    public void ChangeScale()
    {
        gameObject.GetComponent<Transform>().localScale += new Vector3(0, 0.1f, 0);
      
    }
}
