using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Zone : MonoBehaviour
{
    [SerializeField] [Tooltip("La banque de données qui initialise la génération de la zone")]
        private ZoneData data;
    [Min(0)] [SerializeField] [Tooltip("Le nombre d'entités à générer")]
        private int entitiesNeeded;

    private int currentEntities;

    [SerializeField] [Tooltip("Le prefab d'entité à instancier lors de la génération")]
        private Entity entityPrefab;
    private List<Entity> entityPool = new List<Entity>();

    private bool randomizeOperation;

    [SerializeField] [Tooltip("Le texte servant à afficher l'opération à l'écran")]
        private TextMeshProUGUI operationText;

    private void Start()
    {
        //Quand on lance l'application, on génère la zone à partir du nombre d'entités à générer
        GenerateZone(entitiesNeeded, true);
    }

    /// <summary>
    /// Génère la zone en fonction des données renseignées.
    /// </summary>
    /// <param name="_amountNeeded">Nombre d'entités à modifier.</param>
    /// <param name="_firstGeneration">Détermine si l'opération est la génération du lancement de l'application.</param>
    /// <returns>Rends le nombre d'entités qui ont pû étre affectées lors de l'opération demandée.</returns>
    public int GenerateZone(int _amountNeeded, bool _firstGeneration)
    {
        //On vérifie qu'on a bien une banque de données à utiliser
        if (data == null)
        {
            Debug.LogError("Le data de " + gameObject.name + " n'est pas assigné !");
            return 0;
        }

        //On détermine le nombre d'entités qui seront réellement affectées par l'opération
        _amountNeeded = Mathf.Clamp(_amountNeeded, 0, entitiesNeeded);
        int _remainder = _amountNeeded >= currentEntities ? _amountNeeded - currentEntities : currentEntities - _amountNeeded;

        //On détermine le nombre de lignes en fonction du nombre total d'entités et le nombre maximal d'entités par lignes.
        int _rowsAmount = (int)Mathf.Ceil((float)_amountNeeded / data.EntitiesPerRow);

        //On détermine le nombre d'entités à spawner en fonction du nombre d'entités requises et le nombre d'entités déjà spawnées.
        int _entitiesToSpawn = _amountNeeded - entityPool.Count;
        for (int i = 0; i < _entitiesToSpawn; i++)
        {
            entityPool.Add(Instantiate(entityPrefab, transform));
        }

        //On parcours le "tableau" en x et z, et on assigne une position et une couleur à chaque entité.
        int _index = 0;
        Vector3 _cameraCenter = Vector3.zero;
        for (int x = 0; x < _rowsAmount; x++)
        {
            for (int z = 0; z < data.EntitiesPerRow; z++)
            {
                if (_index >= _amountNeeded) break;

                entityPool[_index].SetActive(true);

                entityPool[_index].transform.position = new Vector3(x*2, 0, z*2);
                //On applique une couleur seulement lors de la première génération
                if(_firstGeneration) entityPool[_index].SetColor(data.Colors[Random.Range(0, data.Colors.Length)]);
                entityPool[_index].gameObject.name = "Entity " + _index;

                _index++;
            }

        }

        currentEntities = _index;

        //On désactive les entités déjà présentes mais inutilisées.
        for (_index = _index; _index < entityPool.Count; _index++)
        {
            entityPool[_index].SetActive(false);
        }

        //On centre la caméra pour faciliter la lecture lors de la première génération.
        if (_firstGeneration)
        {
            _cameraCenter = new Vector3(_rowsAmount -1, 0, data.EntitiesPerRow -1);
            Camera.main.transform.parent.position = _cameraCenter;
        }

        return _remainder;
    }

    /// <summary>
    /// Effectue une opération sur la zone (true = Créer / false = Détruire).
    /// </summary>
    /// <param name="_toggle">Détermine le type d'opération.</param>
    public void CreateOrDestroy(bool _toggle)
    {
        int _amount = 0;
        if (randomizeOperation)
        {
            //On détermine un nombre aléatoire d'entités auxquelles appliquer l'opération
            Random.seed = (int)System.DateTime.Now.Ticks;
            _amount = Random.Range(1, data.EntitiesPerOperation + 1);
        }
        else _amount = data.EntitiesPerOperation;

        //Si _toggle est faux, l'opération est une destruction, sinon elle est une création
        if (_toggle == false) _amount = -_amount;

        //On applique l'opération et on stocke le nombre réel d'entités affectées pour l'affichage
        int _remainder = GenerateZone(currentEntities + _amount, false);

        //On affiche le texte avec le nombre d'entités qu'on a voulu affecter, et le nombre réel d'entités affectées
        operationText.gameObject.SetActive(true);
        operationText.text = "Opération: " + _amount + "\n" 
            + _remainder + " entité(s) " + (_toggle ? "créée(s)" : "détruite(s)");
    }

    /// <summary>
    /// Modifie la valeur de randomizeOperation.
    /// </summary>
    /// <param name="_toggle">Détermine si la quantité d'entités affectées lors d'une opération est déterminée aléatoirement ou non.</param>
    public void SetRandomize(bool _toggle)
    {
        randomizeOperation = _toggle;
    }
}
