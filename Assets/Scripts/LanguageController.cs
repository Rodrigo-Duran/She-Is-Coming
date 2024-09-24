using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class LanguageController : MonoBehaviour
{

    //LABELS
    [Header("FOR LABELS: ")]

    [Header("Portuguese: ")]
    [SerializeField] private string portuguese;

    [Header("English: ")]
    [SerializeField] private string english;

    //DROPDOWNS
    [Header("FOR DROPDOWNS: ")]

    [Header("Is Dropdown: ")]
    [SerializeField] private bool isDropdown = false;

    [Header("Portuguese Options: ")]
    [SerializeField] private List<string> portugueseOptions;

    [Header("English Options: ")]
    [SerializeField] private List<string> englishOptions;

    private TMP_Dropdown dropdown = null;

    private TextMeshProUGUI label;

    // Awake
    void Awake()
    {
        //SE CASO FOR DROPDOWN, PEGA ELE NO OBJETO PAI
        if (isDropdown) dropdown = GetComponentInParent<TMP_Dropdown>();

        label = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //SE NÃO FOR DROPDOWN
        if (!isDropdown)
        {
            //SE ESTIVER EM PORTUGUES
            if (GameController.language == 0)
            {
                //MUDA O TEXTO PARA O PORTUGUES
                label.text = portuguese;
            }
            //SE ESTIVER EM INGLES
            else if (GameController.language == 1)
            {
                //MUDA O TEXTO PARA INGLES
                label.text = english;
            }
        }
        //SE FOR DROPDOWN
        else
        {
            //SE ESTIVER EM PORTUGUES
            if (GameController.language == 0)
            {
                //MUDA TODAS AS OPCOES DO DROPDOWN PARA PORTUGUES
                for (int i = 0; i < dropdown.options.Count; i++)
                {
                    dropdown.options[i].text = portugueseOptions[i];
                }

                //MUDA O TEXTO ATUAL DO DROPDOWN PARA A OPÇÃO ATUAL EM PORTUGUES
                label.text = portugueseOptions[dropdown.value];
            }
            //SE ESTIVER EM INGLES
            else if (GameController.language == 1)
            {
                //MUDA TODAS AS OPCOES DO DROPDOWN PARA INGLES
                for (int i = 0; i < dropdown.options.Count; i++)
                {
                    dropdown.options[i].text = englishOptions[i];
                }

                //MUDA O TEXTO ATUAL DO DROPDOWN PARA A OPÇÃO ATUAL EM INGLES
                label.text = englishOptions[dropdown.value];
            }
        }
    }
}
