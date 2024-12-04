using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngameViewPanel : MonoBehaviour, IPointerClickHandler
{
    private RawImage _image;
    [SerializeField] private GameObject testObject;
    [SerializeField] private Camera ingameCamera;
    [SerializeField] private LayerMask rayLayerMask;

    private Vector3[] v = new Vector3[4];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _image = GetComponent<RawImage>();

        Debug.LogWarning("IngameViewPanel Rect");
        // Debug.Log(_image.rectTransform.anchoredPosition);

        _image.rectTransform.GetWorldCorners(v);
        Debug.Log(v[0]);
        Debug.Log(v[1]);
        Debug.Log(v[2]);
        Debug.Log(v[3]);
    }

    private void OnRectTransformDimensionsChange()
    {
        _image.rectTransform.GetWorldCorners(v);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.LogWarning("OnPointerClick");
        ingameCamera.ScreenPointToRay(GetIngameCamPosition(eventData.position));
        Physics.Raycast(ingameCamera.ScreenPointToRay(GetIngameCamPosition(eventData.position)), out var hitInfo);
        Debug.Log(1 << hitInfo.collider.gameObject.layer);
        Debug.Log(rayLayerMask.value);
        if ((1 << hitInfo.transform.gameObject.layer & rayLayerMask.value) > 0)
            testObject.transform.position = hitInfo.point;
        // Debug.Log(eventData.pointerCurrentRaycast.worldPosition);
        // Debug.Log(eventData.pointerCurrentRaycast.screenPosition);
        // Debug.Log(eventData.pointerPressRaycast.worldPosition);
        // Debug.Log(eventData.pointerPressRaycast.screenPosition);
        // Debug.Log(eventData.position);
        // Debug.Log(eventData.pressPosition);
    }

    private Vector3 GetIngameCamPosition(Vector3 origin)
    {
        origin -= v[0];
        var size = (v[2] - v[0]);
        var x = origin.x / size.x * ingameCamera.pixelWidth;
        var y = origin.y / size.y * ingameCamera.pixelHeight;
        // origin /= (v[1] - v[0]);
        return new Vector3(x, y, 0);
    }
}