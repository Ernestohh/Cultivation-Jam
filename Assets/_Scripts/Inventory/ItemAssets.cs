using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; } = null;

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
    public Sprite SeedNutriballSpr;
    public Sprite SeedMiseroSpr;
    public Sprite SeedTeslaSpr;
    public Sprite SeedCactusSpr;
    public Sprite SeedGranadaSpr;
    public Sprite FruitNutriballSpr;
    public Sprite FruitMiseroSpr;
    public Sprite FruitTeslaSpr;
    public Sprite FruitCactusSpr;
    public Sprite FruitGranadaSpr;

}
