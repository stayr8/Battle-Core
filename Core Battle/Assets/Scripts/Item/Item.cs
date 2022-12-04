using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    public int hpHill;
    public int mpHill;

    public ItementSlot itemSlot;

    public virtual void Use()
    {
        Debug.Log("Using " + name);
    }


}

public enum ItementSlot { Hppotion, Mppotion }
