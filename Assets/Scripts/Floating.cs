
using UnityEngine;
using UnityEngine.Serialization;

public class Floating : MonoBehaviour
{
    public FloatingPoint[] floatPoints;

    private void Awake() {
        floatPoints = GetComponentsInChildren<FloatingPoint>();
    }

    
}