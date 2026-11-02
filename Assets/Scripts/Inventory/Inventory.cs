using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public GameObject hotBarObj;
    public GameObject inventorySlotParent;

    private List<Slots> inventorySlots = new List<Slots>();
    private List<Slots> hotBarSlots = new List<Slots>();
    private List<Slots> allSlots = new List<Slots>();

    private void Awake()
    {
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<Slots>());
        hotBarSlots.AddRange(hotBarObj.GetComponentsInChildren<Slots>());
        
        allSlots.AddRange(inventorySlots);
        allSlots.AddRange(hotBarSlots);
    }

    public void AddItem(ItemScriptableObject itemToAdd, int amount)
    {
        int remainingAmount = amount;

        foreach (Slots slot in allSlots)
        {
            if (slot.HasItem() && slot.GetItem() == itemToAdd)
            {
                int currentAmount = slot.GetItemCount();
                int maxStackSize = itemToAdd.maxStackSize;

                if (currentAmount < maxStackSize)
                {
                    int spaceLeft = maxStackSize - currentAmount;
                    int amountToAdd = Mathf.Min(spaceLeft, remainingAmount);
                    slot.AddAmount(amountToAdd);
                    remainingAmount -= amountToAdd;
                    if (remainingAmount <= 0)
                        return;
                }
            }
        }

        foreach (Slots slot in allSlots)
        {
            if (!slot.HasItem())
            {
                int amountToPlace = Mathf.Min(itemToAdd.maxStackSize, remainingAmount);
                slot.SetItem(itemToAdd, amountToPlace);
                remainingAmount -= amountToPlace;
                if (remainingAmount <= 0)
                    return;
            }
        }

        if(remainingAmount > 0)
        {
            Debug.LogWarning("Not enough space in inventory to add all items. Remaining amount: " + remainingAmount);

        }
    }
}
