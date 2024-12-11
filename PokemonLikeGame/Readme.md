**Aper�u**  
ExerciceMonster est un jeu de combat de type Pok�mon d�velopp� en C# avec Windows Presentation Foundation (WPF). Le jeu permet aux joueurs de g�rer et de participer � des combats au tour par tour avec une vari�t� de monstres, en utilisant diff�rents sorts et techniques pour vaincre leurs adversaires.

**Fonctionnalit�s**  
Authentification utilisateur : Connectez-vous avec un nom d'utilisateur et un mot de passe  
Gestion des monstres : Affichez et s�lectionnez des monstres pour le combat, avec des d�tails sur la sant� et les capacit�s de chaque monstre  
Grimoire : Acc�dez � une liste de sorts disponibles avec des d�tails sur leurs effets  
Syst�me de combat : Participez � des batailles au tour par tour contre des adversaires automatis�s  
Param�tres : G�rez les param�tres de l'application, notamment les d�tails de connexion � la base de donn�es, consultez et actualisez les informations de la base directement depuis l'interface utilisateur  

**Pr�requis**  
Vous devez disposer de  
.NET Framework 4.7.2 ou une version ult�rieure  
SQL Server Express (LocalDB ou SQL Server classique)  

**Configuration de la base de donn�es**  
Assurez-vous que votre serveur SQL est en cours d'ex�cution  
Mettez � jour la cha�ne de connexion dans le fichier *App.config* pour qu'elle pointe vers votre instance SQL Server  
Ex�cutez le script fourni dans le fichier *DatabaseScript.sql* pour configurer le sch�ma de la base de donn�es et les donn�es initiales  

**Comment utiliser**  
�cran de connexion : Entrez votre nom d'utilisateur et votre mot de passe  
Gestion des monstres : S�lectionnez un monstre dans la liste pour afficher ses d�tails ou pour le choisir pour un combat  
�cran des sorts : Parcourez les sorts disponibles  
�cran de combat : Participez � un combat en s�lectionnant des sorts et en observant le d�roulement du combat au tour par tour  
Mouvements : Utilisez les fl�ches pour vous d�placer sur la carte  
�cran des param�tres : Ajustez les param�tres de l'application, y compris la cha�ne de connexion � la base de donn�es. Utilisez le bouton "Appliquer" pour enregistrer les modifications  