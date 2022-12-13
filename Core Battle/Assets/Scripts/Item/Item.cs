using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public enum ItemType // 아이템 유형
    {
        Equipment,
        Used,
        Ingredient,
        Potion
    }

    new public string name = "New Item";
    public ItemType itmeType;
    public Sprite icon = null;
    public GameObject itemPrefab;
    public bool isDefaultItem = false;
    

    public int hpHill;
    public int mpHill;

    

    public virtual void Use()
    {
        Debug.Log("Using " + name);
    }


}

