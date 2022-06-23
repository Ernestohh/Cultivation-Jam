using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        SeedNutriball,
        SeedMisero,
        SeedTesla,
        SeedCactus,
        SeedGranada,
        FruitNutriball,
        FruitMisero,
        FruitTesla,
        FruitCactus,
        FruitGranada,
    }
    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.SeedNutriball:  return ItemAssets.Instance.SeedNutriballSpr;
            case ItemType.SeedMisero:     return ItemAssets.Instance.SeedMiseroSpr;
            case ItemType.SeedTesla:      return ItemAssets.Instance.SeedTeslaSpr;
            case ItemType.SeedCactus:     return ItemAssets.Instance.SeedCactusSpr;
            case ItemType.SeedGranada:    return ItemAssets.Instance.SeedGranadaSpr;
            case ItemType.FruitNutriball: return ItemAssets.Instance.FruitNutriballSpr;
            case ItemType.FruitMisero:    return ItemAssets.Instance.FruitMiseroSpr;
            case ItemType.FruitTesla:     return ItemAssets.Instance.FruitTeslaSpr;
            case ItemType.FruitCactus:    return ItemAssets.Instance.FruitCactusSpr;
            case ItemType.FruitGranada:   return ItemAssets.Instance.FruitGranadaSpr;

        }
    }
}
