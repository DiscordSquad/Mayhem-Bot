using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mayhem_Bot.Core
{
    public static class MessageHandler
    {
        public enum MessageType
        {
            Info,
            Error,
            Warning,
            Question,
            Success
        }
        private static Color INFO_COLOR = Color.Gold;
        private static Color ERROR_COLOR = Color.Red;
        private static Color WARNING_COLOR = Color.DarkRed;
        private static Color QUESTION_COLOR = Color.Purple;
        private static Color SUCCESS_COLOR = Color.Green;

        /// <summary>
        /// Creates an embed channel message async
        /// </summary>
        /// <param name="type"></param>
        /// <param name="channel"></param>
        /// <param name="Title"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static async Task SendEmbededMessageAsync(MessageType type, ISocketMessageChannel channel, string Title, string Content)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithTitle(Title + "  :x:");
            eb.WithDescription(Content);
            switch (type)
            {
                case MessageType.Info:
                    eb.WithColor(INFO_COLOR);
                    eb.WithThumbnailUrl(CoreProgram._client.CurrentUser.GetAvatarUrl());
                    eb.WithCurrentTimestamp();
                    break;
                case MessageType.Error:
                    eb.WithColor(ERROR_COLOR);
                    eb.WithUrl("http://google.de");
                    eb.WithFooter("Please type '!help' or click the Title for more info");
                    break;
                case MessageType.Warning:
                    eb.WithColor(WARNING_COLOR);
                    break;
                case MessageType.Question:
                    eb.WithColor(QUESTION_COLOR);
                    break;
                case MessageType.Success:
                    eb.WithColor(SUCCESS_COLOR);
                    break;
            }
            await channel.SendMessageAsync("", false, eb);
        }
        /// <summary>
        /// Creates an embed DM message async
        /// </summary>
        /// <param name="type"></param>
        /// <param name="channel"></param>
        /// <param name="Title"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static async Task SendEmbededDMMessageAsync(MessageType type, IDMChannel channel, string Title, string Content)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithTitle(Title);
            eb.WithDescription(Content);
            switch (type)
            {
                case MessageType.Info:
                    eb.WithColor(INFO_COLOR);
                    eb.WithThumbnailUrl(CoreProgram._client.CurrentUser.GetAvatarUrl());
                    eb.WithCurrentTimestamp();
                    break;
                case MessageType.Error:
                    eb.WithTitle(Title + "  :x:");
                    eb.WithColor(ERROR_COLOR);
                    eb.WithUrl("http://google.de");
                    eb.WithFooter("Please type '!help' or click the Title for more info");
                    break;
                case MessageType.Warning:
                    eb.WithColor(WARNING_COLOR);
                    break;
                case MessageType.Question:
                    eb.WithColor(QUESTION_COLOR);
                    break;
                case MessageType.Success:
                    eb.WithColor(SUCCESS_COLOR);
                    break;
            }
            await channel.SendMessageAsync("", false, eb);
        }
        /// <summary>
        /// Get an EmbedBuilder object for the default layout
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static EmbedBuilder GetDefaultBuilder(MessageType type)
        {
            EmbedBuilder eb = new EmbedBuilder();
            switch (type)
            {
                case MessageType.Info:
                    eb.WithColor(INFO_COLOR);
                    eb.WithThumbnailUrl(CoreProgram._client.CurrentUser.GetAvatarUrl());
                    eb.WithCurrentTimestamp();
                    break;
                case MessageType.Error:
                    eb.WithColor(ERROR_COLOR);
                    eb.WithUrl("http://google.de");
                    eb.WithFooter("Please type '!help' or click the Title for more info");
                    break;
                case MessageType.Warning:
                    eb.WithColor(WARNING_COLOR);
                    break;
                case MessageType.Question:
                    eb.WithColor(QUESTION_COLOR);
                    break;
                case MessageType.Success:
                    eb.WithColor(SUCCESS_COLOR);
                    break;
            }
            return eb;
        }
    }
}
