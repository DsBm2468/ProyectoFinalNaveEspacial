using UnityEngine;
using TMPro;

public class SpeedUI : MonoBehaviour
{
    public TMP_Text speedText;

    public PlayerMovement player;

    void Update()
    {
        if (
        player == null)
            return;

        float speed =
        player.GetCurrentSpeed();

        bool boosting =
        speed >
        player.baseSpeed
        + 1;

        speedText.text =
        "VEL: "
        +
        Mathf.RoundToInt(
        speed)
        +
        "\nBOOST: "
        +
        (
        boosting
        ?
        "ON"
        :
        "OFF");
    }
}