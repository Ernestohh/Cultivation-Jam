using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
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

    //bütün bu get fonksiyonlarý sanki çok sistem yiycekmiþ gibi geliyo ama bilmiyorum merak ediyorum
    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default: 
            case ItemType.SeedNutriball: return ItemAssets.Instance.SeedNutriballInventorySprite;
            case ItemType.SeedMisero: return ItemAssets.Instance.SeedNutriballInventorySprite;
            case ItemType.SeedGranada: return ItemAssets.Instance.SeedGranadaInventorySprite;
            case ItemType.SeedTesla: return ItemAssets.Instance.SeedTeslaInventorySprite;
            case ItemType.SeedCactus: return ItemAssets.Instance.SeedCactusInventorySprite;
        }
    }
    public MeshRenderer GetMeshRenderer()
    {
        switch (itemType)
        {
            default:
            case ItemType.SeedNutriball: return ItemAssets.Instance.SeedNutriballMeshRenderer;
            case ItemType.SeedMisero: return ItemAssets.Instance.SeedMiseroMeshRenderer;
            case ItemType.SeedGranada: return ItemAssets.Instance.SeedGranadaMeshRenderer;
            case ItemType.SeedTesla: return ItemAssets.Instance.SeedTeslaMeshRenderer;
            case ItemType.SeedCactus: return ItemAssets.Instance.SeedCactusMeshRenderer;
            case ItemType.FruitNutriball: return ItemAssets.Instance.FruitNutriballMeshRenderer;
            case ItemType.FruitMisero: return ItemAssets.Instance.FruitMiseroMeshRenderer;
            case ItemType.FruitGranada: return ItemAssets.Instance.FruitGranadaMeshRenderer;
            case ItemType.FruitTesla: return ItemAssets.Instance.FruitTeslaMeshRenderer;
            case ItemType.FruitCactus: return ItemAssets.Instance.FruitCactusMeshRenderer;
        }
    }
  
    public Mesh GetMesh()
    {
        switch (itemType)
        {
            default:
            case ItemType.SeedNutriball: return ItemAssets.Instance.SeedNutriballMesh;
            case ItemType.SeedMisero: return ItemAssets.Instance.SeedMiseroMesh;
            case ItemType.SeedGranada: return ItemAssets.Instance.SeedGranadaMesh;
            case ItemType.SeedTesla: return ItemAssets.Instance.SeedTeslaMesh;
            case ItemType.SeedCactus: return ItemAssets.Instance.SeedCactusMesh;
            case ItemType.FruitNutriball: return ItemAssets.Instance.FruitNutriballMesh;
            case ItemType.FruitMisero: return ItemAssets.Instance.FruitMiseroMesh;
            case ItemType.FruitGranada: return ItemAssets.Instance.FruitGranadaMesh;
            case ItemType.FruitTesla: return ItemAssets.Instance.FruitTeslaMesh;
            case ItemType.FruitCactus: return ItemAssets.Instance.FruitCactusMesh;
        }
    }
    public Material GetMaterial()
    {
        return ItemAssets.Instance.PropMaterial;
    }
    public Color GetColor()
    {
        switch (itemType)
        {
            default: return new Color(0.86f, 0.24f, 0.79f);
        }
    }
    public bool IsStackable()
    {
        switch (itemType)
        {
            default : return true;
        }
    }
}
