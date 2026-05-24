using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Slider healthSlider;

    public void UpdateHealth(
    int current,
    int max)
    {
        healthSlider.value =
        (float)current /
        max;
    }
}