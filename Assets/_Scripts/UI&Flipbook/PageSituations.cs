using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageSituations : MonoBehaviour
{
    const float kPageTurnSpringStrength = 20.0f;
    const float kPageTurnSpringDamping = 0.00001f;
    public Spring page_spring = new Spring(0.0f, 0.0f, kPageTurnSpringStrength, kPageTurnSpringDamping, true);
    public int page_number;

    public Transform rightPos;
    public Transform leftPos;
}
