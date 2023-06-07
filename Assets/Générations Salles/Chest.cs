using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Chest : MonoBehaviour
{
    public WeightedRandomList<Transform> SelectionSalles;
    public Transform itemHolder1;
    public Transform itemHolder2;
    public Transform itemHolder3;

    public GameObject Salle1;
    public GameObject Salle2;
    public GameObject Salle3;
    public GameObject Salle4;
    public GameObject Salle5;
    public GameObject Salle6;
    public GameObject Salle7;


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
        selectionSalles.Add(Salle1.transform, 1);
        selectionSalles.Add(Salle2.transform, 1);
        selectionSalles.Add(Salle3.transform, 1);
        selectionSalles.Add(Salle4.transform, 1);
        selectionSalles.Add(Salle5.transform, 1);
        selectionSalles.Add(Salle6.transform, 1);
        selectionSalles.Add(Salle7.transform, 1);

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