**Aperçu**  
ExerciceMonster est un jeu de combat de type Pokémon développé en C# avec Windows Presentation Foundation (WPF). Le jeu permet aux joueurs de gérer et de participer à des combats au tour par tour avec une variété de monstres, en utilisant différents sorts et techniques pour vaincre leurs adversaires.

**Fonctionnalités**  
Authentification utilisateur : Connectez-vous avec un nom d'utilisateur et un mot de passe  
Gestion des monstres : Affichez et sélectionnez des monstres pour le combat, avec des détails sur la santé et les capacités de chaque monstre  
Grimoire : Accédez à une liste de sorts disponibles avec des détails sur leurs effets  
Système de combat : Participez à des batailles au tour par tour contre des adversaires automatisés  
Paramètres : Gérez les paramètres de l'application, notamment les détails de connexion à la base de données, consultez et actualisez les informations de la base directement depuis l'interface utilisateur  

**Prérequis**  
Vous devez disposer de  
.NET Framework 4.7.2 ou une version ultérieure  
SQL Server Express (LocalDB ou SQL Server classique)  

**Configuration de la base de données**  
Assurez-vous que votre serveur SQL est en cours d'exécution  
Mettez à jour la chaîne de connexion dans le fichier *App.config* pour qu'elle pointe vers votre instance SQL Server  
Exécutez le script fourni dans le fichier *DatabaseScript.sql* pour configurer le schéma de la base de données et les données initiales  

**Comment utiliser**  
Écran de connexion : Entrez votre nom d'utilisateur et votre mot de passe  
Gestion des monstres : Sélectionnez un monstre dans la liste pour afficher ses détails ou pour le choisir pour un combat  
Écran des sorts : Parcourez les sorts disponibles  
Écran de combat : Participez à un combat en sélectionnant des sorts et en observant le déroulement du combat au tour par tour  
Mouvements : Utilisez les flèches pour vous déplacer sur la carte  
Écran des paramètres : Ajustez les paramètres de l'application, y compris la chaîne de connexion à la base de données. Utilisez le bouton "Appliquer" pour enregistrer les modifications  