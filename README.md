
# 🍽️ NoWaste

**NoWaste** est une application mobile multiplateforme développée avec **.NET MAUI**. Elle aide à réduire le gaspillage alimentaire en utilisant l’intelligence artificielle pour analyser les aliments à partir d’une photo, puis en suggérant des recettes à partir des aliments reconnus.

## 📸 Fonctionnalités principales

- 📷 **Capture et analyse d’image** : prenez une photo de vos aliments.
- 🧠 **Reconnaissance d’aliments** via l’API CALORIEMAMA.
- 📋 **Affichage interactif des résultats** : noms, familles, groupes, scores nutritionnels.
- ➕ **Ajout manuel** d’aliments si besoin.
- ❌ **Suppression d’un aliment reconnu**.
- 🍳 **Suggestions de recettes** via l'API SPOONACULAR à partir des aliments détectés.

## 🧪 Technologies utilisées

- [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/) (multi-plateforme)
- **C#** (architecture MVVM légère)
- **API CALORIEMAMA** pour l’analyse d’image alimentaire
- **API SPOONACULAR** pour la suggestion de recettes
- **HttpClient** pour la requête POST
- **System.Text.Json** pour le traitement des réponses

## 📂 Structure du projet

```
/NoWaste
│
├── Models/
│   └── CalorieMamaPrediction.cs         # Modèle d’aliment avec nom, groupe, score, etc.
│
├── Services/
│   └── ImageAnalyzer.cs                 # Envoie l’image à l’API CALORIEMAMA et traite la réponse
│
├── Views/
│   ├── CameraPage.xaml(.cs)             # Page pour capturer une image et lancer l’analyse
│   └── ResultPage.xaml(.cs)             # Page qui affiche les résultats (aliments détectés)
│
├── AppShell.xaml                        # Définition de la navigation
└── MainPage.xaml                        # Page d’accueil (non utilisée directement)
```

## 🧠 Fonctionnement technique

1. L’utilisateur prend une photo via **CameraPage**.
2. L’image est redimensionnée et envoyée à **l’API CALORIEMAMA** par `ImageAnalyzer.cs`.
3. La réponse est désérialisée en objets `SegmentedItem` et `SegmentationCompleteResponse`.
4. Les résultats sont affichés sur `ResultPage.xaml` avec la possibilité :
   - d’ajouter manuellement un aliment,
   - de supprimer un élément reconnu.
5. Affichage d'une recette sur `RecettePage.xaml` via **l’API SPOONACULAR** traîtée dans `SpoonacularRecipeService.cs`

## 🛠️ Prérequis

- .NET SDK 8.0+
- Visual Studio 2022/2025 avec les workloads MAUI installés
- Compte sur [CALORIEMAMA](https://caloriemama.ai/)/) avec une clé API
- Accès à une caméra (Android ou iOS)

## ▶️ Lancer le projet

1. Clone le dépôt :
   ```bash
   git clone https://github.com/YacDms/NoWaste.git
   cd NoWaste
   ```

2. Renseigne ta clé API dans `ImageAnalyzer.cs` :
   ```csharp
   private static readonly string apiKey = "VOTRE_CLÉ_API_CALORIEMAMA";
   ```

3. Lance l’application depuis Visual Studio sur un simulateur ou un appareil.

## ✨ Exemple d’usage

> 📸 Je prends une photo de mon frigo → l’application détecte « brocoli, tomate, carottes » → j’obtiens la liste → je peux ajouter ou retirer des aliments → (prochainement) j’obtiens des idées de recettes !

## 📌 Améliorations à venir

- 🧾 Suggestions de plusieurs recettes à partir des aliments détectés
- 📅 Suivi des dates de péremption
- 🔔 Notifications avant expiration
- 📊 Tableau nutritionnel avec IA

## 🤝 Contribuer

Les PR sont les bienvenues ! Si tu veux proposer une idée ou corriger un bug :

1. Fork le projet
2. Crée une branche
3. Envoie ta Pull Request !

## 📄 Licence

:)

## 🙋‍♀️ Auteur

En cours de développement par **Yacine Dehmous (YacDms)**.
