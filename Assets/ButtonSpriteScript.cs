using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSpriteScript : MonoBehaviour
{
    [SerializeField] Sprite[] buttonSelectedSprite;
    [SerializeField] Sprite[] BUttonUneselectedSprite;
    [SerializeField] Toggle[] toggle;

    public void ChangeSpriteduration(int index)
    {
        if(toggle[index].isOn)
        {
            toggle[index].image.sprite = buttonSelectedSprite[index];
            return;
        }
        else
        {
            toggle[index].image.sprite = BUttonUneselectedSprite[index];
        }
    }
}
