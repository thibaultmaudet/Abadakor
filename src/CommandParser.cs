using Abadakor.Models;
using Discord.WebSocket;
using System;
using System.Collections.Generic;

namespace Abadakor
{
    public class CommandParser
    {
        public static Database Database => Database.Instance;

        public static async void Parse(SocketMessage message)
        {
            string[] args = message.Content.Remove(0,1).Split(' ');

            switch (args[0])
            {
                case "users":
                    GetUsersArguments(args, message);
                    break;
                case "me":
                    User user = Database.GetUser(message.Author.Username.ToString());

                    if (user == null)
                        await message.Channel.SendMessageAsync("Une erreur s'est produite durant la récupération de l'utilisateur");
                    else
                        await message.Channel.SendMessageAsync("Vous êtes : " + user.FirstName + " " + user.Name);
                    break;
                case "courses":
                    GetCoursesArguments(args, message);
                    break;
                case "help":
                    await message.Channel.SendMessageAsync("Aide :" + Environment.NewLine + "Toutes les commandes doivent commencé par \"!\"." + Environment.NewLine + "!me : Afficher les informations de l'utilisateur qui saisit la commande." + "!users list : Afficher la liste des utilisateurs." + Environment.NewLine + "!users add <Prénom> <Nom> : Ajouter l'utilisateur qui saisi la commande dans la base de données." + "!users informations <Prénom> <Nom> : Récupérer les informations d'un utilisateur en se basant sur son nom et prénom." + Environment.NewLine + "!courses list : Afficher la liste des cours." + Environment.NewLine + "!courses add <Cours> : Ajouter un cours." + Environment.NewLine + "!courses associate <IDCours> : Associer un utilisateur avec un cours." + Environment.NewLine + "!courses mine : Lister les cours associés à l'utilisateur courant." + Environment.NewLine + "!courses check <IDCours> : Cocher un cours pour l'utilisateur courant." + Environment.NewLine + "!courses uncheck <IDCours> : Décocher un cours pour l'utilisateur courant.");
                    break;
                default:
                    await message.Channel.SendMessageAsync("Commande inconnue. !help pour afficher la liste des commandes");
                    break;
            }
        }

        private static async void GetCoursesArguments(string[] args, SocketMessage message)
        {
            switch(args[1])
            {
                case "list":
                    List<Course> courses = Database.GetCourses();

                    if (courses == null)
                    {
                        await message.Channel.SendMessageAsync("Une erreur s'est produite pendant la récupération de la liste des cours.");

                        return;
                    }

                    if (courses.Count == 0)
                        await message.Channel.SendMessageAsync("Je n'arrive pas à trouver de cours d'enregistré.");
                    else
                    {
                        string coursesList = "Voici la liste des cours qui j'ai réussi à récupérer :";

                        foreach (Course c in courses)
                            coursesList += Environment.NewLine + "- " + c.Caption + " ~ ID du cours : " + c.Id + ".";

                        await message.Channel.SendMessageAsync(coursesList);
                    }
                    break;
                case "add":
                    if (Database.AddCourse(args[2]))
                        await message.Channel.SendMessageAsync("Le cours " + args[2] + " a été ajouté.");
                    else
                        await message.Channel.SendMessageAsync("Une erreur s'est produite pendant l'ajout du cours.");
                    break;
                case "informations":
                    if (args.Length == 3)
                    {
                        Course course = Database.GetCourse(args[2]);

                        if (course != null)
                            await message.Channel.SendMessageAsync("Cours : " + course.Caption + "(ID : " + course.Id + ")");
                        else
                            await message.Channel.SendMessageAsync("Impossible de récupérer les informations de ce cours. Est-ce que le nom est correct ?");
                    }
                    else
                        await message.Channel.SendMessageAsync("Structure de la commande : !courses informations <Nom>");
                    break;
                case "associate":
                    if (args.Length == 3)
                    {
                        int.TryParse(args[2], out int result);

                        if (Database.CreateAssociation(message.Author.Id.ToString(), result))
                            await message.Channel.SendMessageAsync("L'association du cours " + Database.GetCourse(result).Caption +" a réussi");
                        else
                            await message.Channel.SendMessageAsync("Une erreur s'est produite pendant l'association d'un utilisateur et d'un cours.");
                    }
                    else
                        await message.Channel.SendMessageAsync("Structure de la commande : !courses associate <IDCours>");
                    break;
                case "mine":
                    courses = Database.GetCourses(message.Author.Id.ToString());

                    if (courses == null)
                    {
                        await message.Channel.SendMessageAsync("Une erreur s'est produite pendant la récupération de la liste des cours.");

                        return;
                    }

                    if (courses.Count == 0)
                        await message.Channel.SendMessageAsync("Pas de cours associés à l'utilisateur actuellement");
                    else
                    {
                        string stateCourse = "Voici la liste des cours enregistrés que j'ai réussi à récupérer.";


                        foreach (Course c in courses)
                            if (c.State == 0)
                                stateCourse += Environment.NewLine + "- Etat du cours " + c.Caption + " : :x: (ID:" + c.Id + ")";
                            else if(c.State==1)
                                 stateCourse += Environment.NewLine + "- Etat du cours " + c.Caption + " : :white_check_mark: (ID:" + c.Id + ")";

                        await message.Channel.SendMessageAsync(stateCourse);
                    }
                    break;
                case "check":
                    int.TryParse(args[2], out int checkedCourseId);

                    if (Database.UpdateState(message.Author.Id.ToString(), checkedCourseId, 1))
                        await message.Channel.SendMessageAsync("Le cours " + Database.GetCourse(checkedCourseId).Caption + " a été coché.");
                    else
                        await message.Channel.SendMessageAsync("Une erreur s'est produite pendant la mise à jour du statut du cours.");
                    break;
                case "uncheck":
                    int.TryParse(args[2], out int uncheckedCourseId);

                    if (Database.UpdateState(message.Author.Id.ToString(), uncheckedCourseId, 0))
                        await message.Channel.SendMessageAsync("Le cours " + Database.GetCourse(uncheckedCourseId).Caption + " a été décoché.");
                    else
                        await message.Channel.SendMessageAsync("Une erreur s'est produite pendant la mise à jour du statut du cours.");
                    break;
                default:
                    await message.Channel.SendMessageAsync("Commande inconnue. !help pour afficher la liste des commandes");
                    break;
            }
        }

