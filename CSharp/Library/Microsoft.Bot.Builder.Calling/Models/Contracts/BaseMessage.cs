﻿using System;
using System.Net.Mime;
using System.Text.RegularExpressions;
using Microsoft.Bot.Builder.Calling.ObjectModel.Misc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Bot.Builder.Calling.ObjectModel.Contracts
{
    /// <summary>
    /// Base message wrapper for all API messages.  The format is:
    /// {
    ///    type: (type-of-message),
    ///    to: (to-user-MRI),
    ///    from: (from-user-MRI),
    ///    (optional additional-message-specific-properties),
    ///    (optional additional-message-specific-properties),
    ///    (optional additional-message-specific-properties),
    /// }
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public abstract class BaseMessage
    {
        /// <summary>
        /// Weak MRI validation.  There are other more robust MRI checkers but they
        /// can't be refereneced here.  This is just a weak check to make sure the
        /// format is 1-8 digits followed by a colon followed by 1-512 characters.
        /// </summary>
        private static Regex MriWeakRegex = new Regex("^\\d{1,8}\\:.{1,512}$", RegexOptions.Compiled);

        public static readonly ContentType JsonContentType = new ContentType("application/json");
        public const string ToHeaderKey = "to";
        public const string FromHeaderKey = "from";
        public const string MessageTypeHeaderKey = "type";

        protected BaseMessage(string messageType)
        {
            Utils.AssertArgument(!string.IsNullOrWhiteSpace(messageType), "messageType is missing.");
            this.MessageType = messageType;
        }

        [JsonProperty(PropertyName = "type", Required = Required.Always, Order = -900)]
        public string MessageType { get; set; }

        [JsonProperty(PropertyName = "to", Required = Required.Always, Order = -800)]
        public string To { get; set; }

        [JsonProperty(PropertyName = "from", Required = Required.Always, Order = -700)]
        public string From { get; set; }

        public void Validate()
        {
            VerifyPropertyExists(this.MessageType, "MessageType");
            ValidateMri(this.To, "To");
            ValidateMri(this.From, "From");
            this.ValidateInternal();
        }

        public static string GetMessageType(string json)
        {
            JObject jObj;
            try
            {
                jObj = JObject.Parse(json);
            }
            catch (JsonReaderException)
            {
                return null;
            }

            JToken typeToken = jObj[MessageTypeHeaderKey];
            return typeToken == null ? null : typeToken.ToString();
        }

        protected static void VerifyPropertyExists(string value, string name)
        {
            Utils.AssertArgument(!string.IsNullOrWhiteSpace(value), name + " is missing.");
        }

        protected static void ValidateMri(string value, string name)
        {
            VerifyPropertyExists(value, name);
            if (!MriWeakRegex.IsMatch(value))
            {
                throw new ArgumentException(name + " is not a valid MRI.");
            }
        }

        protected abstract void ValidateInternal();
    }
}
