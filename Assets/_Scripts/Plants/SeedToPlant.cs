using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedToPlant : MonoBehaviour
{
    public bool seedPlanted = false;

    public IEnumerator ManagePlantGrowth()
    {
        do
        {
  
        }
        while (seedPlanted);
        yield return null;
    }
    public void SetPlantData(Item item)
    {

    }
}
