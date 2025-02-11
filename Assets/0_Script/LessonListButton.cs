using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LessonListButton : MonoBehaviour
{
    [SerializeField] private Text lessonNameText;
    [SerializeField] private GameObject checkImage;
    [SerializeField] private Button button;

    public void SetLessonName(string name)
    {
        lessonNameText.text = name;
    }
    public void CheckLessonClear(bool CheckImage)
    {
        checkImage.SetActive(CheckImage);
        button.interactable = !CheckImage;
    }
}
