using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//BlackJack and Hookers
public class UI_Buttons : MonoBehaviour
{
    [Range(1, 4)]
    public int mButtonCount = 1;
    public GameObject mButtonPreFab = null;

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
        for(int i = 0; i < mButtonCount;i++)
        {
            GameObject newButton = Instantiate(mButtonPreFab);
            newButton.transform.SetParent(transform);

            string drawingMode = "Mode" + (i + 1).ToString();
            newButton.GetComponentInChildren<Text>().text = drawingMode;
            newButton.name = drawingMode;

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
        }
    }
}
