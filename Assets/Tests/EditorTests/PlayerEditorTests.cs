using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class PlayerEditorTests
{
     // Use the Assert class to test conditions
        GameObject character = Resources.Load<GameObject>("Prefabs/UpdatedPlayer");
   // A Test behaves as an ordinary method
    [Test]
    public void PlayerTestsSimplePasses()
    {
        TestPlayerExistance();
        TestPlayerHasComponents();
        TestPlayerHasStartAmmo();
        TestPlayerSwitchAmmo();
    }

    [Test]
    public void TestPlayerExistance()
    {
        Assert.That(character, !Is.Null);
    }

    [Test]
    public void TestPlayerHasComponents()
    {
        // Assert player has all needed components
        var scoopManager = character.GetComponent<PlayerScoopManager>();
        var inventory = character.GetComponent<IceCreamInventoryManager>();
        Assert.That(scoopManager, !Is.Null);
        Assert.That(inventory, !Is.Null);
    }

    [Test]
    public void TestPlayerHasStartAmmo()
    {
        var inventory = character.GetComponent<IceCreamInventoryManager>();
        inventory.Initalize();

        var chocolate = inventory.GetAmmoCountByFlavor(FlavorType.CHOCOLATE);
        var vanilla = inventory.GetAmmoCountByFlavor(FlavorType.VANILLA);
        var berry = inventory.GetAmmoCountByFlavor(FlavorType.BERRY);
        var mint = inventory.GetAmmoCountByFlavor(FlavorType.MINT);

        Assert.IsTrue(chocolate == 10);
        Assert.IsTrue(vanilla == 10);
        Assert.IsTrue(berry == 10);
        Assert.IsTrue(mint == 10);

    }

    [Test]
    public void TestPlayerSwitchAmmo()
    {        
        var inventory = character.GetComponent<IceCreamInventoryManager>();
        inventory.Initalize();

        inventory.SwitchScoopToIndex(1);
        Assert.IsTrue(inventory.CurrentPlayerType == FlavorType.CHOCOLATE);

        inventory.SwitchScoopToIndex(2);
        Assert.IsTrue(inventory.CurrentPlayerType == FlavorType.VANILLA);

        inventory.SwitchScoopToIndex(3);
        Assert.IsTrue(inventory.CurrentPlayerType == FlavorType.BERRY);

        inventory.SwitchScoopToIndex(4);
        Assert.IsTrue(inventory.CurrentPlayerType == FlavorType.MINT);
    }
}