        private static async void GetUsersArguments(string[] args, SocketMessage message)
        {
            switch(args[1])
            {
                case "list":
                    List<User> users = Database.GetUsers();

                    if (users == null)
                    {
                        await message.Channel.SendMessageAsync("Une erreur s'est produite pendant la récupération de la liste des utilisateurs.");

                        return;
                    }

                    if (users.Count == 0)
                        await message.Channel.SendMessageAsync("Je n'arrive pas à trouver des utilisateurs enregistrés.");
                    else
                    {
                        string usersList = "Voici la liste des utilisateurs que j'ai pu récupérer :";


                        foreach (User user in users)
                            usersList += Environment.NewLine + "- " + user.FirstName + " " + user.Name + " ~ ID Discord de l'utilisateur : " + user.Id + ".";

                        await message.Channel.SendMessageAsync(usersList); 
                    }
                    break;
                case "add":
                    if (args.Length == 4)
                    {
                        if (Database.AddUser(message.Author.Id.ToString(), args[2], args[3]))
                            await message.Channel.SendMessageAsync("L'utilisateur " + args[2] + " " + args[3] + " a été ajouté.");
                        else
                            await message.Channel.SendMessageAsync("Vous n'auriez pas déjà ajouté un utilisateur avec ce compte ?");
                    }
                    else
                        await message.Channel.SendMessageAsync("Structure de la commande : !users add <Prénom> <Nom>");
                    break;
                case "informations":
                    users = Database.GetUsers(args[2], args[3]);

                    if (users == null)
                    {
                        await message.Channel.SendMessageAsync("Une erreur s'est produite pendant la récupération de la liste des utilisateurs.");

                        return;
                    }

                    if (users.Count == 0)
                        await message.Channel.SendMessageAsync("Pas d'utilisateurs enregistrés sous ce nom actuellement");
                    else
                    {
                        string usersList = "Voici la liste des utilisateur portant ce nom que j'ai réussi à trouver :";
                        
                        foreach (User user in users)
                            usersList += Environment.NewLine + "- " + user.FirstName + " " + user.Name + " ~ ID Discord de l'utilisateur : " + user.Id + ".";

                        await message.Channel.SendMessageAsync(usersList);
                    }
                    break;
                default:
                    await message.Channel.SendMessageAsync("Commande inconnue. !help pour afficher la liste des commandes");
                    break;
            }
        }
    }
}
