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
        string id = "0";
        var inventoryManager = new InventoryManager();
        var interactableItem = CreateInteractable(id, true);

        //Act
        inventoryManager.AddItem(interactableItem);

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 1);
        Assert.IsTrue(inventoryManager.FindItems(id.GetHashCode()).Count() == 1);
        Assert.IsTrue(inventoryManager.FindItem(id.GetHashCode()).quantity == 1);

    }

    // A Test behaves as an ordinary method
    [Test]
    public void IfInventoryManagerWhenAddingTwoStackablesThenQuantityOne()
    {
        //Arrange
        string id = "0";
        var inventoryManager = new InventoryManager();
        var interactableItemA = CreateInteractable(id, true);
        var interactableItemB = CreateInteractable(id, true);

        //Act
        inventoryManager.AddItem(interactableItemA);
        inventoryManager.AddItem(interactableItemB);

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 1);
        Assert.IsTrue(inventoryManager.FindItems(id.GetHashCode()).Count() == 1);
        Assert.IsTrue(inventoryManager.FindItem(id.GetHashCode()).quantity == 2);
    }

    [Test]
    public void IfInventoryManagerWhenAddingTwoNonStackablesThenQuantityTwo()
    {
        //Arrange
        string id = "0";
        var inventoryManager = new InventoryManager();
        var interactableItemA = CreateInteractable(id, false);
        var interactableItemB = CreateInteractable(id, false);

        //Act
        inventoryManager.AddItem(interactableItemA);
        inventoryManager.AddItem(interactableItemB);

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 2);
        Assert.IsTrue(inventoryManager.FindItems(id.GetHashCode()).Count() == 2);
        Assert.IsTrue(inventoryManager.FindItem(id.GetHashCode()).quantity == 1);
    }

    [Test]
    public void IfInventoryManagerWhenAddingThreeDifferentItensThenStackCorrectly()
    {
        //Arrange
        string id1 = "0";
        bool stackable1 = true;

        string id2 = "1";
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
        Assert.IsTrue(inventoryManager.FindItems(id1.GetHashCode()).Count() == 1);
        Assert.IsTrue(inventoryManager.FindItem(id1.GetHashCode()).quantity == 2);

        Assert.IsTrue(inventoryManager.FindItems(id2.GetHashCode()).Count() == 1);
        Assert.IsTrue(inventoryManager.FindItem(id2.GetHashCode()).quantity == 1);
    }

    [Test]
    public void IfInventoryManagerWhenRemoveAllThenRemoveCorrectly()
    {
        //Arrange
        string id1 = "0";
        bool stackable1 = true;

        string id2 = "1";
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

        inventoryManager.RemoveItem(id1.GetHashCode());
        inventoryManager.RemoveAllItem(id2.GetHashCode());

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 0);
        Assert.IsTrue(inventoryManager.FindItems(id1.GetHashCode()).Count() == 0);
        Assert.IsNull(inventoryManager.FindItem(id1.GetHashCode()));

        Assert.IsTrue(inventoryManager.FindItems(id2.GetHashCode()).Count() == 0);
        Assert.IsNull(inventoryManager.FindItem(id2.GetHashCode()));
    }

    [Test]
    public void IfInventoryManagerWhenTryConsumeLessThenTotalThenRemoveCorrectly()
    {
        //Arrange
        string id1 = "0";
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

        var result = inventoryManager.TryConsumeItem(id1.GetHashCode(), 3, out int notConsumed);

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 1);
        Assert.IsTrue(inventoryManager.FindItem(id1.GetHashCode()).GetQuantity() == 1);
        Assert.IsTrue(result);
        Assert.IsTrue(notConsumed == 0);
    }

    [Test]
    public void IfInventoryManagerWhenTryConsumeExactlyAmountThenRemoveCorrectly()
    {
        //Arrange
        string id1 = "0";
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
        var result = inventoryManager.TryConsumeItem(id1.GetHashCode(), 4, out int notConsumed);

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 0);
        Assert.IsNull(inventoryManager.FindItem(id1.GetHashCode()));
        Assert.IsTrue(result);
        Assert.IsTrue(notConsumed == 0);
    }

    [Test]
    public void IfInventoryManagerWhenTryConsumeMoreThenTotalForceConsumeFalseThenDontRemove()
    {
        //Arrange
        string id1 = "0";
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
        var result = inventoryManager.TryConsumeItem(id1.GetHashCode(), 5, out int notConsumed);

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 1);
        Assert.IsTrue(inventoryManager.FindItem(id1.GetHashCode()).GetQuantity() == 4);
        Assert.IsFalse(result);
        Assert.IsTrue(notConsumed == 5);
    }

    [Test]
    public void IfInventoryManagerWhenTryConsumeMoreThenTotalForceConsumeTrueThenRemove()
    {
        //Arrange
        string id1 = "0";
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
        var result = inventoryManager.TryConsumeItem(id1.GetHashCode(), 5, out int notConsumed, true);

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 0);
        Assert.IsNull(inventoryManager.FindItem(id1.GetHashCode()));
        Assert.IsTrue(result);
        Assert.IsTrue(notConsumed == 1);
    }
    private static Interactable CreateInteractable(string id, bool stackable)
    {
        var interactableItem = new Interactable();

        var itemData = ScriptableObject.CreateInstance<ItemData>();
        itemData.name = id;
        itemData.stackable = stackable;
        interactableItem.SetData<ItemData>(itemData);

        return interactableItem;
    }
}
