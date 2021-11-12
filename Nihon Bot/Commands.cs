using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NihonBot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nihon_Bot
{
    public class Commands : ModuleBase
    {        
        public EmbedBuilder ErrorEmbed(string Error)
        {
            var Embed = new EmbedBuilder()
            {
                Timestamp = DateTimeOffset.Now,
                ThumbnailUrl = null,
                Title = $"❌ Bot Error",
                Description = $"```csharp\n{Error}\n```",
                Color = new Color(205, 50, 85)
            };
            return Embed;
        }

        public class Admin : ModuleBase
        {
            /* If Kick Or Ban Reason == null Then Will Be Replaced With "Reason Not Specified" */

            [Command("Ban")]
            public async Task Ban(IGuildUser User, [Remainder] string Reason = "Reason Not Specified")
            {
                if (User.GuildPermissions.BanMembers == true) await Context.Guild.AddBanAsync(User, 7, Reason); /* Ban User / Delete Messages Within 7 Days And Add Reason To Ban */
                else await ReplyAsync($"{Utility.GetEmote("Close", "906777653669941268")} You Do Not Have Ban Members Permission"); /* Permissions Not Met */
            }

            [Command("Kick")]
            public async Task Kick(IGuildUser User, [Remainder] string Reason = "Reason Not Specified")
            {
                if (User.GuildPermissions.KickMembers == true) await User.KickAsync(Reason); /* Kick User And Add Reason */ 
                else await ReplyAsync($"{Utility.GetEmote("Close", "906777653669941268")} You Do Not Have Kick Members Permission"); /* Permissions Not Met */
            }

            [Command("Purge")]
            public async Task Purge(int Amount)
            {
                if (Context.User is SocketGuildUser User) /* Cast Context.User -> SocketGuildUser */
                {
                    if (User.GuildPermissions.ManageMessages == true) /* If Manage Messages Permission Returns True */
                    {
                        var Messages = await Context.Channel.GetMessagesAsync(Amount + 1).FlattenAsync(); /* Get Messages */
                        await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(Messages); /* Delete Messages */
                    }
                }
                else await ReplyAsync($"{Utility.GetEmote("Close", "906777653669941268")} You Do Not Have Manage Messages Permission"); /* If User Dosen't Have Permissions */
            }
        }

        internal class User : ModuleBase
        {
            [Command("User")]
            public async Task UserInfo(IUser UserAccount)
            {
                if (UserAccount == null) UserAccount = Context.User;

                var Embed = new EmbedBuilder
                {
                    Timestamp = DateTimeOffset.Now,
                    ThumbnailUrl = UserAccount.GetAvatarUrl(ImageFormat.Auto, 128),
                    Title = $"User Info - {UserAccount.Username}#{UserAccount.Discriminator}",
                    Fields =
                    {
                        new EmbedFieldBuilder()
                        {
                            Name = ":id: User Id:",
                            Value = UserAccount.Id,
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = $"{Utility.GetEmote("Online", "906801384727379988")} Status:",
                            Value = UserAccount.Status,
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = ":date: Creation Date:",
                            Value = $"{UserAccount.CreatedAt.Month}/{UserAccount.CreatedAt.Day}/{UserAccount.CreatedAt.Year}",
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = ":computer: Activity:",
                            Value = UserAccount.Activity,
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = $"{Utility.GetEmote("DiscordWhite", "906765565648048138")} Is Bot / Webhook:",
                            Value = $"Bot - {UserAccount.IsBot}\nWebhook - {UserAccount.IsWebhook}",
                            IsInline = true
                        }
                    },
                    Color = new Color(205, 50, 85)
                };
                await ReplyAsync($"{Utility.GetEmote("Info", "906767083050778644")} Info | {UserAccount.Username}#{UserAccount.Discriminator}", false, Embed.Build());
            }

            [Command("Download")]
            public async Task Download()
            {
                var Embed = new EmbedBuilder
                {
                    Timestamp = DateTimeOffset.Now,
                    ThumbnailUrl = "https://cdn.discordapp.com/attachments/893079592175280128/906303326365286420/Nihon_Logo_Transparent.png",
                    Title = "Nihon Official Download",
                    Fields =
                {
                    new EmbedFieldBuilder()
                    {
                        Name = $"{Utility.GetEmote("Nihon", "906765461067284480")} Download:",
                        Value = "[Click Here](https://wearedevs.net/d/Nihon)",
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = $"{Utility.GetEmote("WeAreDevs", "906765511776419950")} Latest Version:",
                        Value = "5.6.0",
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = $"{Utility.GetEmote("Github", "906779060976353280")} Github Organization:",
                        Value = "[Github Link](https://github.com/Nihon-Development)",
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = ":tools: File Version",
                        Value = "version-f98ef77f473148f6",
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = $"{Utility.GetAnimatedEmote("VerifyBlue", "906777655750328342")} Developer:",
                        Value = "ImmuneLion318#0001",
                        IsInline = true
                    }
                },
                    Color = new Color(205, 50, 85)
                };
                await ReplyAsync(null, false, Embed.Build());
            }
        }
    }
}
