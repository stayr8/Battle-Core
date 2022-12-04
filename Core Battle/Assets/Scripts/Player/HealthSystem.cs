using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    //[SerializeField] float health = 100;
    [SerializeField] private float MaxHealth = 100.0f;
    [SerializeField] private float lerpTimer;
    [SerializeField] private float CurrentHealth;
    [SerializeField] private float chipSpeed = 2;

    public Image frontHealthBar;
    public Image backHealthBar;

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

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = CurrentHealth / MaxHealth;
        if(fillB > hFraction)
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
        MaxHealth += (CurrentHealth * 0.01f) * ((100 - level) * 0.1f);
        CurrentHealth = MaxHealth;
    }
}
