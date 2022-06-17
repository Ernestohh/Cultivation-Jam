using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onInteract;
    public string interactMessage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeScale()
    {
        gameObject.GetComponent<Transform>().localScale += new Vector3(0, 0.1f, 0);
    }
    public void OpenBook()
    {
        gameObject.GetComponent<BookManager>().OpenBook();
    }
    public void CloseBook()
    {

    }
}
