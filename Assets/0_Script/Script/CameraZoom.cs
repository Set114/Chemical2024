using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera mainCamera; // �A�n�����v��
    public float zoomSpeed = 1f; // ��j�t��
    public float minZoom = 10f; // �̤p������
    public float maxZoom = 60f; // �̤j������
    public float targetDistance = 20f; // �ؼжZ��

    void Update()
    {
        // ���ƹL���ؼжZ��
        float currentDistance = Vector3.Distance(mainCamera.transform.position, transform.position);
        float newDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * zoomSpeed);
        newDistance = Mathf.Clamp(newDistance, minZoom, maxZoom);

        // �]�w�s����v����m
        mainCamera.transform.position = transform.position - mainCamera.transform.forward * newDistance;
    }
}
