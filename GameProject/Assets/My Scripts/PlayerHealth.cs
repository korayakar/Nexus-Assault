using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    private GameObject player;
    public GameObject Player { get => player; }
    private CharacterController cc;

    [Header("Health Bar")]
    public float maxHealth = 100f;
    public float chipSpeed = 2f;

    [Header("UI")]
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI deadText;

    [Header("Damage Overlay")]
    public Image overlay; //Blood overlay game object
    public float duration; 
    public float fadeSpeed;

    private float durationTimer;
    public Vector3 spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        player = GameObject.FindGameObjectWithTag("Player");
        cc = GetComponent<CharacterController>();
        deadText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            StartCoroutine(DisplayDeadText("You Died", 2));
            cc.enabled = false;
            player.transform.position = spawnPoint;
            cc.enabled = true;
            health += 5;
        }
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        if(overlay.color.a > 0)
        {
            if(health < 20)
            {
                return; // never stop fade process
            }
            durationTimer += Time.deltaTime;
            if(durationTimer > duration)
            {
                //fade image
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }
    }
    public void UpdateHealthUI()
    {
        //Debug.Log(health);
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float hFraction = health / maxHealth; // to be between 0 and 1

        if (fillBack > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, hFraction, percentComplete);
        }

        if (fillFront < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentComplete);
        }
        healthText.text = Mathf.Round(health) + "/" + Mathf.Round(maxHealth);
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0.6f);
    }
    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }

    public void IncreaseHealth(int level)
    {
        maxHealth += (health * 0.01f) * ((100 - level) * 0.1f);
        health = maxHealth;
    }

    IEnumerator DisplayDeadText(string message, float delay)
    {
        deadText.text = message;
        deadText.enabled = true;
        yield return new WaitForSeconds(delay);
        deadText.enabled = false;
    }
}
