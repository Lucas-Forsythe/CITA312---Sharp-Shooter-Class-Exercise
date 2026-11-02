using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "NewItem")]
public class ItemScriptableObject: ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int maxStackSize;
    public GameObject itemPrefab;
    public GameObject heldItem;
}
