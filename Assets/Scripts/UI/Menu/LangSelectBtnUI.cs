using UnityEngine;
using UnityEngine.UI;
public class LangSelectBtnUI : MonoBehaviour
{
    [SerializeField]
    private string selectedLang;
    [SerializeField]
    private Outline outline;
    private void DisableOutline()
    {
        outline.enabled = false;
    }
    private void EnableOutline()
    {
        outline.enabled = true;
    }
    public void Check()
    {
        if (PlayerPrefs.GetString("language") == selectedLang)
            EnableOutline();
        else
            DisableOutline();
    }
}