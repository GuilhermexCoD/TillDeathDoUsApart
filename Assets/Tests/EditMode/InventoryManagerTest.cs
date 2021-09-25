using NUnit.Framework;
using UnityEngine;
using System.Linq;

public class InventoryManagerTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void IfInventoryManagerWhenAddingNewItemThenQuantityOne()
    {
        //Arrange
        int id = 0;
        var inventoryManager = new InventoryManager();
        var interactableItem = CreateInteractable(id, true);

        //Act
        inventoryManager.AddItem(interactableItem);

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 1);
        Assert.IsTrue(inventoryManager.FindItems(id).Count() == 1);
        Assert.IsTrue(inventoryManager.FindItem(id).quantity == 1);

    }

    // A Test behaves as an ordinary method
    [Test]
    public void IfInventoryManagerWhenAddingTwoStackablesThenQuantityOne()
    {
        //Arrange
        int id = 0;
        var inventoryManager = new InventoryManager();
        var interactableItemA = CreateInteractable(id, true);
        var interactableItemB = CreateInteractable(id, true);

        //Act
        inventoryManager.AddItem(interactableItemA);
        inventoryManager.AddItem(interactableItemB);

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 1);
        Assert.IsTrue(inventoryManager.FindItems(id).Count() == 1);
        Assert.IsTrue(inventoryManager.FindItem(id).quantity == 2);
    }

    [Test]
    public void IfInventoryManagerWhenAddingTwoNonStackablesThenQuantityTwo()
    {
        //Arrange
        int id = 0;
        var inventoryManager = new InventoryManager();
        var interactableItemA = CreateInteractable(id, false);
        var interactableItemB = CreateInteractable(id, false);

        //Act
        inventoryManager.AddItem(interactableItemA);
        inventoryManager.AddItem(interactableItemB);

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 2);
        Assert.IsTrue(inventoryManager.FindItems(id).Count() == 2);
        Assert.IsTrue(inventoryManager.FindItem(id).quantity == 1);
    }

    [Test]
    public void IfInventoryManagerWhenAddingThreeDifferentItensThenStackCorrectly()
    {
        //Arrange
        int id1 = 0;
        bool stackable1 = true;

        int id2 = 1;
        bool stackable2 = false;

        var inventoryManager = new InventoryManager();

        var interactableItemA = CreateInteractable(id1, stackable1);
        var interactableItemB = CreateInteractable(id1, stackable1);

        var interactableItemC = CreateInteractable(id2, stackable2);


        //Act
        inventoryManager.AddItem(interactableItemA);
        inventoryManager.AddItem(interactableItemB);
        inventoryManager.AddItem(interactableItemC);

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 2);
        Assert.IsTrue(inventoryManager.FindItems(id1).Count() == 1);
        Assert.IsTrue(inventoryManager.FindItem(id1).quantity == 2);

        Assert.IsTrue(inventoryManager.FindItems(id2).Count() == 1);
        Assert.IsTrue(inventoryManager.FindItem(id2).quantity == 1);
    }

    private static Interactable CreateInteractable(int id, bool stackable)
    {
        var interactableItem = new Interactable();

        var itemData = ScriptableObject.CreateInstance<ItemData>();
        itemData.id = id;
        itemData.stackable = stackable;
        interactableItem.SetData<ItemData>(itemData);

        return interactableItem;
    }
}
