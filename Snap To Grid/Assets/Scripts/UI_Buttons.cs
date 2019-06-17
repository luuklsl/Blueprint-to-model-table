using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System.Text.RegularExpressions;

//BlackJack and Hookers
public class UI_Buttons : MonoBehaviour
{
    public GameObject mButtonPreFab = null;
    public GameObject[] drawingModes = null;

    private ToggleGroup mToggleGroup = null;
    // Start is called before the first frame update
    void Awake()
    {
        mToggleGroup = GetComponent<ToggleGroup>();
        CreateButtons();
    }

    // Update is called once per frame
    private void CreateButtons()
    {
        for (int i = 0; i < drawingModes.Length; i++)
        {
            GameObject newButton = Instantiate(mButtonPreFab);
            newButton.transform.SetParent(transform);

            string drawingModeName = drawingModes[i].name;
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
            for (int i = 0; i < drawingModes.Length; i++)
            {
                if (toggle.gameObject.name == drawingModes[i].name)
                {
                    drawingModes[i].SetActive(true);
                }
                else
                {
                    drawingModes[i].SetActive(false);
                }
            }
        }
    }
}
