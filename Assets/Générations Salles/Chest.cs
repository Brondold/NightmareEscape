using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Chest : MonoBehaviour
{
    public WeightedRandomList<Transform> SelectionSalles;
    public Transform itemHolder1;
    public Transform itemHolder2;
    public Transform itemHolder3;

    public GameObject SalleA;
    public GameObject SalleB;
    public GameObject SalleC;
    public GameObject SalleD;

    private void Start()
    {
        ShowItems();
    }
    void Update()
    {
       // if (Input.GetKeyDown("space"))
        {
            //ShowItems();
        }

        //UNIQUEMENT POUR LES TESTS

        if (Input.GetMouseButton(0))
        {

            Destroy(itemHolder1.transform.GetChild(0).gameObject);
            Destroy(itemHolder2.transform.GetChild(0).gameObject);
            Destroy(itemHolder3.transform.GetChild(0).gameObject);
        }

        //UNIQUEMENT POUR LES TESTS

    }
    void ShowItems()
    {
        WeightedRandomList<Transform> selectionSalles = new WeightedRandomList<Transform>();

        // Ajoutez vos éléments à la liste avec leurs poids respectifs à l'aide de la méthode Add()
        selectionSalles.Add(SalleA.transform, 1);
        selectionSalles.Add(SalleB.transform, 1);
        selectionSalles.Add(SalleC.transform, 1);
        selectionSalles.Add(SalleD.transform, 1);

        List<Transform> items = new List<Transform>();
        List<Transform> itemHolders = new List<Transform> { itemHolder1, itemHolder2, itemHolder3 };

        while (items.Count < 3)
        {
            Transform item = selectionSalles.GetRandom();

            if (!items.Contains(item))
            {
                items.Add(item);
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            Instantiate(items[i], itemHolders[i]);
        }
    }


}