using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    [SerializeField]
    private Text _alertText;

    public void GiveNotSuitableAreaWarning()
    {
        if (_alertText.text.Equals(""))
        {
            StartCoroutine(ShowMessage("The area is not suitable for placing!"));
        }
    }

    IEnumerator ShowMessage(String message)
    {
        _alertText.text = message;
        yield return new WaitForSeconds(2f);
        _alertText.text = "";
    }

}
