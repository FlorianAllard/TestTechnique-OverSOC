# TestTechnique-OverSOC

Une Zone est créée au lancement de l'application, et sera générée à partir d'une ZoneData et d'un nombre d'entités à créer. On prendra en compte EntitiesPerRow afin de définir le nombre maximal d'entités par lignes et après un calcul le nombre de lignes. On place chaque entité de manière à les espacer de deux unités en X et Z. Une fois instanciée, l'entité est placée dans un pool pour faciliter et surtout optimiser la création et destruction d'entités.
Une fois les entités placées, une couleur leur ai assignée à partir d'un tableau de couleurs stocké dans ZoneData. Toutes les entités ont le même matériau, on utilise donc les MaterialPropertyBlocks pour changer leur couleur.
Un plane a été ajouté sous chaque entité afin de garder à l'esprit leur position si l'entité venait à être masquée.

Les boutons ont été crées à la main (sans passez par le component Unity) afin de permettre plus de flexibilité dans le contrôle de son apparence et dans l'appel des events BoolEvent (event prenant en paramètre un booléen).
J'ai décidé d'ajouter un bouton Randomize afin de laisser l'option de détruire/créer un nombre fixe d'entités selon EntitiesPerOperation du ZoneData ou, quand randomize est activé, un nombre aléatoire d'entités entre 0 et EntitiesPerOperation.
Le bouton Créer/Détruire sert à effectuer une opération sur la zone:
- En cliquant sur Détruire, on masque un certain nombre d'entités (le nombre est EntitiesPerRow clampé entre 0 et le nombre actuel d'entités affichées). Le bouton passe à l'état Créer une fois l'opération effectuée.
- En cliquant sur Créer, on affiche un certain nombre d'entités (le nombre est EntitiesPerRow clampé entre 0 et le nombre actuel d'entités affichées). Le bouton passe à l'état Détruire une fois l'opération effectuée.
