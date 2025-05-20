using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageTitleView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageName;
    [SerializeField] private Image stageImage;

    public void SetView(string sName, Sprite image)
    {
        stageName.text = sName;
        stageImage.sprite = image;
    }
}
