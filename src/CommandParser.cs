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
                    User user = Database.GetUser(message.Author.Id.ToString());

                    if (user == null)
                        await message.Channel.SendMessageAsync("Une erreur s'est produite durant la récupération de l'utilisateur");
                    else
                        await message.Channel.SendMessageAsync("Vous êtes :" + user.FirstName + " " + user.Name);
                    break;
                case "courses":
                    GetCoursesArguments(args, message);
                    break;
                case "help":
                    await message.Channel.SendMessageAsync("Aide :");
                    await message.Channel.SendMessageAsync("Toutes les commandes doivent commencé par \"!\".");
                    await message.Channel.SendMessageAsync("!me : Afficher les informations de l'utilisateur qui saisit la commande.");
                    await message.Channel.SendMessageAsync("!users list : Afficher la liste des utilisateurs.");
                    await message.Channel.SendMessageAsync("!users add <Prénom> <Nom> : Ajouter l'utilisateur qui saisi la commande dans la base de données.");
                    await message.Channel.SendMessageAsync("!users informations <Prénom> <Nom> : Récupérer les informations d'un utilisateur en se basant sur son nom et prénom.");
                    await message.Channel.SendMessageAsync("!courses list : Afficher la liste des cours.");
                    await message.Channel.SendMessageAsync(":courses add <Cours> : Ajouter un cours.");
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
                        await message.Channel.SendMessageAsync("Pas de cours enregistrés actuellement");
                    else
                    {
                        await message.Channel.SendMessageAsync("Affichage de la liste des cours enregistrés");

                        foreach (Course course in courses)
                            await message.Channel.SendMessageAsync("- ID= "+course.Id + " ~ " + course.Caption);
                    }
                    break;
                case "add":
                    if (Database.AddCourse(args[2]))
                        await message.Channel.SendMessageAsync("Le cours " + args[2] + " a été ajouté.");
                    else
                        await message.Channel.SendMessageAsync("Une erreur s'est produite pendant l'ajout du cours.");
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
                        await message.Channel.SendMessageAsync("Pas d'utilisateurs enregistrés actuellement");
                    else
                    {
                        await message.Channel.SendMessageAsync("Liste des utilisateurs enregistrés :");

                        foreach (User user in users)
                            await message.Channel.SendMessageAsync("    - " + user.FirstName + " " + user.Name + " (ID Discord : " + user.Id + ")"); 
                    }
                    break;
                case "add":
                    if (args.Length == 4)
                    {
                        if (Database.AddUser(message.Author.Id.ToString(), args[2], args[3]))
                            await message.Channel.SendMessageAsync("L'utilisateur " + args[2] + " " + args[3] + " a été ajouté.");
                        else
                            await message.Channel.SendMessageAsync("Vous avez déja ajouté un utilisateur avec ce compte ?");
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync("Erreur de synthax : !users add <Prénom> <Nom> : Ajouter l'utilisateur qui saisi la commande dans la base de données.");
                    }
                    
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
                        await message.Channel.SendMessageAsync("Affichage de la liste des utilisateurs enregistrés");

                        foreach (User user in users)
                            await message.Channel.SendMessageAsync("    - " + user.FirstName + " " + user.Name + " (ID Discord : " + user.Id + ")");
                    }
                    break;
                default:
                    await message.Channel.SendMessageAsync("Commande inconnue. !help pour afficher la liste des commandes");
                    break;
            }
        }
    }
}
