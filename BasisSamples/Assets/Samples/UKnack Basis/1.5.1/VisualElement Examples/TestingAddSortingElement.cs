using System.Collections;
using System.Collections.Generic;
using UKnack;
using UKnack.Common;
using UKnack.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class TestingAddSortingElement : MonoBehaviour
{
    [SerializeField] 
    private string _addSortedHereName;

    [SerializeField] 
    private string _sortingOrderInputName;

    [SerializeField] 
    private string _addButtonName;

    private VisualElement _sortedPlacholder;
    private IntegerField _sortingOrderInput;
    private Button _addButton;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _sortedPlacholder = root.Q<VisualElement>(_addSortedHereName);

        _sortingOrderInput = root.Q<IntegerField>(_sortingOrderInputName);
        _addButton = root.Q<Button>(_addButtonName);
        _addButton.clicked += AddSortedWithRandomColor;

        AddSorted(Color.red, -1000, 30);
        AddSorted(Color.green, 1000, 30);
        AddSorted(Color.black, 30);
        AddSorted(Color.white);
        AddSorted(Color.blue, 2000, 30);
    }

    private void OnDisable()
    {
        _addButton.clicked -= AddSortedWithRandomColor;
    }

    public void AddSortedWithRandomColor()
    {
        AddSorted(Random.ColorHSV(), _sortingOrderInput.value);
    }

    public void AddSorted(Color background, int sortingOrder = 0, int height = 30)
    {
        _sortedPlacholder.TryAddSafeAndOrderCorrectly(CreateSortedColored(background, sortingOrder, height));
    }

    public static VisualElement CreateSortedColored(Color background, int sortingOrder, int height = 30)
    {
        VisualElement a;
        if (sortingOrder == 0)
            a = new VisualElement();
        else
            a = new VisualElementSortedOnAddition(sortingOrder);
        a.style.backgroundColor = background;
        a.style.width = 400;
        a.style.height = height;
        a.TryAddSafeAndOrderCorrectly(new Label(sortingOrder.ToString()) { style = { color = ColorHelpers.InvertedRGB( background) } });
        return a;
    }

    //public static Color InvertRGB(Color color) => new Color(1-color.r, 1-color.g, 1-color.b, color.a);
}
