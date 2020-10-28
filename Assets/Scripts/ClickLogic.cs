using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickLogic : MonoBehaviour, IPointerClickHandler
{
    private Chips chips;
    public GameObject _ingredientObject;
    public List<Recipe> _recipeList;
    public List<string> ingredientList;
    public bool _used;
    public List<GameObject> _usedList;
    public FieldGenerator _fieldGenerator;
    public GameObject thisObject;
    public GameObject _bufferListObject;
    private SpecialList specialList;
    private Renderer _component;
    public int XPosition { get; set; }
    public int YPosition { get; set; }
    public int ZPosition { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _usedList = new List<GameObject>();
        chips = gameObject.GetComponent<Chips>();
        specialList = _bufferListObject.GetComponent<SpecialList>();
        _component = gameObject.GetComponent<Renderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        ClickLogic component;
        component = specialList._chipsCoordinate[XPosition, YPosition, ZPosition].GetComponent<ClickLogic>();
        if (component.ZPosition +1 > specialList._chipsCoordinate.GetUpperBound(2))
        {
            if (component.XPosition + 1 > specialList._chipsCoordinate.GetUpperBound(0) || component.XPosition - 1 < 0)
            {                
                Comparison();
            }
            else
            {
                if (specialList._chipsCoordinate[XPosition + 1, YPosition, ZPosition].activeSelf == false || specialList._chipsCoordinate[XPosition - 1, YPosition, ZPosition].activeSelf == false)
                {                   
                    Comparison();
                }
                else
                {
                    DeSelectChips();
                }
            }
            
        }
        else
        {

        }
    }

    public void GetLastRecipe()
    {
        for (int i = 0; i <= _recipeList[_recipeList.Count - 1]._recipe.Count - 1; i++)
        {            
            ingredientList.Add(_recipeList[_recipeList.Count - 1]._recipe[i]._ingredient);
        }
    }

    public void Comparison()
    {
        if (ingredientList.Contains(chips._ingredient))
        {
            specialList._localList.Add(chips._ingredient);
            specialList._usedObjects.Add(gameObject);
            ingredientList.Remove(chips._ingredient);
            _used = true;
            _component.materials[0].color = new Color(0.55f, 1f, 0.55f);
            _component.materials[1].color = new Color(0.55f, 1f, 0.55f);

            if (ingredientList.Count == 0)
            {
                for (int i = 0; i <= specialList._usedObjects.Count - 1; i++)
                {
                    specialList._usedObjects[i].SetActive(false);
                }
                specialList._usedObjects.Clear();
                specialList._localList.Clear();
                ingredientList.Clear();
                _fieldGenerator.DeleteReadyRecipe(_fieldGenerator._clickLogic._recipeList);
            }
        }
        else
        {
            DeSelectChips();
        }
    }

    public void DeSelectChips()
    {
        foreach (GameObject material in specialList._usedObjects)
        {
            material.GetComponent<Renderer>().materials[0].color = new Color(1f, 1f, 1f);
            material.GetComponent<Renderer>().materials[1].color = new Color(1f, 1f, 1f);
            material.GetComponent<ClickLogic>()._used = false;
        }

        for (int i = specialList._localList.Count - 1; i >= 0; i--)
        {
            ingredientList.Add(specialList._localList[i]);
        }
        specialList._localList.Clear();
        specialList._usedObjects.Clear();
    }
}
