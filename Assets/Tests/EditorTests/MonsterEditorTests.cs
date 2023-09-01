using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MonsterEditorTests
{
    GameObject monster = Resources.Load<GameObject>("Prefabs/Monsters/BaseMonster");
    // A Test behaves as an ordinary method
    [Test]
    public void MonsterEditorTestsSimplePasses()
    {
        // Use the Assert class to test conditions
        TestMonster();

        monster = Resources.Load<GameObject>("Prefabs/Monsters/VanillaMonster");
        TestMonster();

        monster = Resources.Load<GameObject>("Prefabs/Monsters/BerryMonster");
        TestMonster();

        monster = Resources.Load<GameObject>("Prefabs/Monsters/MintMonster");
        TestMonster();
    }

    [Test]
    public void TestMonster()
    {
        TestMonsterExistance();
        TestMonsterHasComponents();
    }

    [Test]
    public void TestMonsterExistance()
    {
        Assert.That(monster, !Is.Null);
    }

    [Test]
    public void TestMonsterHasComponents()
    {
        // Assert player has all needed components
        var contoller = monster.GetComponentInChildren<MonsterController>();
        var state = monster.GetComponentInChildren<MonsterStateMachine>();
        var monsterType = monster.GetComponentInChildren<MonsterType>();
        var animationConroller = monster.GetComponentInChildren<MonsterAnimationController>();


        Assert.That(contoller, !Is.Null);
        Assert.That(state, !Is.Null);
        Assert.That(monsterType, !Is.Null);
        Assert.That(animationConroller, !Is.Null);
    }
}
