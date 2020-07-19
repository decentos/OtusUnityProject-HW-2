using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    TextMesh textMesh;
    Health health;
    float displayedHealth;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        health = GetComponentInParent<Health>();
        displayedHealth = health.current - 1.0f; // гарантируем обновление текста в первом апдейте
    }

    // Update is called once per frame
    void Update()
    {
        float value = health.current;
        if (!Mathf.Approximately(displayedHealth, value)) {
            displayedHealth = value;
            textMesh.text = $"{value}";
        }
    }
}
