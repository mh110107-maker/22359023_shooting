using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Health playerHealth;
    [SerializeField] Image maxHealthBar;
    [SerializeField] Image currntHealthBar;
    // Start is called before the first frame update
    void Start()
    {
        maxHealthBar.fillAmount = playerHealth.currentHealth / 10f;
    }

    // Update is called once per frame
    void Update()
    {
        currntHealthBar.fillAmount = playerHealth.currentHealth / 10f;
    }
}
