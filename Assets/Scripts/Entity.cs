using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] [Tooltip("Renderer du de l'entité")]
        private MeshRenderer visualRenderer;
    private MaterialPropertyBlock matPropBlock;

    /// <summary>
    /// Applique une taille et une couleur à l'Entité.
    /// </summary>
    /// <param name="_scale">Taille de l'entity.</param>
    /// <param name="_color">Couleur de l'entity.</param>
    public void SetColor(Color _color)
    {
        //On vérifie que les Components soient bien remplis.
        if (visualRenderer == null)
        {
            Debug.LogError("Le visualRenderer de " + gameObject.name + " n'est pas assigné !");
            return;
        }

        //Si ce n'est pas déjà fait, on initialise le propertyBlock.
        if (matPropBlock == null)
        {
            matPropBlock = new MaterialPropertyBlock();
            visualRenderer.GetPropertyBlock(matPropBlock);
        }

        //On applique la couleur au propertyBlock afin de changer la couleur de cet entité sans en changer le matériau.
        matPropBlock.SetColor("_Color", _color);
        visualRenderer.SetPropertyBlock(matPropBlock);
    }

    /// <summary>
    /// Désactive uniquement le visuel de l'entité.
    /// </summary>
    /// <param name="_toggle">Valeur du toggle.</param>
    public void SetActive(bool _toggle)
    {
        visualRenderer.gameObject.SetActive(_toggle);
    }

    #region Debugging

    [ContextMenu("Debug SetData")]
    private void Debug_SetData()
    {
        SetColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
    }
    [ContextMenu("Debug ResetData")]
    private void Debug_ResetData()
    {
        SetColor(Color.white);
    }

    #endregion
}
