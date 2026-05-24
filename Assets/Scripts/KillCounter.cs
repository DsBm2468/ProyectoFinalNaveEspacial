using TMPro;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    public TMP_Text textCounter;

    void Update()
    {
        if (
        GameManager.Instance
        == null)
            return;

        textCounter.text =
        "Objetivos: "
        +
        GameManager.Instance
        .enemiesAlive;
    }
}