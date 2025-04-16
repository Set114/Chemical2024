using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterFinished : MonoBehaviour
{
    public Image Tick;
    public Sprite TickSprite;
    private void OnEnable()
    {
        Tick.sprite = TickSprite;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
