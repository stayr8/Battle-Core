using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public enum ItemType // 아이템 유형
    {
        Equipment,
        Used,
        Ingredient,
        Potion,
        Skill_1,
        Skill_2,
        Skill_3,
        Skill_4
    }

    new public string name = "New Item";
    public ItemType itmeType;
    public Sprite icon = null;
    public GameObject itemPrefab;
    public bool isDefaultItem = false;

    public bool hp = false;
    public bool Skill_1 = false;
    public bool Skill_2 = false;
    public bool Skill_3 = false;
    public bool Skill_4 = false;


    public int hpHill;
    public int mpHill;

    

    public virtual void Use()
    {
        Debug.Log("Using " + name);
    }


}

