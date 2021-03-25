using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//Classe comprenant les couleurs des différents composants du bouton, et la taille qu'il devra adopter
[System.Serializable]
public class ToggleStateAppearance
{
    public Color imageColor = Color.white;
    public Color shadowColor = Color.grey;
    public Color textColor = Color.black;
    public float scaleFactor = 1;
}

//Un event prenant comme paramètre un booléen
[System.Serializable] public class BoolEvent : UnityEvent<bool> { }

//Classe comprenant un 'kit' d'apparences à adopter en fonction de l'état du bouton, et le text à afficher sur celui-ci
[System.Serializable]
public class ToggleAppearance
{
    public ToggleStateAppearance normalAppearance, hoverAppearance, pressedAppearance;
    public string label;
}

public class Toggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    //Les différents états du bouton: normal, survolé et pressé
    private enum ToggleState { Normal, Hover, Pressed}
    private ToggleState state = ToggleState.Normal;

    //Les différents composants du bouton
    [SerializeField] private Image image;
    [SerializeField] private Shadow shadow;
    [SerializeField] private TextMeshProUGUI text;


    [SerializeField] [Tooltip("L'état dans lequel sera le bouton au lancement")]
        private bool isToggled = false;

    [SerializeField] [Tooltip("Les méthodes à lancer quand on clique sur le bouton")]
        public BoolEvent events;

    [SerializeField] [Tooltip("Les différentes apparences à adopter")]
        private ToggleAppearance falseAppearance, trueAppearance;

    private float originalY;

    private void Awake()
    {
        //On garde la position en Y en mémoire
        originalY = ((RectTransform)transform).anchoredPosition.y;
    }

    private void Start()
    {
        //On lance l'event au lancement et on met à jour l'apparence
        events.Invoke(isToggled);
        isToggled = !isToggled;

        UpdateAppearance();
    }

    /// <summary>
    /// Met à jour l'apparence du bouton on fonction de isToggled et de son état
    /// </summary>
    private void UpdateAppearance()
    {
        //On détermine quelle 'kit' d'apparence à adopter en fonction de isToggled;
        ToggleAppearance _appearance = isToggled ? trueAppearance : falseAppearance;
        ToggleStateAppearance _stateAppearance = null;

        //On sélectionne une apparence dans le 'kit' à partir de l'état du bouton
        switch (state)
        {
            case ToggleState.Normal:
                _stateAppearance = _appearance.normalAppearance;
                shadow.enabled = true;
                ((RectTransform)transform).anchoredPosition = new Vector2(((RectTransform)transform).anchoredPosition.x, originalY);
                break;
            case ToggleState.Hover:
                _stateAppearance = _appearance.hoverAppearance;
                shadow.enabled = true;
                ((RectTransform)transform).anchoredPosition = new Vector2(((RectTransform)transform).anchoredPosition.x, originalY);
                break;
            case ToggleState.Pressed:
                _stateAppearance = _appearance.pressedAppearance;
                shadow.enabled = false;
                ((RectTransform)transform).anchoredPosition = new Vector2(((RectTransform)transform).anchoredPosition.x, originalY + shadow.effectDistance.y);
                break;
        }

        //On applique les données de l'apparence choisie
        ((RectTransform)transform).localScale = Vector3.one * _stateAppearance.scaleFactor;
        image.color = _stateAppearance.imageColor;
        shadow.effectColor = _stateAppearance.shadowColor;
        text.color = _stateAppearance.textColor;
        text.text = _appearance.label;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        state = ToggleState.Pressed;
        UpdateAppearance();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        state = ToggleState.Hover;
        UpdateAppearance();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        state = ToggleState.Normal;
        UpdateAppearance();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (state == ToggleState.Pressed)
        {
            events.Invoke(isToggled);
            isToggled = !isToggled;
        }
        state = ToggleState.Hover;

        UpdateAppearance();
    }
}
