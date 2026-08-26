using UnityEngine;
using UnityEngine.InputSystem;

public class EffectSpawner : MonoBehaviour
{
    public GameObject effectPrefab;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }
    }
}