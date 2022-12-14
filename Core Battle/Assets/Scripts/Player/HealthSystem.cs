using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;


public class HealthSystem : MonoBehaviour
{
    //[SerializeField] float health = 100;
    [SerializeField] public float MaxHealth = 100.0f;
    [SerializeField] private float lerpTimer;
    [SerializeField] private float CurrentHealth;
    [SerializeField] private float chipSpeed = 2;
    [SerializeField] private float CurrentSavehp = 0;

    public Image frontHealthBar;
    public Image backHealthBar;

    public bool isDie = false;

    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragdoll;

    Animator animator;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        UpdateHealthUI();

        if(Inventory.instance.hillCheck)
        {
            Hill(10);
            Inventory.instance.hillCheck = false;
        }
    }

    private void FixedUpdate()
    {
        AreaControl();
    }

    public void TakeDamage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        animator.SetTrigger("damage");
        lerpTimer = 0f;

        if (CurrentHealth <= 0)
        {
            Die();

        }
    }


    private float deltaDamage = 0;
    public bool onzone = true;
    private void AreaControl()
    {
        Area a = Area.AREA();
        if (a != null && !onzone)
        {
            this.deltaDamage += Time.deltaTime;
            if (a.actualArea.MustDamage(this.deltaDamage))
            {
                this.deltaDamage = 0;
                this.TakeDamage(a.actualArea.Damage);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Area a = Area.AREA();
        if (a != null && other.gameObject == a.Cylinder.gameObject)
        {
            onzone = true;
            this.deltaDamage = 0;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Area a = Area.AREA();
        if (a != null && other.gameObject == a.Cylinder.gameObject)
        {
            onzone = false;
        }
    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = CurrentHealth / MaxHealth;
        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillB < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }

    }

    public void RestoreHealth(float healAmount)
    {
        CurrentHealth += healAmount;
        lerpTimer = 0f;
    }

    void Die()
    {
        isDie = true;

        Instantiate(ragdoll, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);
    }

    public void IncreaseHealth(int level)
    {
        CurrentSavehp = CurrentHealth;
        MaxHealth += (CurrentHealth * 0.01f) * ((100 - level) * 0.1f);
        CurrentHealth = MaxHealth;
        CurrentHealth -= CurrentSavehp;
    }

    public void Hill(int hill)
    {
        CurrentHealth += hill;
    }
}


