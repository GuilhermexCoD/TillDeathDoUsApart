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

    [Test]
    public void IfInventoryManagerWhenRemoveAllThenRemoveCorrectly()
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
        var interactableItemD = CreateInteractable(id2, stackable2);

        //Act
        inventoryManager.AddItem(interactableItemA);
        inventoryManager.AddItem(interactableItemB);
        inventoryManager.AddItem(interactableItemC);
        inventoryManager.AddItem(interactableItemD);

        inventoryManager.RemoveItem(id1);
        inventoryManager.RemoveAllItem(id2);

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 0);
        Assert.IsTrue(inventoryManager.FindItems(id1).Count() == 0);
        Assert.IsNull(inventoryManager.FindItem(id1));

        Assert.IsTrue(inventoryManager.FindItems(id2).Count() == 0);
        Assert.IsNull(inventoryManager.FindItem(id2));
    }

    [Test]
    public void IfInventoryManagerWhenTryConsumeLessThenTotalThenRemoveCorrectly()
    {
        //Arrange
        int id1 = 0;
        bool stackable1 = true;

        var inventoryManager = new InventoryManager();

        var interactableItemA = CreateInteractable(id1, stackable1);
        var interactableItemB = CreateInteractable(id1, stackable1);
        var interactableItemC = CreateInteractable(id1, stackable1);
        var interactableItemD = CreateInteractable(id1, stackable1);

        //Act
        inventoryManager.AddItem(interactableItemA);
        inventoryManager.AddItem(interactableItemB);
        inventoryManager.AddItem(interactableItemC);
        inventoryManager.AddItem(interactableItemD);

        var result = inventoryManager.TryConsumeItem(id1, 3, out int notConsumed);

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 1);
        Assert.IsTrue(inventoryManager.FindItem(id1).GetQuantity() == 1);
        Assert.IsTrue(result);
        Assert.IsTrue(notConsumed == 0);
    }

    [Test]
    public void IfInventoryManagerWhenTryConsumeExactlyAmountThenRemoveCorrectly()
    {
        //Arrange
        int id1 = 0;
        bool stackable1 = true;

        var inventoryManager = new InventoryManager();

        var interactableItemA = CreateInteractable(id1, stackable1);
        var interactableItemB = CreateInteractable(id1, stackable1);
        var interactableItemC = CreateInteractable(id1, stackable1);
        var interactableItemD = CreateInteractable(id1, stackable1);

        inventoryManager.AddItem(interactableItemA);
        inventoryManager.AddItem(interactableItemB);
        inventoryManager.AddItem(interactableItemC);
        inventoryManager.AddItem(interactableItemD);

        //Act
        var result = inventoryManager.TryConsumeItem(id1, 4, out int notConsumed);

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 0);
        Assert.IsNull(inventoryManager.FindItem(id1));
        Assert.IsTrue(result);
        Assert.IsTrue(notConsumed == 0);
    }

    [Test]
    public void IfInventoryManagerWhenTryConsumeMoreThenTotalForceConsumeFalseThenDontRemove()
    {
        //Arrange
        int id1 = 0;
        bool stackable1 = true;

        var inventoryManager = new InventoryManager();

        var interactableItemA = CreateInteractable(id1, stackable1);
        var interactableItemB = CreateInteractable(id1, stackable1);
        var interactableItemC = CreateInteractable(id1, stackable1);
        var interactableItemD = CreateInteractable(id1, stackable1);

        inventoryManager.AddItem(interactableItemA);
        inventoryManager.AddItem(interactableItemB);
        inventoryManager.AddItem(interactableItemC);
        inventoryManager.AddItem(interactableItemD);

        //Act
        var result = inventoryManager.TryConsumeItem(id1, 5, out int notConsumed);

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 1);
        Assert.IsTrue(inventoryManager.FindItem(id1).GetQuantity() == 4);
        Assert.IsFalse(result);
        Assert.IsTrue(notConsumed == 5);
    }

    [Test]
    public void IfInventoryManagerWhenTryConsumeMoreThenTotalForceConsumeTrueThenRemove()
    {
        //Arrange
        int id1 = 0;
        bool stackable1 = true;

        var inventoryManager = new InventoryManager();

        var interactableItemA = CreateInteractable(id1, stackable1);
        var interactableItemB = CreateInteractable(id1, stackable1);
        var interactableItemC = CreateInteractable(id1, stackable1);
        var interactableItemD = CreateInteractable(id1, stackable1);

        inventoryManager.AddItem(interactableItemA);
        inventoryManager.AddItem(interactableItemB);
        inventoryManager.AddItem(interactableItemC);
        inventoryManager.AddItem(interactableItemD);

        //Act
        var result = inventoryManager.TryConsumeItem(id1, 5, out int notConsumed, true);

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 0);
        Assert.IsNull(inventoryManager.FindItem(id1));
        Assert.IsTrue(result);
        Assert.IsTrue(notConsumed == 1);
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
