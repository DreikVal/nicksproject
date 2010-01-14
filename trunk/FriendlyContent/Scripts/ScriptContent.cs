using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;

namespace FriendlyContent
{
    public class ScriptContent
    {
        public Collection<ConversationContent> Conversations =
            new Collection<ConversationContent>();
    }

    public class ConversationContent
    {
        public string Name;
        public string Text;
        public Collection<ConversationHandlerContent> Handlers =
            new Collection<ConversationHandlerContent>();
    }

    public class ConversationHandlerContent
    {
        public string Caption;
        public List<ConversationHandlerActionContent> Actions =
            new List<ConversationHandlerActionContent>();
    }

    public class ConversationHandlerActionContent
    {
        public string MethodName;
        public object[] Parameters;
    }
}
