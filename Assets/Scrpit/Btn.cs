using UnityEngine;
using UnityEngine.UI;

public enum btnType
{
    Sound
}

public class Btn : MonoBehaviour
{
    public btnType btnType;
    public Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();

        switch(btnType)
        {
            case btnType.Sound:
                btn.onClick.AddListener(SfxManager.instance.SfxSettingON);
                break;
        }
    }
}
