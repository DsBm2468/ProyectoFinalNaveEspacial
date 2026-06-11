using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlanetsBehavior : MonoBehaviour
{
    public enum Planet { Sol, Mercurio, Venus, Tierra, Marte, Jupiter, Saturno, Urano, Neptuno, Luna}

    [Header("Orbit Settings")]
    public Transform CenterOfSystem;
    public Planet planetSelectioned; 
    public float OrbitalSpeed; // Indica que tan rápido gira el planeta al rededor del centro (sol)
    public float RotationalSpeedHimself; // Indica que tan rápido gira el planeta sobre si mísmo

    public static Planet SelectedPlanetGlobal = Planet.Tierra; // Variable estándar seleccionada

    void Start()
    {
        // Inicialmente, los planetas contarán con su respectiva rotación y orbita
        switch (planetSelectioned) 
        {
            case Planet.Sol:
                OrbitalSpeed = 0f;
                RotationalSpeedHimself = 2f;
                break;
            case Planet.Mercurio:
                OrbitalSpeed = 12f;
                RotationalSpeedHimself = 4f;
                break;
            case Planet.Venus:
                OrbitalSpeed = 9f;
                RotationalSpeedHimself = -3f;
                break;
            case Planet.Tierra:
                OrbitalSpeed = 7f;
                RotationalSpeedHimself = 15f;
                break;
            case Planet.Marte:
                OrbitalSpeed = 5f;
                RotationalSpeedHimself = 14f;
                break;
            case Planet.Jupiter:
                OrbitalSpeed = 3f;
                RotationalSpeedHimself = 30f;
                break;
            case Planet.Saturno:
                OrbitalSpeed = 2f;
                RotationalSpeedHimself = 26f;
                break;
            case Planet.Urano:
                OrbitalSpeed = 1.2f;
                RotationalSpeedHimself = -18f;
                break;
            case Planet.Neptuno:
                OrbitalSpeed = 0.8f;
                RotationalSpeedHimself = 16f;
                break;
            case Planet.Luna:
                OrbitalSpeed = 20f;
                RotationalSpeedHimself = 12f;
                break;
        }
    }

    void Update()
    {
        // Se detecta el valor de orbita actual, obtenido al multiplicar la velocidad de orbita inicial por el tiempo para que este movimiento sea sutil
        // Esto aplica un Movimiento Circular Uniforme (MCU), que indica que un objeto describe una trayectoria circular alrededor de un centro de masa manteniendo una distancia constante y una velocidad angular regular.
        float currentOrbit = OrbitalSpeed * Time.deltaTime; 
        transform.RotateAround(CenterOfSystem.position, Vector3.up, currentOrbit); // Se usa Vector3.up para que la rotación sea en horizontal (eje y)

        // Se detecta el valor de rotación actual sobre si mismo, obtenido al multiplicar la velocidad de la rotacion propia inicial por el tiempo para que este movimiento sea sutil
        // Esto aplica un Momento Angular Intrínseco, es una propiedad física que  indica que una partícula posee una rotación y un magnetismo propios e inherentes a su estructura, que se mantienen constantes sin importar su movimiento lineal en el espacio o la ausencia de fuerzas externas
        float currentOwnRotation = RotationalSpeedHimself * Time.deltaTime;
        transform.Rotate(Vector3.up, currentOwnRotation);
    }

    public void ChangeOrbitalSpeed(float newValue)
    {
        //if(planetSelectioned == Planet.Sol) return;

        PlanetsBehavior[] AllCB = Object.FindObjectsByType<PlanetsBehavior>(FindObjectsSortMode.None);
        foreach (PlanetsBehavior p in AllCB)
        {
            if (p.planetSelectioned == SelectedPlanetGlobal && p.planetSelectioned != Planet.Sol)
            {
                p.OrbitalSpeed = newValue;
            }
        }
    }

    public void ChangeOwnRotation(float newNumber)
    {
        //if (planetSelectioned == Planet.Sol) return;

        PlanetsBehavior[] AllCB = Object.FindObjectsByType<PlanetsBehavior>(FindObjectsSortMode.None);
        foreach (PlanetsBehavior p in AllCB)
        {
            if (p.planetSelectioned == SelectedPlanetGlobal && p.planetSelectioned != Planet.Sol)
            {
                p.RotationalSpeedHimself = newNumber;
            }
        }
    }

    // FUNCION PARA CAMBIO DE PLANETA SEGÚN EL 
    public void SelectPlanetFromDropdown(int opcion)
    {
        switch (opcion)
        {
            case 0: SelectedPlanetGlobal = Planet.Tierra; break;
            case 1: SelectedPlanetGlobal = Planet.Luna; break;
            case 2: SelectedPlanetGlobal = Planet.Marte; break;
            case 3: SelectedPlanetGlobal = Planet.Jupiter; break;
            case 4: SelectedPlanetGlobal = Planet.Saturno; break;
            case 5: SelectedPlanetGlobal = Planet.Venus; break;
            case 6: SelectedPlanetGlobal = Planet.Neptuno; break;
            case 7: SelectedPlanetGlobal = Planet.Mercurio; break;
            case 8: SelectedPlanetGlobal = Planet.Urano; break;
        }
        Debug.Log("Dropdown seleccionó el planeta enfocado: " + SelectedPlanetGlobal);
    }
}








 //float modifierBoost = 1; // Valor de los propulsores
        //if (Input.GetKey(KeyCode.Space)) // Si presionas la barra espaciadora (este comando es temporal)
        //{
        //    modifierBoost = 3f; // El valor de los propulsores será 3 veces más rápido
        //}
        // Se detecta el valor de orbita actual, obtenido al multiplicar la velocidad de orbita inicial por el valor de los propulsores
        //float currentOrbit = OrbitalSpeed * modifierBoost * Time.deltaTime;