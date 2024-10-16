using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpecialEffects
{
    Carrying
}


[System.Serializable]
public class Item 
{
    public SpecialEffects SpecialEffect;
    Type type;
    public int Id;

    public string Title;
    public string Description;

    public Item()
    {
        type = this.GetType();
    }

    public virtual void WhenRecieved(Player _p)
    {
        
    }

    public virtual void WhenShots() {

    }

    public virtual void OnCarrying()
    {

    }
}

public class ItemTheSadOnion : Item
{
    public override void WhenRecieved(Player _p)
    {
        Debug.Log("hey bitch");
        _p.AddShotSpeed(0.5f);
    }

    public override void OnCarrying() {
        // not implmented but in future im gonna make isaac cry
    }
}

public class ItemCricketsHead : Item
{
    public override void WhenRecieved(Player _p)
    {
        
    }
}

public class ActiveItem : Item
{
    public virtual void OnActiveItemUsed(Player _p)
    {

    }
}

public class YumHeart : ActiveItem
{
    public override void OnActiveItemUsed(Player _p)
    {
        _p.gameObject.GetComponent<Health>().HealHealth(2.0f);
    }
}