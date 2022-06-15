using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DawnToDuskController : MonoBehaviour
{
    [field: SerializeField] public Material proceduralSkybox { get; set; }
    [field: SerializeField] public bool DayPassed { get; set; }
    [field: SerializeField] public float Lerp { get; set; } = 1.5f;
    [field: SerializeField] public DayManager DayManager { get; set; }
    public void ProgressTheDay()
    {
        DayPassed = false;
        Lerp = 1.5f;
        StartCoroutine(FromDawnToDuskCoroutine());
    }

    public void EndTheDay()
    {
        DayPassed = true;
        StopCoroutine(FromDawnToDuskCoroutine());
        DayManager.OnStartOfDay?.Invoke();
    }
    
    protected IEnumerator FromDawnToDuskCoroutine()
    {
        do
        {
            Lerp -= 0.0001f;
            proceduralSkybox.SetFloat("_AtmosphereThickness", Lerp);
            if (Lerp <= 0.2f)
                DayPassed = true;
            yield return null;
        } while (!DayPassed);
    }

    private void Start()
    {
        DayManager = GetComponent<DayManager>();
    }

    private void Update()
    {
        if (!DayPassed) return;
        DayManager.OnEndOfDay?.Invoke();
    }
}
