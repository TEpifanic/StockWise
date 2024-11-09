# StockWise

API de gestion de stock développée avec ASP.NET Core 8.0, implémentant le pattern CQRS.

## 🏗️ Architecture

L'application suit une architecture en couches avec CQRS :

- **API** : Points d'entrée REST
- **Commands** : Gestion des opérations d'écriture (CQRS)
- **Common** : DTOs, Mapping, et Validation
- **Domain** : Entités et interfaces métier
- **Persistence** : Accès aux données et configuration
- **Tests** : Tests unitaires et d'intégration

## 🚀 Fonctionnalités

### Gestion des Produits

#### Endpoints API

- `POST /api/products` - Créer un produit
- `GET /api/products` - Liste des produits
- `GET /api/products/{id}` - Détails d'un produit
- `GET /api/products/price-range` - Recherche par prix
- `PUT /api/products/{id}` - Mettre à jour un produit
- `DELETE /api/products/{id}` - Supprimer un produit

#### Validation des Données

- **Nom** : Requis, max 100 caractères
- **Description** : Optionnel, max 500 caractères
- **Prix** : Supérieur à 0
- **URL Image** : Optionnel, format URL valide

## 🛠️ Technologies

### Framework
- **ASP.NET Core 8.0**
- **Entity Framework Core**
- **SQLite**

### Patterns & Architecture
- **CQRS** via MediatR
- **Repository Pattern**
- **Dependency Injection**

### Librairies
- **MediatR** - Pattern CQRS
- **AutoMapper** - Mapping objet-objet
- **FluentValidation** - Validation des données
- **Swagger/OpenAPI** - Documentation API

### Tests
- **xUnit**
- **Moq**

## 📝 Exemples d'Utilisation

### Créer un Produit
```http
POST /api/products
Content-Type: application/json

{
  "name": "iPhone 15",
  "description": "Dernier modèle d'iPhone",
  "price": 999.99,
  "imageUrl": "https://example.com/iphone15.jpg"
}
```

### Rechercher par Prix
```http
GET /api/products/price-range?minPrice=10&maxPrice=100
```

## 🔄 Workflow de Développement

1. Cloner le repository
2. Restaurer les packages NuGet
3. Exécuter les migrations EF Core
4. Lancer l'application

## 🧪 Tests

Exécuter les tests :
```bash
dotnet test
```

## 🚧 Points d'Extension Futurs

- Authentification/Autorisation
- Pagination des résultats
- Filtrage avancé
- Cache
- Logging avancé
- Métriques et monitoring
- Gestion des catégories
- Gestion des stocks

## 📦 Structure du Projet

```
StockWise/
├── API/
│   └── Controllers/
├── Commands/
│   └── Products/
├── Common/
│   ├── DTOs/
│   ├── Mapping/
│   └── Validation/
├── Domain/
│   └── Entities/
├── Persistence/
│   └── Infrastructure/
└── Tests/
```

## 📋 Prérequis

- .NET 8.0 SDK
- SQLite

## 🚀 Démarrage

1. Cloner le repository
```bash
git clone [url-du-repo]
```

2. Naviguer vers le dossier du projet
```bash
cd StockWise
```

3. Restaurer les dépendances
```bash
dotnet restore
```

4. Appliquer les migrations
```bash
dotnet ef database update
```

5. Lancer l'application
```bash
dotnet run
```

L'API sera accessible à `https://localhost:5001`  
La documentation Swagger à `https://localhost:5001/swagger`

## 📖 Documentation API

Documentation complète disponible via Swagger UI une fois l'application lancée.

## 👥 Contribution

1. Fork le projet
2. Créer une branche pour votre fonctionnalité
3. Commiter vos changements
4. Pousser vers la branche
5. Ouvrir une Pull Request
