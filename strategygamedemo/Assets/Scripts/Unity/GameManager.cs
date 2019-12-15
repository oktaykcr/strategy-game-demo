using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public enum Products
    {
        Barrack,
        PowerPlant,
        SoldierUnit
    }

    [SerializeField]
    private Text _alertText;

    #region Information Panel

    [Header("Information Panel")]

    [SerializeField]
    private List<Sprite> _productSprites;

    [SerializeField]
    private GameObject _informationPanel;

    [SerializeField]
    private GameObject _informationProductionPanel;

    [SerializeField]
    private Image _selectedProductImage;

    [SerializeField]
    private Text _selectedProductText;

    [SerializeField]
    private Button _selectedProductProductionButton;

    [SerializeField]
    private Text _selectedProductProductionText;

    #endregion

    /// <summary>
    /// Shows warning to player when the area is not suitable for build
    /// </summary>
    public void GiveNotSuitableAreaWarning()
    {
        if (_alertText.text.Equals(""))
        {
            StartCoroutine(ShowMessage("The area is not suitable for placing!"));
        }
    }

    public void GiveNotSuitableAreaForSpawnWarning()
    {
        if (_alertText.text.Equals(""))
        {
            StartCoroutine(ShowMessage("The area is not suitable for spawning a soldier!"));
        }
    }

    IEnumerator ShowMessage(String message)
    {
        _alertText.text = message;
        yield return new WaitForSeconds(2f);
        _alertText.text = "";
    }

    /// <summary>
    /// Shows information about products, the method using at after select a product
    /// </summary>
    /// <param name="products"></param>
    public void ShowProductInformation(Products products)
    {
        _informationPanel.SetActive(true);
        _selectedProductText.text = products.ToString();
        switch (products)
        {
            case Products.Barrack:
                _informationProductionPanel.SetActive(true);
                _selectedProductImage.sprite = _productSprites[0];
                _selectedProductProductionButton.image.sprite = _productSprites[2];
                _selectedProductProductionButton.name = "SoldierUnitButton";
                _selectedProductProductionText.text = "Soldier Unit";
                break;
            case Products.PowerPlant:
                _selectedProductImage.sprite = _productSprites[1];
                if (_informationProductionPanel.activeSelf)
                    _informationProductionPanel.SetActive(false);
                break;
            case Products.SoldierUnit:
                _selectedProductImage.sprite = _productSprites[2];
                if (_informationProductionPanel.activeSelf)
                    _informationProductionPanel.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// Hides the information panels, the method using after deselect product
    /// </summary>
    public void HideProductInformation()
    {
        if(_informationPanel.activeSelf)
            _informationPanel.SetActive(false);
        if(_informationProductionPanel.activeSelf)
            _informationProductionPanel.SetActive(false);
    }

}
