﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace SysBot.ACNHOrders
{
    // src: https://github.com/foxbot/patek/blob/master/src/Patek/Modules/InfoModule.cs
    // ISC License (ISC)
    // Copyright 2017, Christopher F. <foxbot@protonmail.com>
    // ReSharper disable once UnusedType.Global
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        private const string detail = "I am an open source Discord bot powered by SysBot.NET, NHSE, ACNHMS and other open source software. Forked from berichan by _hedge";
        private const string repo = "https://github.com/CodeHedge/SysBot.ACNHOrders";

        [Command("info")]
        [Alias("about", "whoami", "owner")]
        public async Task InfoAsync()
        {
            var app = await Context.Client.GetApplicationInfoAsync().ConfigureAwait(false);

            var builder = new EmbedBuilder
            {
                Color = new Color(114, 137, 218),
                Description = detail,
            };

            builder.AddField("Info",
                $"- [Source Code]({repo})\n" +
                $"- {Format.Bold("Owner")}: {app.Owner} ({app.Owner.Id})\n" +
                $"- {Format.Bold("Library")}: Discord.Net ({DiscordConfig.Version})\n" +
                $"- {Format.Bold("Uptime")}: {GetUptime()}\n" +
                $"- {Format.Bold("Runtime")}: {RuntimeInformation.FrameworkDescription} {RuntimeInformation.ProcessArchitecture} " +
                $"({RuntimeInformation.OSDescription} {RuntimeInformation.OSArchitecture})\n" +
                $"- {Format.Bold("Buildtime")}: {GetBuildTime()}\n"
            );

            builder.AddField("Stats",
                $"- {Format.Bold("Heap Size")}: {GetHeapSize()}MiB\n" +
                $"- {Format.Bold("Guilds")}: {Context.Client.Guilds.Count}\n" +
                $"- {Format.Bold("Channels")}: {Context.Client.Guilds.Sum(g => g.Channels.Count)}\n" +
                $"- {Format.Bold("Users")}: {Context.Client.Guilds.Sum(g => g.Users.Count)}\n"
            );

            await ReplyAsync("Here's a bit about me!", embed: builder.Build()).ConfigureAwait(false);
        }

        private static string GetUptime() => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
        private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString(CultureInfo.CurrentCulture);

        private static string GetBuildTime()
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
                return DateTime.Now.ToString(@"yy-MM-dd\.hh\:mm");
            return File.GetLastWriteTime(assembly.Location).ToString(@"yy-MM-dd\.hh\:mm");
        }
    }
}
