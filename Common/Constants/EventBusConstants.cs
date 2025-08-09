using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusMessages.Constants
{
    public static class EventBusConstants
    {
        public const string UserCreatedQueue = "usercreated-queue";
        public const string UserCreatedReplicationToRelationServiceQueue = "usercreated-repl-queue";
        public const string UserCreatedReplicationToChatServiceQueue = "usercreated-chat-queue";
    }
}
