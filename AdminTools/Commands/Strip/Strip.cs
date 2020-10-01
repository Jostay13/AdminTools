﻿using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;

namespace AdminTools.Commands.Strip
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Strip : ParentCommand
    {
        public Strip() => LoadGeneratedCommands();

        public override string Command { get; } = "strip";

        public override string[] Aliases { get; } = new string[] { "stp" };

        public override string Description { get; } = "Clears a user or users inventories instantly";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            EventHandlers.LogCommandUsed((CommandSender)sender, EventHandlers.FormatArguments(arguments, 0));
            if (!CommandProcessor.CheckPermissions(((CommandSender)sender), "strip", PlayerPermissions.PlayersManagement, "AdminTools", false))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: strip ((player id / name) or (all / *))";
                return false;
            }

            switch (arguments.At(0))
            {
                case "*":
                case "all":
                    foreach (Player Ply in Player.List)
                        Ply.ClearInventory();

                    response = "Everyone's inventories have been cleared now";
                    return true;
                default:
                    Player Pl = Player.Get(arguments.At(0));
                    if (Pl == null)
                    {
                        response = $"Player not found: {arguments.At(0)}";
                        return false;
                    }

                    Pl.ClearInventory();
                    response = $"Player {Pl.Nickname}'s inventory have been cleared now";
                    return true;
            }
        }
    }
}
