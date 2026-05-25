//using UnityEngine;
//using TMPro;

//public class SimulationHUD : MonoBehaviour
//{
//    [Header("UI")]
//    public TMP_Text statusText;

//    [Header("Configuración")]
//    public Vector3 gravityRef = new Vector3(0, -9.81f, 0);
//    public float groundY = 0f;     // referencia para energía potencial

//    private void OnEnable()
//    {
//        if (SimulationManager.Instance != null)
//            SimulationManager.Instance.OnSimulationStep += Refresh;
//    }

//    private void OnDisable()
//    {
//        if (SimulationManager.Instance != null)
//            SimulationManager.Instance.OnSimulationStep -= Refresh;
//    }

//    private void Refresh(float dt)
//    {
//        var sim = SimulationManager.Instance;
//        int count = ParticleWorld.All.Count;

//        // Energías acumuladas del sistema completo
//        float kinetic = 0f;
//        float potential = 0f;
//        float gMag = gravityRef.magnitude;

//        foreach (var p in ParticleWorld.All)
//        {
//            float v2 = p.Velocity.sqrMagnitude;
//            kinetic += 0.5f * p.Mass * v2;

//            float h = p.Position.y - groundY;
//            potential += p.Mass * gMag * h;
//        }

//        float total = kinetic + potential;
//        string state = sim.isPaused ? "PAUSED" : "RUNNING";

//        statusText.text =
//            $"[ {state} ]\n" +
//            $"t = {sim.SimulationTime:F2} s   step #{sim.StepCount}\n" +
//            $"Δt = {sim.updateTime * 1000f:F1} ms   timeScale = {sim.timeScale:F2}x\n" +
//            $"\n" +
//            $"Partículas: {count}\n" +
//            $"Ek = {kinetic:F1} J\n" +
//            $"Ep = {potential:F1} J\n" +
//            $"E  = {total:F1} J";
//    }
//}