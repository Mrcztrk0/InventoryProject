using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] ObjectItemData objectItem;

    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary; // Pair up the UI slots with the system slots.

    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;

    protected virtual void Start()
    {
        
    }

    public abstract void AssignSlot(InventorySystem invToDisplay); // Implemented in child classes.

    protected virtual void UpdateSlot(InventorySlot updateSlot)
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updateSlot) // Slot value - the "under the hood" inventory slot.
            {
                slot.Key.UpdateUISlot(updateSlot); // Slot key - the UI representation of the value.
            }
        }
    }

    public void SlotClicked(InventorySlot_UI clickedUISlot)
    {
        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;
        // Clicked slot has an item - mouse doesn't have an item - pick up that item.
        
        //Does the clicked slot have item data - Does the mouse have no item data?
        if (clickedUISlot.AssignedInventorySlot.ItemData != null && objectItem.AssignedInventorySlot.ItemData == null)
        {
            // If player is holding shift key? Split the stack.
            if (isShiftPressed && clickedUISlot.AssignedInventorySlot.SplitStack(out  InventorySlot halfStackSlot)) // Split stack.
            {
                objectItem.UpdateObjectSlot(halfStackSlot);
                clickedUISlot.UpdateUISlot();
                return;
            }
            else // Pick up the item in the clicked slot.
            {
                objectItem.UpdateObjectSlot(clickedUISlot.AssignedInventorySlot);
                clickedUISlot.ClearSlot();
                return;
            }
        }
        
        // Clicked slot doesn't have an item - Mouse does have an item - place the mouse item into the empty slot.

        if (clickedUISlot.AssignedInventorySlot.ItemData == null && objectItem.AssignedInventorySlot.ItemData != null)
        {
            clickedUISlot.AssignedInventorySlot.AssignItem(objectItem.AssignedInventorySlot);
            clickedUISlot.UpdateUISlot();
            
            objectItem.ClearSlot();
            return;
        }
        
        
           // Are both items the same? If so cobine them.
              // Is the slot stack size + mouse stack size > the slot Max Stack Size? If so, take from mouse.
           // If different items, then swap the items.
        
        // Both slots have an item - decide what to do..
        if (clickedUISlot.AssignedInventorySlot.ItemData != null && objectItem.AssignedInventorySlot.ItemData != null)
        {
            bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemData == objectItem.AssignedInventorySlot.ItemData;
            
            // Are both items the same? If so combine them.
            if (isSameItem && clickedUISlot.AssignedInventorySlot.EnougRoomLeftInStack(objectItem.AssignedInventorySlot.StackSize))
            {
                clickedUISlot.AssignedInventorySlot.AssignItem(objectItem.AssignedInventorySlot);
                clickedUISlot.UpdateUISlot();
                
                objectItem.ClearSlot();
                return;
            }
            else if (isSameItem &&
                     !clickedUISlot.AssignedInventorySlot.EnougRoomLeftInStack(objectItem.AssignedInventorySlot.StackSize,
                         out int leftInStack))
            {
                if(leftInStack < 1) SwapSlots(clickedUISlot); // Stack is full so swap the items.
                else // Slot is not at max, so take what's need from the mouse inventory.
                {
                    int remainingOnMouse = objectItem.AssignedInventorySlot.StackSize - leftInStack;
                    
                    clickedUISlot.AssignedInventorySlot.AddToStack(leftInStack);
                    clickedUISlot.UpdateUISlot();

                    var newItem = new InventorySlot(objectItem.AssignedInventorySlot.ItemData, remainingOnMouse);
                    
                    objectItem.ClearSlot();
                    objectItem.UpdateObjectSlot(newItem);
                    return;
                }
            }
            else if (!isSameItem)
            {
                SwapSlots(clickedUISlot);
                return;
            }
        }
    }

    private void SwapSlots(InventorySlot_UI clickedUISlot)
    {
        var cloneSlot = new InventorySlot(objectItem.AssignedInventorySlot.ItemData,
            objectItem.AssignedInventorySlot.StackSize);
        objectItem.ClearSlot();
        
        objectItem.UpdateObjectSlot(clickedUISlot.AssignedInventorySlot);
        
        clickedUISlot.ClearSlot();
        clickedUISlot.AssignedInventorySlot.AssignItem(cloneSlot);
        clickedUISlot.UpdateUISlot();
    }
}
