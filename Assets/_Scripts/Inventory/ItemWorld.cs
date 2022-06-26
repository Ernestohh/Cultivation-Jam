using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position ,Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);

        if (transform == null)
        {
            Debug.Log("transform null");
        }
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }
    public static ItemWorld DropItem(Vector3 dropPosition, Item item)
    {
        Vector3 randomDir = UtilsClass.GetRandomDir();
        //Debug.Log(randomDir );
        ItemWorld itemWorld = SpawnItemWorld(dropPosition + randomDir * 0.2f, item);
        itemWorld.GetComponent<Rigidbody>().AddForce(randomDir, ForceMode.Impulse);
        return itemWorld;
    }
    private Item item;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    Mesh mesh;
    Light light;
    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        light = GetComponent<Light>();
    }
    public void SetItem(Item item)
    {
        this.item = item;
        meshRenderer.material = item.GetMaterial();
        meshFilter.mesh = item.GetMesh();
        light.color = item.GetColor();
    }
    public Item GetItem()
    {
        return this.item;
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
