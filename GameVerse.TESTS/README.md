# 🧪 GameVerse.Tests

Suite de tests unitaires pour l'API GameVerse, développée avec **xUnit**, **FluentAssertions** *(si utilisé)* et le fournisseur **EF Core InMemory**.

[⬅ Retour au README principal](../README.md)

---

## 📑 Sommaire

- [Stack de test](#-stack-de-test)
- [Structure du projet](#-structure-du-projet)
- [Stratégie de test](#-stratégie-de-test)
- [Couverture actuelle](#-couverture-actuelle)
- [Lancer les tests](#-lancer-les-tests)

---

## 🧱 Stack de test

- **xUnit** — framework de test principal (`[Fact]`, `[Theory]`)
- **Microsoft.EntityFrameworkCore.InMemory** — simulation de la base de données pour tester les services sans dépendance à une vraie instance Azure SQL
- **BCrypt.Net-Next** — vérification directe du hashing des mots de passe dans les tests

---

## 🗂 Structure du projet

```
GameVerse.Tests/
│
├── Validators/
│   └── UpdateGameDtoValidatorTests.cs
│
├── Services/
│   ├── AuthServiceTests.cs             (génération JWT)
│   ├── AuthServiceLoginTests.cs        (connexion)
│   ├── AuthServiceRegisterTests.cs     (inscription)
│   └── AuthServiceRefreshTokenTests.cs (rotation de refresh token)
│   ├── GameServiceTests.cs             (CRUD jeux, bibliothèque utilisateur)
│   └── UserGameServiceTests.cs         (wishlist/library, favoris, notation, statistiques)
│
└── GameVerse.Tests.csproj
└── README.md
```

---

## 🎯 Stratégie de test

Les tests suivent une progression en couches, du plus simple au plus complexe :

1. **Validateurs (FluentValidation)** — logique pure, aucune dépendance externe.
2. **Logique métier isolée** — méthodes ne dépendant que de la configuration (ex. génération de JWT), simulée via `ConfigurationBuilder.AddInMemoryCollection()`.
3. **Services avec accès aux données** — testés via le fournisseur **EF Core InMemory** (`UseInMemoryDatabase`), avec une base isolée par test (identifiant GUID unique) pour éviter toute pollution entre tests.

Chaque méthode testée couvre systématiquement :
- Le **chemin de succès** (cas attendu).
- Les **cas d'échec métier** (email déjà utilisé, mot de passe incorrect, token expiré/révoqué, jeu non possédé…).
- Les **propriétés de sécurité**, quand pertinent (ex. vérifier que le mot de passe n'est jamais stocké en clair).
- Les **régressions connues** : certains tests documentent explicitement un bug corrigé (ex. un jeu en liste de souhaits ne doit jamais apparaître dans la bibliothèque), pour empêcher qu'il ne réapparaisse silencieusement.

---

## ✅ Couverture actuelle

| Composant | Scénarios couverts |
|---|---|
| `UpdateGameDtoValidator` | Titre vide/invalide, date hors plage (1970 → année+1), cas valide |
| `AuthService.GenerateJwtToken` | Claims corrects, expiration future |
| `AuthService.EmailExists` | Email existant, email inconnu |
| `AuthService.RegisterAsync` | Hashing du mot de passe, persistance en base |
| `AuthService.LoginAsync` | Email inconnu, mot de passe incorrect, connexion valide |
| `AuthService.RefreshTokenAsync` | Token inconnu, révoqué, expiré, rotation valide (ancien révoqué + nouveau émis) |
| `GameService` | CRUD complet, filtrage strict bibliothèque vs wishlist (non-régression) |
| `UserGameService.AddOrUpdateAsync` | Création, upsert sans doublon, préservation du statut favori |
| `UserGameService.ToggleFavoriteAsync` | Bascule sur entrée existante, rejet si jeu absent des listes |
| `UserGameService.UpdateRatingAsync` | Notation autorisée en bibliothèque uniquement, rejet sur wishlist/entrée inconnue |
| `UserGameService.GetFavoritesAsync` | Favoris indépendants du statut (bibliothèque ou wishlist) |
| `UserGameService.GetStatsAsync` | Compteurs, moyenne des notes, regroupement par genre |

**43 tests**, tous verts.

---

## 🚀 Lancer les tests

```bash
cd GameVerse.Tests
dotnet test
```
