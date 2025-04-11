using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;

public class DestroyBlock : MonoBehaviour, IMixedRealityPointerHandler
{
    public LayerMask blockLayer; // Capa de los bloques

    void OnEnable()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);
    }

    void OnDisable()
    {
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityPointerHandler>(this);
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        if (eventData.Pointer != null)
        {
            Vector3 origin = eventData.Pointer.Position;    // Origen del rayo
            Vector3 direction = eventData.Pointer.Rotation * Vector3.forward; // Dirección corregida

            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, 10f, blockLayer))
            {
                Debug.Log("Destruyendo: " + hit.collider.name);
                Destroy(hit.collider.gameObject);
            }

            Debug.DrawRay(origin, direction * 10, Color.green, 2f);
        }
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData) { }
    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }
    public void OnPointerClicked(MixedRealityPointerEventData eventData) { }
}