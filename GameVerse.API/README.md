# 🎮 GameVerse API

API RESTful développée en **.NET 10**, permettant de gérer des utilisateurs, des jeux vidéo et leurs relations (wishlist, favoris, bibliothèque, etc.).
Le projet utilise **Entity Framework Core** et une base **Azure SQL**.

[⬅ Retour au README principal](../README.md)

---

## 📑 Sommaire

- [Fonctionnalités](#-fonctionnalités)
- [Structure du projet](#-structure-du-projet)
- [Connexion à la base Azure SQL](#-connexion-à-la-base-azure-sql)
- [Configuration Modèle & Pivot EF Core](#-configuration-modèle--pivot-ef-core)
- [Migration Azure SQL](#-migration-appliquée-sur-azure-sql)
- [Sécurité](#-sécurité)
- [Documentation Scalar](#-accès-à-la-documentation-scalar)
- [Déploiement](#-déploiement)

---

## 📸 Captures d'écran

<table>
  <thead>
    <tr>
      <th align="center" width="50%">Inscription</th>
      <th align="center" width="50%">Connexion</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td align="center"><img src="../docs/images/Register.png" width="200"></td>
      <td align="center"><img src="../docs/images/Login.png" width="200"></td>
    </tr>
  </tbody>
</table>

---

## 🚀 Fonctionnalités

### 👤 Gestion des utilisateurs
- Inscription & connexion
- Récupération, mise à jour et suppression d'un utilisateur
- Authentification sécurisée via **JWT**

### 🎮 Gestion des jeux
- CRUD complet
- DTOs dédiés (Create, Update, Read)
- AutoMapper configuré
- Endpoints sensibles protégés

### ❤️ Relations User ↔ Game
- Ajout d'un jeu à la bibliothèque ou à la liste de souhaits (`RelationType`: `Wishlist` ou `Library`)
- **Statut favori indépendant** (`IsFavorite`) : un jeu peut être marqué favori qu'il soit dans la bibliothèque ou la wishlist — nécessite que le jeu soit déjà présent dans l'une des deux listes
- Mise à jour de la relation (type, rating)
- Suppression d'un jeu de la bibliothèque
- Listing des jeux d'un utilisateur (bibliothèque, wishlist, favoris)
- DTOs dédiés (`GameWithStatusDto` pour le catalogue avec statut utilisateur)

### 🔐 Sécurité & Secrets
- **User Secrets Manager** activé
- Connexion Azure SQL sécurisée
- `.gitignore` renforcé (secrets, env, appsettings, bin/obj…)

### 🗄 Base de données Azure SQL
- Base **GameVerse** déployée sur Azure
- Migration initiale appliquée
- Tables : `Users`, `Games`, `UserGames`, `RefreshTokens`, `__EFMigrationsHistory`

### 🧱 Entity Framework Core
- Contexte `GameVerseContext`
- Clé composite pour `UserGame`
- Relation Many-to-Many configurée via Fluent API
- Migration `Initial` appliquée avec succès

### ✔ Validation (FluentValidation)
- Suppression des DataAnnotations
- Validateurs dédiés par entité
- Pipeline global (`ValidationFilter`) renvoyant automatiquement les erreurs en **400 Bad Request**

### 📘 Documentation (Scalar)
- Intégration OpenAPI (`AddOpenApi`)
- Interface Scalar moderne accessible via `/scalar`

---

## 🗂 Structure du projet

```
GameVerse.API/
│
├── Controllers/
│   ├── AuthController.cs
│   ├── UsersController.cs
│   ├── GamesController.cs
│   └── UserGamesController.cs
│
├── DTOs/
│   ├── Auth/
│   ├── Games/
│   ├── Users/
│   └── UserGames/
│
├── Services/
│   ├── Interfaces/
│   ├── AuthService.cs
│   ├── UserService.cs
│   ├── GameService.cs
│   └── UserGameService.cs
│
├── Mappings/
│   └── AutoMapperProfile.cs
│
├── Data/
│   └── GameVerseContext.cs
│
├── Models/
│   ├── User.cs
│   ├── Game.cs
│   └── UserGame.cs
│
├── Program.cs
└── appsettings.json
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

## 🧱 Configuration Modèle & Pivot EF Core

### Modèle de données `UserGame`

```csharp
public class UserGame
{
    public int UserId { get; set; }
    public int GameId { get; set; }
    public string RelationType { get; set; } = "Wishlist";
    public DateTime AddedAt { get; set; } = DateTime.Now;
    public int? Rating { get; set; }

    public User? User { get; set; }
    public Game? Game { get; set; }
}
```

### Configuration Fluent API (`GameVerseContext`)

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Configuration de la clé primaire composite
    modelBuilder.Entity<UserGame>()
        .HasKey(ug => new { ug.UserId, ug.GameId });

    // Pivot Many-to-Many
    modelBuilder.Entity<UserGame>()
        .HasOne(ug => ug.User)
        .WithMany(u => u.UserGames)
        .HasForeignKey(ug => ug.UserId);

    modelBuilder.Entity<UserGame>()
        .HasOne(ug => ug.Game)
        .WithMany(g => g.UserGames)
        .HasForeignKey(ug => ug.GameId);
}
```

---

## 📸 Migration appliquée sur Azure SQL

![Migration Azure](../docs/images/migration-azure.png)

---

## 🔐 Sécurité

### Authentification JWT

L'API utilise un système d'authentification basé sur **JSON Web Tokens (JWT)** pour sécuriser les endpoints.
- Tokens signés de manière cryptographique.
- Validation rigoureuse de l'issuer, de l'audience et de la signature.
- Expiration automatique configurée.

### 🔑 Fonctionnement du flux
- Lors de l'inscription, un utilisateur est créé de manière unique en base de données.
- Lors de la connexion, un **token JWT signé** est généré par le serveur.
- Ce token doit être envoyé dans chaque requête protégée via l'en-tête HTTP suivant :

```http
Authorization: Bearer <token>
```

### ✔ Protection des secrets
- User Secrets activé (aucun jeton ni clé en clair dans le code).
- Aucun secret ou fichier de configuration sensible poussé sur GitHub.
- Connexions Azure SQL cryptées de bout en bout (`Encrypt=True`).

### ✔ HTTPS obligatoire
L'API force l'utilisation et la redirection systématique vers le protocole sécurisé HTTPS.

### ✔ Bonnes pratiques de conception
- Ne jamais stocker les mots de passe des utilisateurs en clair : utilisation de **BCrypt** pour le salage et le hashing.
- Ne jamais exposer les entités EF Core directement sur les contrôleurs afin de prévenir les injections de masse et les fuites de métadonnées (isolation totale via les **DTOs**).
- Architecture logicielle entièrement asynchrone (`async/await`) pour maximiser la disponibilité des threads du serveur web.

---

## 📘 Accès à la Documentation (Scalar)

Le pipeline OpenAPI et l'interface utilisateur Scalar sont initialisés dans le fichier `Program.cs`.

- **Adresse locale de développement** : `https://localhost:7000/scalar`

```csharp
builder.Services.AddOpenApi();

// Enregistrement automatique des validateurs du projet
builder.Services.AddValidatorsFromAssemblyContaining<UserGameCreationValidator>();

var app = builder.Build();

app.MapOpenApi();
app.UseScalar(options =>
{
    options.Title = "GameVerse API";
    options.Theme = ScalarTheme.Dark;
});
```

---

## 🚀 Déploiement

### 🌐 Déploiement possible sur :
- Azure App Service
- Azure Container Apps
- Docker + Azure Container Registry
- Kubernetes (AKS)

### 🧪 Étapes de déploiement locales (Docker)

```bash
docker build -t gameverse-api .
docker run -p 8080:80 gameverse-api
```

[⬅ Retour au README principal](../README.md)