using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class DayManager : MonoBehaviour
{ 
    [field: SerializeField] public UnityEvent OnEndOfDay { get; set; }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
            OnEndOfDay?.Invoke();
    }
}
