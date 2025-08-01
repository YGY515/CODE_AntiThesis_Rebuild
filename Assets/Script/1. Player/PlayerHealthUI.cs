using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth status;

    public GameObject heart0;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    void OnEnable()
    {
        if (status == null)
        {
            status = FindObjectOfType<PlayerHealth>();
        }
        status.HealthChange += UpdateHealth;
    }

    void OnDisable()
    {
        status.HealthChange -= UpdateHealth;
    }

    public void UpdateHealth(int currentHealth)
    {
        heart0.SetActive(false);
        heart1.SetActive(false);
        heart2.SetActive(false);
        heart3.SetActive(false);

        
        switch (currentHealth)
        {
            case 3:
                heart3.SetActive(true);
                break;
            case 2:
                heart2.SetActive(true);
                break;
            case 1:
                heart1.SetActive(true);
                break;
            default:
                heart0.SetActive(true);
                break;
        }
    }
}