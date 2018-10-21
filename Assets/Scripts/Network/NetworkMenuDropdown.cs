using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class NetworkMenuDropdown : MonoBehaviour {

    private Dropdown networkDropwork;
    public GameObject panelLan;
    public GameObject panelMatchMaker;
    private BoardNetworkConfiguration configuration;

    void Start()
    {

        networkDropwork = GetComponent<Dropdown>();
        
        List<Dropdown.OptionData> optionDataList = new List<Dropdown.OptionData>
        {
            new Dropdown.OptionData("Select a Network Mode ...")
        };

        for (int position = 0; position < NetworkModeOptions.options.Length; position++)
        {
            optionDataList.Add(new Dropdown.OptionData(NetworkModeOptions.options[position].Label));
        }
        networkDropwork.AddOptions(optionDataList);
        AddListeners();
    }

    private void AddListeners()
    {

        networkDropwork.onValueChanged.AddListener(delegate {
            DropdownValueChanged(networkDropwork);
        });
    }

    void DropdownValueChanged(Dropdown change)
    {
        if (change.value != 0)
        {
            if ((change.value) == 1)
            {
                panelLan.SetActive(true);
                panelMatchMaker.SetActive(false);
                NetworkManagerSpecific.StartDiscovery();
                configuration = NetworkConfigurationGetter.getConfigurationObject();
                configuration.NetworkType = "LAN";

            }
            else
            {
                panelLan.SetActive(false);
                panelMatchMaker.SetActive(true);
            }
        }
        else
        {
            panelLan.SetActive(false);
            panelMatchMaker.SetActive(false);
        }
    }

}
