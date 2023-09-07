using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerTests
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayerTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        SceneManager.LoadScene("LevelRefactor", LoadSceneMode.Single);
        // yield return WaitForSceneLoad();
        yield return null;

        yield return VerifyPlayerWeapon();
        yield return VerifyPlayerShooting();
    }

    [UnityTest]
    public IEnumerator VerifyPlayerWeapon()
    {
        GameObject character = GameObject.Find("UpdatedPlayer");

        var scoop = character.GetComponent<PlayerScoopManager>();
        var weapon = scoop.GetWeapon();
        Assert.That(weapon, !Is.Null);
        yield return null;
    }

    [UnityTest]
    public IEnumerator VerifyPlayerShooting()
    {
        GameObject character = GameObject.Find("UpdatedPlayer");

        var scoop = character.GetComponent<PlayerScoopManager>();
        var inventory = character.GetComponent<IceCreamInventoryManager>();

        /* inventory.OnScoopAmmoChangedEvent += HandleScoopChanged;

        void HandleScoopChanged(object sender, ScoopAmmoChangedEventArgs e)
        {
            Assert.IsTrue(e.AmmoType == FlavorType.CHOCOLATE);
            Debug.Log(e.CurrentAmmoCount + "/ " + e.MaxAmmoCount);
            Assert.IsTrue(e.CurrentAmmoCount == e.MaxAmmoCount - 1);
            Assert.IsTrue(e.MaxAmmoCount == 10);
        }*/

        inventory.SwitchScoopToIndex(1); // Set to chocolate;
        var preShotCount = inventory.CurrentAmmo;

        var weapon = scoop.GetWeapon();

        weapon.HandleShootInputs(true, inventory);
        yield return new WaitForSeconds(5);

        Assert.IsTrue(inventory.CurrentAmmo < 10);        
    }

    private IEnumerator WaitForSceneLoad()
    {
        while (SceneManager.GetActiveScene().buildIndex > 0)
        {
            yield return null;
        }
    }
}
