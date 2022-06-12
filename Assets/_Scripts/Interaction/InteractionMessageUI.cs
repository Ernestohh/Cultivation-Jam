using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionMessageUI : MonoBehaviour
{
   
    [SerializeField] GameObject uiPanel;
    [SerializeField] Text messageText;
    // Start is called before the first frame update
    void Start()
    {
        uiPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool isDisplayed = false;
    public void Setup(string messageText)
    {
        this.messageText.text = messageText;
        uiPanel.SetActive(true);
        isDisplayed = true;
    }
    public void Close()
    {
        uiPanel.SetActive(false);
        isDisplayed = false;
    }
}
