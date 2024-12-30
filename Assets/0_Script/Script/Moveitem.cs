using UnityEngine;

public class Moveitem : MonoBehaviour
{
    private Vector3 _offset;
    private float _zCoord;

    public Type moveType;
    public Transform targetPoint; // 特定地点
    public float lockingDistance = 1f; // 靠近距离

    public enum Type
    {
        normal,
        XAxis,
        YAxis,
        ZAxis,
        XYPlane,
        YZPlane,
        ZXPlane
    }

    private void OnMouseDown()
    {
        _zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        _offset = transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = _zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        Vector3 newPos = new Vector3(0, 0, 0);
        Quaternion newRot = transform.rotation;
        switch (moveType)
        {
            case Type.normal:
                newPos = GetMouseWorldPos() + _offset;
                break;
            case Type.XAxis:
                newPos.x = GetMouseWorldPos().x + _offset.x;
                newPos.y = transform.position.y;
                newPos.z = transform.position.z;
                break;
            case Type.YAxis:
                newPos.x = transform.position.x;
                newPos.y = GetMouseWorldPos().y + _offset.y;
                newPos.z = transform.position.z;
                break;
            case Type.ZAxis:
                newPos.x = transform.position.x;
                newPos.y = transform.position.y;
                newPos.z = GetMouseWorldPos().z + _offset.z;
                break;
            case Type.XYPlane:
                newPos.x = GetMouseWorldPos().x + _offset.x;
                newPos.y = GetMouseWorldPos().y + _offset.y;
                newPos.z = transform.position.z;
                break;
            case Type.YZPlane:
                newPos.x = transform.position.x;
                newPos.y = GetMouseWorldPos().y + _offset.y;
                newPos.z = GetMouseWorldPos().z + _offset.z;
                break;
            case Type.ZXPlane:
                newPos.x = GetMouseWorldPos().x + _offset.x;
                newPos.y = transform.position.y;
                newPos.z = GetMouseWorldPos().z + _offset.z;
                break;
        }

        // 判断物体是否靠近目标点
        if (Vector3.Distance(newPos, targetPoint.position) <= lockingDistance)
        {
            newPos = targetPoint.position; // 锁定到目标点
            newRot = targetPoint.rotation;
        }

        transform.position = newPos;
        transform.rotation = newRot;
    }
}
