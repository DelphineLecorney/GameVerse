# 🎮 GameVerse API  
API RESTful développée en .NET 10 permettant de gérer des utilisateurs, des jeux vidéo et leurs relations (wishlist, favoris, bibliothèque, etc.).  
Le projet utilise Entity Framework Core et une base de données Azure SQL.


![.NET](https://img.shields.io/badge/.NET-10-blueviolet)
![Build](https://img.shields.io/badge/build-passing-brightgreen)
![Status](https://img.shields.io/badge/status-active-success)
![Platform](https://img.shields.io/badge/platform-Azure-blue)

---

## 🚀 Fonctionnalités actuelles

### ✔ Initialisation du projet
- Création d’un projet **ASP.NET Core Web API (.NET 10)**  
- Mise en place d’une architecture propre (Models, Data, Services, Controllers)

### ✔ Gestion sécurisée des secrets
- Activation du **User Secrets Manager**
- Stockage sécurisé de la chaîne de connexion Azure SQL
- `.gitignore` configuré pour exclure :
  - `Secrets.json`
  - `.env`
  - `appsettings.Development.json`
  - `UserSecrets/`
  - `bin/`, `obj/`, etc.

### ✔ Base de données Azure SQL
- Création d’une base **GameVerse** sur Azure
- Exécution du script SQL initial
- Connexion testée et validée depuis l’API

### ✔ Entity Framework Core
- Installation des packages :
  - `Microsoft.EntityFrameworkCore`
  - `Microsoft.EntityFrameworkCore.SqlServer`
  - `Microsoft.EntityFrameworkCore.Design`
- Création du `GameVerseContext`
- Définition d’une **clé composite** pour `UserGame`
- Création et application de la migration `Initial`
- Vérification des tables dans Azure :
  - `Users`
  - `Games`
  - `UserGames`
  - `__EFMigrationsHistory`

---

## 🗂 Structure du projet

```
GameVerse/
│
├── docs/
│   ├── images/
│
├── GameVerse.API/
│   ├── Controllers/
│   ├── Data/
│   │   └── GameVerseContext.cs
│   ├── Models/
│   │   ├── User.cs
│   │   ├── Game.cs
│   │   └── UserGame.cs
│   ├── Properties/
│   ├── appsettings.json
│   ├── Program.cs
│   └── README.md
├── GameVerse.Tests/
├── GameVerse.Client/
├── GameVerse.Domain/
├── GameVerse.Infrastructure/
└── README.md
```


---

## 🔌 Connexion à la base Azure SQL

La chaîne de connexion est stockée dans les **User Secrets** :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:<server>.database.windows.net,1433;Initial Catalog=GameVerse;User ID=<user>;Password=<password>;Encrypt=True;"
  }
}
```

---

## 📸 Migration appliquée sur Azure SQL

Voici la migration EF Core appliquée avec succès sur Azure :

![Migration Azure](docs/images/migration-azure.png)

---
## 🔐 Sécurité

###  Authentification JWT

L’API utilise un système d’authentification basé sur **JSON Web Tokens (JWT)** pour sécuriser les endpoints.
- Tokens signés
- Validation de l’issuer, audience et signature
- Expiration automatique

### 🔑 Fonctionnement
- Lors de l’inscription, un utilisateur est créé dans la base.
- Lors de la connexion, un **token JWT signé** est généré.
- Ce token doit être envoyé dans les requêtes protégées via l’en-tête :


### ✔ Protection des secrets
- User Secrets activé
- Aucun secret dans GitHub
- Connexion Azure SQL sécurisée (Encrypt=True)

### ✔ HTTPS obligatoire
L’API force la redirection HTTPS.

### ✔ Bonnes pratiques
- Ne jamais stocker les mots de passe en clair
- Utiliser BCrypt pour le hashing
- Ne jamais exposer les entités EF Core directement

---

## 🚀 Déploiement

### 🌐 Déploiement possible sur :
- Azure App Service
- Azure Container Apps
- Docker + Azure Container Registry
- Kubernetes (AKS)

### 🧪 Étapes de déploiement (Docker)
```
bash
docker build -t gameverse-api .
docker run -p 8080:80 gameverse-api
```
