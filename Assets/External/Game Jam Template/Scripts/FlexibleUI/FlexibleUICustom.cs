using UnityEngine;
using UnityEngine.UI;

public class FlexibleUICustom : MonoBehaviour
{

    public FlexibleUIData flexibleUIData;

    void Awake()
    {
        Image image = GetComponent<Image>();
        image.color = flexibleUIData.background;
    }

}
