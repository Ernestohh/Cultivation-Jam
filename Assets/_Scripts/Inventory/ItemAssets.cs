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
    public Transform pfItemWorld;

    public Material PropMaterial;

    public Sprite SeedNutriballInventorySprite;
    public MeshRenderer SeedNutriballMeshRenderer;
    public Mesh SeedNutriballMesh;
    public Sprite FruitNutriballInventorySprite;
    public MeshRenderer FruitNutriballMeshRenderer;
    public Mesh FruitNutriballMesh;

    public Sprite SeedGranadaInventorySprite;
    public MeshRenderer SeedGranadaMeshRenderer;
    public Mesh SeedGranadaMesh;
    public Sprite FruitGranadaInventorySprite;
    public MeshRenderer FruitGranadaMeshRenderer;
    public Mesh FruitGranadaMesh;

    public Sprite SeedMiseroInventorySprite;
    public MeshRenderer SeedMiseroMeshRenderer;
    public Mesh SeedMiseroMesh;
    public Sprite FruitMiseroInventorySprite;
    public MeshRenderer FruitMiseroMeshRenderer;
    public Mesh FruitMiseroMesh;

    public Sprite SeedTeslaInventorySprite;
    public MeshRenderer SeedTeslaMeshRenderer;
    public Mesh SeedTeslaMesh; 
    public Sprite FruitTeslaInventorySprite;
    public MeshRenderer FruitTeslaMeshRenderer;
    public Mesh FruitTeslaMesh;

    public Sprite SeedCactusInventorySprite;
    public MeshRenderer SeedCactusMeshRenderer;
    public Mesh SeedCactusMesh;
    public Sprite FruitCactusInventorySprite;
    public MeshRenderer FruitCactusMeshRenderer;
    public Mesh FruitCactusMesh;
}
