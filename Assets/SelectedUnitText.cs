using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectedUnitText : MonoBehaviour
{

    public TextMeshProUGUI selectedUnitText;
    

    // Update is called once per frame
    void Update()
    {
        if(SelectedUnit.instance.unit != null)
        {
            selectedUnitText.text = SelectedUnit.instance.unit.getSelectedText();
        }
    }
}
