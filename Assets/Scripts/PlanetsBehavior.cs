using UnityEngine;

public class PlanetsBehavior : MonoBehaviour
{
    public enum Planet { Mercurio, Venus, Tierra, Marte, Jupiter, Saturno, Urano, Neptuno, Luna}

    [Header("Orbit Settings")]
    public Transform CenterOfSystem;
    public Planet planetSelectioned; 
    public float OrbitalSpeed; // Indica que tan rápido gira el planeta al rededor del centro (sol)
    public float RotationalSpeedHimself; // Indica que tan rápido gira el planeta sobre si mísmo

    void Start()
    {
        // Inicialmente, los planetas contarán con su respectiva rotación y orbita

        switch (planetSelectioned) 
        {
            case Planet.Mercurio:
                OrbitalSpeed = 60f;
                RotationalSpeedHimself = 10f;
                break;
            case Planet.Venus:
                OrbitalSpeed = 45f;
                RotationalSpeedHimself = -5f;
                break;
            case Planet.Tierra:
                OrbitalSpeed = 35f;
                RotationalSpeedHimself = 30f;
                break;
            case Planet.Marte:
                OrbitalSpeed = 25f;
                RotationalSpeedHimself = 28f;
                break;
            case Planet.Jupiter:
                OrbitalSpeed = 15f;
                RotationalSpeedHimself = 60f;
                break;
            case Planet.Saturno:
                OrbitalSpeed = 10f;
                RotationalSpeedHimself = 55f;
                break;
            case Planet.Urano:
                OrbitalSpeed = 6f;
                RotationalSpeedHimself = 40f;
                break;
            case Planet.Neptuno:
                OrbitalSpeed = 3f;
                RotationalSpeedHimself = 35f;
                break;
            case Planet.Luna:
                OrbitalSpeed = 50f;
                RotationalSpeedHimself = 30f;
                break;
        }
    }

    void Update()
    {
        if (CenterOfSystem == null) return;

        //float modifierBoost = 1; // Valor de los propulsores
        //if (Input.GetKey(KeyCode.Space)) // Si presionas la barra espaciadora (este comando es temporal)
        //{
        //    modifierBoost = 3f; // El valor de los propulsores será 3 veces más rápido
        //}
        // Se detecta el valor de orbita actual, obtenido al multiplicar la velocidad de orbita inicial por el valor de los propulsores
        //float currentOrbit = OrbitalSpeed * modifierBoost * Time.deltaTime;

        // Se detecta el valor de orbita actual, obtenido al multiplicar la velocidad de orbita inicial por el tiempo para que este movimiento sea sutil
        float currentOrbit = OrbitalSpeed * Time.deltaTime; 
        transform.RotateAround(CenterOfSystem.position, Vector3.up, currentOrbit); // Se usa Vector3.up para que la rotación sea en horizontal (eje y)

        // Se detecta el valor de rotación actual sobre si mismo, obtenido al multiplicar la velocidad de la rotacion propia inicial por el tiempo para que este movimiento sea sutil
        float currentOwnRotation = RotationalSpeedHimself * Time.deltaTime;
        transform.Rotate(Vector3.up, currentOwnRotation);
    }
}
