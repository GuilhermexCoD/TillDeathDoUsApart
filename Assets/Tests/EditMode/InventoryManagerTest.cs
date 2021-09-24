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
        var inventoryManager = new InventoryManager();
        var interactableItem = new RangedWeapon();

        //Act
        inventoryManager.AddItem(interactableItem);

        //Assert
        Assert.IsTrue(inventoryManager.GetItemsCount() == 1);
        Assert.IsTrue(inventoryManager.FindItems<RangedWeapon>().Count() == 1);
    }
}
