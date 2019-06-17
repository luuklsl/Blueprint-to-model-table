using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System.Text.RegularExpressions;

public class MaterialPicker : MonoBehaviour
{
    public GameObject mButtonPreFab = null;
    public Material[] mMaterial = null;

    private ToggleGroup mToggleGroup = null;

    public static Material selectedMaterial = null;
    // Start is called before the first frame update
    void Awake()
    {
        mToggleGroup = GetComponent<ToggleGroup>();
        CreateButtons();
    }

    // Update is called once per frame
    private void CreateButtons()
    {
        for (int i = 0; i < mMaterial.Length; i++)
        {
            GameObject newButton = Instantiate(mButtonPreFab);
            newButton.transform.SetParent(transform);

            string drawingModeName = mMaterial[i].name;
            newButton.GetComponentInChildren<Text>().text = drawingModeName;
            newButton.name = drawingModeName;
            //newButton.tag = "DrawingMode" + i.ToString(); ;

            Toggle toggle = newButton.GetComponent<Toggle>();
            toggle.group = mToggleGroup;

            toggle.onValueChanged.AddListener(PrintToggle);

            if (i == 0)
                toggle.isOn = true;
            Debug.Log("Create Button" + i);
        }


    }

    private void PrintToggle(bool value)
    {
        if (!value)
            return;

        List<Toggle> activeToggles = new List<Toggle>(mToggleGroup.ActiveToggles());

        foreach (Toggle toggle in activeToggles)
        {
            print(toggle.gameObject.name);

            for (int i = 0; i < mMaterial.Length; i++)
            {
                if (toggle.gameObject.name == mMaterial[i].name)
                {
                    selectedMaterial = mMaterial[i];
                }
            }
        }
    }
}
