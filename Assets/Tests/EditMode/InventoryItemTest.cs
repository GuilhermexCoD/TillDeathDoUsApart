using NUnit.Framework;
using UnityEngine;

public class InventoryItemTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void IfInventoryItemWhenCastingThenReturnObjectOfClass()
    {
        //Arrange
        var go = new GameObject("interactable", typeof(RangedWeapon));
        IInteractable interactable = go.GetComponent<IInteractable>();

        InventoryItem inventoryItem = new InventoryItem()
        {
            interactable = interactable,
            quantity = 1
        };

        //Act
        var rangedWeapon = inventoryItem.CastTo<RangedWeapon>();

        //Assert
        Assert.IsTrue(rangedWeapon.GetType() == typeof(RangedWeapon));
    }

    [Test]
    public void IfInventoryItemWhenCastingToInvalidThenReturnNull()
    {
        //Arrange
        var go = new GameObject("interactable", typeof(RangedWeapon));
        IInteractable interactable = go.GetComponent<IInteractable>();

        InventoryItem inventoryItem = new InventoryItem()
        {
            interactable = interactable,
            quantity = 1
        };

        //Act
        var itemData = inventoryItem.CastTo<ItemData>();

        //Assert
        Assert.IsNull(itemData);
    }

    [Test]
    public void IfInventoryItemWhenIsTypeOfInvalidThenReturnNull()
    {
        //Arrange
        var go = new GameObject("interactable", typeof(RangedWeapon));
        IInteractable interactable = go.GetComponent<IInteractable>();

        InventoryItem inventoryItem = new InventoryItem()
        {
            interactable = interactable,
            quantity = 1
        };

        //Act
        var result = inventoryItem.IsType<ItemData>(out ItemData itemData);

        //Assert
        Assert.IsFalse(result);
        Assert.IsNull(itemData);
    }
}
