using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldGenerator : MonoBehaviour
{
    public GameObject _pointsObject;
    public Text _pointsLabel;
    public int _lengthOfField;
    public int _widthOfField;
    public GameObject _chipsPrefab;
    public GameObject _chips;
    public GameObject _helpChips;
    public ClickLogic _clickLogic;
    public Chips[ , ] chipsAdded;
    public List<Recipe> _ricepList; // make by hands. Replace enough recipe prefab 
    public List<Chips> _chipsEdited;
    public List<GameObject> _ingredientObjectList;
    public GameObject _bufferListObject;
    private int lengthChips = 21;
    private int widthChips = 21;
    private int points;
    private List<int> numberOfColumn;
    public Camera _mainCamera;
    [SerializeField] private int recipeCount;



    // Start is called before the first frame update
    void Start()
    {
        numberOfColumn = new List<int>();
        chipsAdded = new Chips[_lengthOfField, _widthOfField];
        RandomizeRiceps();
        if (_widthOfField <= 2)
        {
            _mainCamera.transform.position = new Vector3(30f, 50f, 20f);
            _mainCamera.orthographicSize = 40;
        }
        else
        {
            _mainCamera.transform.position = new Vector3((12f + (_widthOfField) * 10), 50f, _widthOfField * 10);
            _mainCamera.orthographicSize = ((_widthOfField) * 20);
        }
        
        _bufferListObject.GetComponent<SpecialList>()._chipsCoordinate = new GameObject[_lengthOfField, _widthOfField, 1];
        points = 0;
        _pointsLabel = _pointsObject.GetComponent<Text>();
        _pointsLabel.text += $"\n {points}";
        _clickLogic._recipeList = _ricepList;
        _clickLogic.GetLastRecipe();
        GenerateField();
        GetCountRecipeChips();

        if (chipsAdded.Length < recipeCount)
        {
            Debug.Log($"Need to add more recipe, not enough chips. You should add {recipeCount - chipsAdded.Length} chips to ricepes");
        }
        else
        {
            SimpleNumber();
            SetTypesForChips();
            CreateRecipeOnField(_clickLogic._recipeList);
            ShowLastRecipe(_clickLogic._recipeList);

            Debug.Log($"There is enough recipe");
        }
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void GenerateField()
    {
        for (int i = 1; i <= _widthOfField; i++)
        {
            for (int j = 1; j <= _lengthOfField; j++)
            {
                _chips = Instantiate(_chipsPrefab, new Vector3(transform.position.x + lengthChips, transform.position.y, transform.position.z + widthChips), _helpChips.transform.rotation);
                // Try to not mix up
                chipsAdded[j-1, i-1] = _chips.GetComponent<Chips>();
                //chipsAdded.Add(_chips.GetComponent<Chips>());
                lengthChips += 21;
                _chips.AddComponent<ClickLogic>();
                _chips.GetComponent<ClickLogic>()._recipeList = _ricepList;
                _chips.GetComponent<ClickLogic>().ingredientList = _clickLogic.ingredientList;
                _chips.GetComponent<ClickLogic>()._fieldGenerator = _clickLogic._fieldGenerator;
                _chips.GetComponent<ClickLogic>()._bufferListObject = _bufferListObject;
                _bufferListObject.GetComponent<SpecialList>()._chipsCoordinate[j - 1, i - 1, 0] = _chips;
                _bufferListObject.GetComponent<SpecialList>()._chipsCoordinate[j - 1, i - 1, 0].GetComponent<ClickLogic>().XPosition = j - 1;
                _bufferListObject.GetComponent<SpecialList>()._chipsCoordinate[j - 1, i - 1, 0].GetComponent<ClickLogic>().YPosition = i - 1;
                _bufferListObject.GetComponent<SpecialList>()._chipsCoordinate[j - 1, i - 1, 0].GetComponent<ClickLogic>().ZPosition = 0;
            }
            lengthChips = 21;
            widthChips += 21;
        }
    }

    public void GetCountRecipeChips()
    {
        for (int i = 1; i <= _ricepList.Count; i++)
        {
            recipeCount += _ricepList[i - 1]._recipe.Count;
        }
    }

    public void SetTypesForChips()
    {
        int YCoordinate = 0;
        int XCoordinate = 0;
        float randomvalue;
        int helpVariable = 0;

        for (int l = _ricepList.Count - 1; l >= 0; l--)
        {
            for (int k = 0; k <= _ricepList[l]._recipe.Count - 1; k++)
            {
                Debug.Log($"I'm doing it for this time");
                randomvalue = Random.value;
                if(randomvalue <= 0.5f)
                {
                    Debug.Log($"Random less");
                    if (chipsAdded[numberOfColumn[XCoordinate], YCoordinate]._emptyType == true)
                    {
                        chipsAdded[numberOfColumn[XCoordinate], YCoordinate]._emptyType = false;
                        chipsAdded[numberOfColumn[XCoordinate], YCoordinate]._ingredient = _ricepList[l]._recipe[k]._ingredient;
                        SetTexturesForChips(chipsAdded[numberOfColumn[XCoordinate], YCoordinate].gameObject, l, k);
                        YCoordinate++;
                        if (helpVariable >= 2)
                        {
                            XCoordinate++;
                            helpVariable = 0;
                        }
                        if (YCoordinate >= (_widthOfField))
                        {                            
                            YCoordinate = 0;
                            helpVariable++;
                        }
                    }
                    else
                    {
                        chipsAdded[numberOfColumn[XCoordinate + 1], YCoordinate]._emptyType = false;
                        chipsAdded[numberOfColumn[XCoordinate + 1], YCoordinate]._ingredient = _ricepList[l]._recipe[k]._ingredient;
                        SetTexturesForChips(chipsAdded[numberOfColumn[XCoordinate + 1], YCoordinate].gameObject, l, k);
                        YCoordinate++;
                        if (helpVariable >= 2)
                        {
                            XCoordinate++;
                            helpVariable = 0;
                        }
                        if (YCoordinate >= (_widthOfField))
                        {
                            YCoordinate = 0;
                            helpVariable++;
                        }
                    }
                    
                }
                else
                {
                    Debug.Log($"Random more");
                    if (chipsAdded[numberOfColumn[XCoordinate + 1], YCoordinate]._emptyType == true)
                    {
                        chipsAdded[numberOfColumn[XCoordinate + 1], YCoordinate]._emptyType = false;
                        chipsAdded[numberOfColumn[XCoordinate + 1], YCoordinate]._ingredient = _ricepList[l]._recipe[k]._ingredient;
                        SetTexturesForChips(chipsAdded[numberOfColumn[XCoordinate + 1], YCoordinate].gameObject, l, k);
                        YCoordinate++;
                        if (helpVariable >= 2)
                        {
                            XCoordinate++;
                            helpVariable = 0;
                        }
                        if (YCoordinate >= (_widthOfField))
                        {
                            YCoordinate = 0;
                            helpVariable++;
                        }
                    }
                    else
                    {
                        chipsAdded[numberOfColumn[XCoordinate], YCoordinate]._emptyType = false;
                        chipsAdded[numberOfColumn[XCoordinate], YCoordinate]._ingredient = _ricepList[l]._recipe[k]._ingredient;
                        SetTexturesForChips(chipsAdded[numberOfColumn[XCoordinate], YCoordinate].gameObject, l, k);
                        YCoordinate++;
                        if (helpVariable >= 2)
                        {
                            XCoordinate++;
                            helpVariable = 0;
                        }
                        if (YCoordinate >= (_widthOfField))
                        {
                            YCoordinate = 0;
                            helpVariable++;
                        }
                    }
                }
            }
        }
    }

    public void CreateRecipeOnField(List<Recipe> recipeList)
    {        
        Chips chipsComponent;
        GameObject chips;
        lengthChips = 21;
        widthChips = 4;
        for (int i = 0; i <= recipeList.Count - 1; i++)
        {
            lengthChips = 21;
            for (int j = 0; j <= recipeList[i]._recipe.Count - 1; j++)
            {
                chips = Instantiate(_chipsPrefab, new Vector3(transform.position.x + lengthChips, transform.position.y, transform.position.z - widthChips), _helpChips.transform.rotation);
                SetTexturesForChips(chips, i, j);
                chipsComponent = chips.GetComponent<Chips>();
                chipsComponent._emptyType = false;

                chipsComponent._ingredient = recipeList[i]._recipe[j]._ingredient;
                chipsComponent._ingredientObject = chips.gameObject;
                recipeList[i]._recipe[j]._ingredientObject = chips;
                recipeList[i]._recipe[j]._ingredientObject.SetActive(false);
                lengthChips += 21;
                _ingredientObjectList.Add(chips);
            }
        }
    }

    public void SetTexturesForChips(GameObject chips, int numberRecipe, int numberIngredient)
    {
        Renderer renderComponent;
        renderComponent = chips.GetComponent<Renderer>();
        Material[] materials = renderComponent.materials;
        materials[0] = _ricepList[numberRecipe]._recipe[numberIngredient]._material;
        materials[1] = _ricepList[numberRecipe]._recipe[numberIngredient]._material;
        renderComponent.materials = materials;
    }

    public void DeleteReadyRecipe(List<Recipe> recipeList)
    {
        int value = _ingredientObjectList.Count - 1;
        if (_clickLogic.ingredientList.Count == 0)
        {
            for (int i = 0; i <= recipeList[recipeList.Count - 1]._recipe.Count - 1; i++)
            {
                _ingredientObjectList[(value - i)].SetActive(false);
                _ingredientObjectList.Remove(_ingredientObjectList[(value - i)]);
            }
            recipeList.Remove(recipeList[recipeList.Count - 1]);

            points += 10;
            _pointsLabel.text = $"Points:\n {points}";
            if (recipeList != null)
            {
                ShowLastRecipe(recipeList);
                _clickLogic.GetLastRecipe();
            }

        }
    }

    public void ShowLastRecipe(List<Recipe> recipeList)
    {
        for (int j = recipeList[recipeList.Count - 1]._recipe.Count - 1; j >= 0; j--)
        {
            _ingredientObjectList[(_ingredientObjectList.Count - 1) - j].SetActive(true);
        }
    }

    public void RandomizeRiceps()
    {
        for (int i = 0; i < _ricepList.Count; i++)
        {
            Recipe temp = _ricepList[i];
            int randomIndex = Random.Range(i, _ricepList.Count);
            _ricepList[i] = _ricepList[randomIndex];
            _ricepList[randomIndex] = temp;
        }
    }

    public void SimpleNumber()
    {
        for (int i =0; i < (_widthOfField/2); i++)
        {
            numberOfColumn.Add(0 + i);
            numberOfColumn.Add((_widthOfField-1)-i);
            Debug.Log($"{0 + i} and {(_widthOfField-1)-i}");
        }
    }
}
