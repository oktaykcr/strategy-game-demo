using UnityEngine;
using UnityEngine.UI;

public class ButtonViewModel : MonoBehaviour, IButton
{
    
    void Start()
    {
        var button = transform.GetComponent<Button>();
        button.onClick.AddListener(delegate () { this.ButtonClicked(); });
    }

    public void ButtonClicked()
    {
        if (GameBoardViewModel.Instance.IsProductSelected())
        {
            GameBoardViewModel.Instance.DeselectProduct();
        }

        var buttonName = gameObject.name;
        GameObject createdProduct = null;
        IProduct product;
        if (buttonName.Contains("Barrack"))
        {
            product = new Barrack("Barrack", ProductType.Building, true, 0.5f);
            createdProduct = ViewFactory.Create<IProduct, BuildingViewModel>(product, Resources.Load<GameObject>("Prefabs/Barrack"), null);
            createdProduct.name = "Barrack_Template";
        }

        if (buttonName.Contains("PowerPlant"))
        {
            product = new PowerPlant("Power Plant", ProductType.Building, true, 0.5f);
            createdProduct = ViewFactory.Create<IProduct, BuildingViewModel>(product, Resources.Load<GameObject>("Prefabs/PowerPlant"), null);
            createdProduct.name = "PowerPlant_Template";
        }
        GameBoardViewModel.Instance.SelectProduct(createdProduct);
    }
}
