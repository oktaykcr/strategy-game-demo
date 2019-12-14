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
        var gameObjectName = gameObject.name;
        IProduct product;
        if (gameObjectName.Contains("Barrack"))
        {
            product = new Barrack("Barrack", ProductType.Building, true, 0.5f);
            var created = ViewFactory.Create<IProduct, BuildingViewModel>(product, Resources.Load<GameObject>("Prefabs/Barrack"), null);
            created.name = "Barrack_Template";
        }

        if (gameObjectName.Contains("PowerPlant"))
        {
            product = new PowerPlant("Power Plant", ProductType.Building, true, 0.5f);
            var created = ViewFactory.Create<IProduct, BuildingViewModel>(product, Resources.Load<GameObject>("Prefabs/PowerPlant"), null);
            created.name = "PowerPlant_Template";
        }
    }
}
