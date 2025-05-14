using UnityEngine;

public class UIFollowTarget : MonoBehaviour
{
    public RectTransform targetUI;  // BUI
    public float moveSpeed = 5f;    // ���ʳt��

    private RectTransform selfRect;

    void Start()
    {
        selfRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (targetUI == null) return;

        // �p��q AUI ��m���ʨ� BUI ���t�Z�]anchoredPosition�^
        Vector2 currentPos = selfRect.anchoredPosition;
        Vector2 targetPos = targetUI.anchoredPosition;

        // ���Ʋ���
        selfRect.anchoredPosition = Vector2.Lerp(currentPos, targetPos, Time.deltaTime * moveSpeed);
    }
}
