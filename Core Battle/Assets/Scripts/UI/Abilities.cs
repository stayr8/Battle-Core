using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    public PlayerCtr _playerCtr;

    [Header("Ability 1")]
    public Image abilityImage1;
    public float cooldown1 = 5;
    bool isCooldown = false;
    public KeyCode ability1;

    //Avility 1 Input Variables
    Vector3 position;
    public Canvas ability1Canvas;
    public Image skillshot;
    public Transform player;

    [Header("Ability 2")]
    public Image abilityImage2;
    public float cooldown2 = 5;
    bool isCooldown2 = false;
    public KeyCode ability2;

    //Avility 2 Input Variables
    Vector3 position2;
    public Canvas ability1Canvas2;
    public Image skillshot2;
    public Transform player2;

    [Header("Ability 3")]
    public Image abilityImage3;
    public float cooldown3 = 5;
    bool isCooldown3 = false;
    public KeyCode ability3;

    //Avility 3 Input Variables
    public Image targetCircle;
    public Image indicatorRangeCircle;
    public Canvas ability2Canvas;
    private Vector3 posUp;
    public float maxAbility2Distance;

    [Header("Ability 4")]
    public Image abilityImage4;
    public float cooldown4 = 5;
    bool isCooldown4 = false;
    public KeyCode ability4;

    //Avility 4 Input Variables
    Vector3 position4;
    public Canvas ability1Canvas4;
    public Image skillshot4;
    public Transform player4;

    Item item;


    // Start is called before the first frame update
    void Start()
    {
        abilityImage1.fillAmount = 0;
        abilityImage2.fillAmount = 0;
        abilityImage3.fillAmount = 0;
        abilityImage4.fillAmount = 0;

        skillshot.GetComponent<Image>().enabled = false;
        skillshot2.GetComponent<Image>().enabled = false;
        targetCircle.GetComponent<Image>().enabled = false;
        indicatorRangeCircle.GetComponent<Image>().enabled = false; 
        skillshot4.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Ability1();
        Ability2();
        Ability3();
        Ability4();
        /*
        if (Inventory.instance.Skill_1)
        {
            Ability1();
        }
        if (Inventory.instance.Skill_2)
        {
            Ability2();
        }
        if (Inventory.instance.Skill_3)
        {
            Ability3();
        }
        if (Inventory.instance.Skill_4)
        {
            Ability4();
        }*/

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Ability 1 Inputs
        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
        //Ability 2 Inputs
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            position2 = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
        //Ability 3 Inputs
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if(hit.collider.gameObject != this.gameObject)
            {
                posUp = new Vector3(hit.point.x, 10f, hit.point.z);
                position = hit.point;
            }
        }
        //Ability 4 Inputs
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            position4 = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }

        //Ability 1 Canvas Inputs
        Vector3 rot = new Vector3(position.x - player.transform.position.x, 0f, position.z - player.transform.position.z);
        Quaternion transRot = Quaternion.LookRotation(rot);
        ability1Canvas.transform.rotation = Quaternion.Lerp(transRot, ability1Canvas.transform.rotation, 0f);

        //Ability 2 Canvas Inputs
        Vector3 rot2 = new Vector3(position.x - player.transform.position.x, 0f, position.z - player.transform.position.z);
        Quaternion transRot2 = Quaternion.LookRotation(rot2);
        ability2Canvas.transform.rotation = Quaternion.Lerp(transRot2, ability1Canvas.transform.rotation, 0f);

        //Ability 3 Canvas Inputs
        var hitPosDir = (hit.point - transform.position).normalized;
        float distance = Vector3.Distance(hit.point, transform.position);
        distance = Mathf.Min(distance, maxAbility2Distance);

        var newHitPos = transform.position + hitPosDir * distance;
        ability2Canvas.transform.position = (newHitPos);

        //Ability 4 Canvas Inputs
        Vector3 rot4 = new Vector3(position.x - player.transform.position.x, 0f, position.z - player.transform.position.z);
        Quaternion transRot4 = Quaternion.LookRotation(rot4);
        ability2Canvas.transform.rotation = Quaternion.Lerp(transRot4, ability1Canvas.transform.rotation, 0f);
    }

    void Ability1()
    {
        if (Input.GetKey(ability1) && isCooldown == false)
        {
            skillshot.GetComponent<Image>().enabled = true;

            //Disable Other UI = 다른 스킬 UI 끄기
            indicatorRangeCircle.GetComponent<Image>().enabled = false;
            targetCircle.GetComponent<Image>().enabled = false;
            skillshot2.GetComponent<Image>().enabled = false;
            skillshot4.GetComponent<Image>().enabled = false;
        }
        
        if(skillshot.GetComponent<Image>().enabled == true && Input.GetMouseButton(0))
        {
            isCooldown = true;
            abilityImage1.fillAmount = 1;
            _playerCtr.skill_1 = true;
        }
        else if(skillshot.GetComponent<Image>().enabled == true && Input.GetMouseButton(1))
        {
            isCooldown = false;
            skillshot.GetComponent<Image>().enabled = false;
            //abilityImage1.fillAmount = 1;
            _playerCtr.skill_1 = false;
        }

        if(isCooldown)
        {
            abilityImage1.fillAmount -= 1 / cooldown1 * Time.deltaTime;
            skillshot.GetComponent<Image>().enabled = false;

            if (abilityImage1.fillAmount <= 0)
            {
                abilityImage1.fillAmount = 0;
                isCooldown = false;
            }
        }
    }

    void Ability2()
    {
        if (Input.GetKey(ability2) && isCooldown2 == false)
        {
            skillshot2.GetComponent<Image>().enabled = true;

            //Disable Other UI = 다른 스킬 UI 끄기
            indicatorRangeCircle.GetComponent<Image>().enabled = false;
            targetCircle.GetComponent<Image>().enabled = false;
            skillshot.GetComponent<Image>().enabled = false;
            skillshot4.GetComponent<Image>().enabled = false;
        }

        if (skillshot2.GetComponent<Image>().enabled == true && Input.GetMouseButton(0))
        {
            isCooldown2 = true;
            abilityImage2.fillAmount = 1;
            _playerCtr.skill_2 = true;
        }
        else if (skillshot2.GetComponent<Image>().enabled == true && Input.GetMouseButton(1))
        {
            isCooldown2 = false;
            skillshot2.GetComponent<Image>().enabled = false;
            //abilityImage1.fillAmount = 1;
            _playerCtr.skill_2 = false;
        }

        if (isCooldown2)
        {
            abilityImage2.fillAmount -= 1 / cooldown2 * Time.deltaTime;
            skillshot2.GetComponent<Image>().enabled = false;

            if (abilityImage2.fillAmount <= 0)
            {
                abilityImage2.fillAmount = 0;
                isCooldown2 = false;
            }
        }
    }

    void Ability3()
    {
        if (Input.GetKey(ability3) && isCooldown3 == false)
        {
            indicatorRangeCircle.GetComponent<Image>().enabled = true;
            targetCircle.GetComponent<Image>().enabled = true;

            //Disable Other UI = 다른 스킬 UI 끄기
            skillshot.GetComponent<Image>().enabled = false;
            skillshot2.GetComponent<Image>().enabled = false;
            skillshot4.GetComponent<Image>().enabled = false;
        }

        if (targetCircle.GetComponent<Image>().enabled == true && Input.GetMouseButton(0))
        {
            isCooldown3 = true;
            abilityImage3.fillAmount = 1;
            _playerCtr.skill_3 = true;
        }
        else if (skillshot2.GetComponent<Image>().enabled == true && Input.GetMouseButton(1))
        {
            isCooldown3 = false;
            indicatorRangeCircle.GetComponent<Image>().enabled = false;
            targetCircle.GetComponent<Image>().enabled = false;
            //abilityImage1.fillAmount = 1;
            _playerCtr.skill_3 = false;
        }

        if (isCooldown3)
        {
            abilityImage3.fillAmount -= 1 / cooldown3 * Time.deltaTime;
            indicatorRangeCircle.GetComponent<Image>().enabled = false;
            targetCircle.GetComponent<Image>().enabled = false;

            if (abilityImage3.fillAmount <= 0)
            {
                abilityImage3.fillAmount = 0;
                isCooldown3 = false;
            }
        }
    }

    void Ability4()
    {
        if (Input.GetKey(ability4) && isCooldown4 == false)
        {
            skillshot4.GetComponent<Image>().enabled = true;

            //Disable Other UI = 다른 스킬 UI 끄기
            indicatorRangeCircle.GetComponent<Image>().enabled = false;
            targetCircle.GetComponent<Image>().enabled = false;
            skillshot.GetComponent<Image>().enabled = false;
            skillshot2.GetComponent<Image>().enabled = false;
        }

        if (skillshot4.GetComponent<Image>().enabled == true && Input.GetMouseButton(0))
        {
            isCooldown4 = true;
            abilityImage4.fillAmount = 1;
            _playerCtr.skill_4 = true;
        }
        else if (skillshot4.GetComponent<Image>().enabled == true && Input.GetMouseButton(1))
        {
            isCooldown4 = false;
            skillshot4.GetComponent<Image>().enabled = false;
            //abilityImage1.fillAmount = 1;
            _playerCtr.skill_4 = false;
        }

        if (isCooldown4)
        {
            abilityImage4.fillAmount -= 1 / cooldown4 * Time.deltaTime;
            skillshot4.GetComponent<Image>().enabled = false;

            if (abilityImage4.fillAmount <= 0)
            {
                abilityImage4.fillAmount = 0;
                isCooldown4 = false;
            }
        }
    }
}
