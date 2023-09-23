using System;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public static ChestManager Singleton;
    public GameObject chestInventory;
    public Chest[] chests;

    private void Awake()
    {
        Singleton = this;
    }

    public void ToggleChestInventory(bool isActivated)
    {
        chestInventory.SetActive(isActivated);
    }

    public void AddItemToChest(Item item)
    {
        int index = -1;
        foreach (var chest in chests)
        {
            if (chest.isActivated)
            {
                index = Array.IndexOf(chests, chest);
            }
        }

        chests[index].MoveItemToChest(item);
    }

    public void RemoveItemFromChest(Item item)
    {
        int index = -1;
        foreach (var chest in chests)
        {
            if (chest.isActivated)
            {
                index = Array.IndexOf(chests, chest);
            }
        }

        chests[index].MoveItemToInventory(item);
    }
}
