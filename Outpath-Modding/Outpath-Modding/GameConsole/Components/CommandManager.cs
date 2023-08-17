using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Outpath_Modding.GameConsole.Components
{
    public static class CommandManager
    {
        public static List<ICommand> Commands = new List<ICommand>();

        public static void AddCommand(ICommand command)
        {
            try
            {
                foreach (string abr in command.Abbreviate)
                {
                    if (Commands.Exists(x => x.Abbreviate.Contains(abr)))
                    {
                        Logger.Error($"An error occurred when adding the \"{command.Command}\" command with \"{command.GetType().Assembly.FullName}\" mod, as such a abbreviate already exists");
                        return;
                    }
                }

                if (Commands.Exists(x => x.Command == command.Command))
                {
                    Logger.Error($"An error occurred when adding the \"{command.Command}\" command with \"{command.GetType().Assembly.FullName}\" mod, as such a command already exists");
                    return;
                }

                Commands.Add(command);

                //Logger.Info($"Successfully added \"{command.Command}\" command with \"{command.GetType().Assembly.FullName}\" mod");
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occurred when adding the \"{command.Command}\" command with \"{command.GetType().Assembly.FullName}\" mod: \n {ex}");
            }
        }

        public static void RemoveCommand(ICommand command)
        {
            try
            {
                if (!Commands.Exists(x => x.Command == command.Command || x.Abbreviate == command.Abbreviate || x.Description == command.Description))
                {
                    Logger.Error($"An error occurred when removing the command \"{command.Command}\" because the command does not exist");
                    return;
                }

                Commands.Remove(command);

                //Logger.Info($"Successfully removed \"{command.Command}\" command with \"{command.GetType().Assembly.FullName}\" mod");
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occurred when removeing the \"{command.Command}\" command with \"{command.GetType().Assembly.FullName}\" mod: \n {ex}");
            }
        }

        public static void OnSendCommand(string text)
        {
            List<string> args = text.Split(new char[] { ' ' }).ToList();

            foreach (ICommand command in Commands)
            {
                if (command.Command != args[0] && !command.Abbreviate.Contains(args[0])) continue;

                args.Remove(args[0]);

                if (command.Execute(args.ToArray(), out string reply))
                    Logger.Message("[Command] " + reply, Color.green);
                else Logger.Message("[Command] " + reply, Color.red);
            }
        }
    }
}
