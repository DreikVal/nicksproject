using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;

namespace FriendlyEngine
{
    public class Script
    {
        Dictionary<string, Conversation> conversations = 
            new Dictionary<string, Conversation>();

        public Conversation this[string name]
        {
            get { return conversations[name]; }
        }

        public Script(params Conversation[] newConversations)
        {
            foreach (Conversation c in newConversations)
                conversations.Add(c.Name, c);
        }
    }

    public class Conversation
    {
        string name;
        string text;
        public Collection<ConversationHandler> Handlers = 
            new Collection<ConversationHandler>();

        public string Name
        {
            get { return name; }
        }

        public string Text
        {
            get { return text; }
        }

        public Conversation(
            string name, 
            string text, 
            params ConversationHandler[] newHandlers)
        {
            this.name = name;
            this.text = text;
            foreach (ConversationHandler c in newHandlers)
                Handlers.Add(c);
        }
    }

    public class ConversationHandler
    {
        string caption;
        ConversationHandlerAction[] actions;

        public string Caption
        {
            get { return caption; }
        }

        public ConversationHandler(
            string caption,
            params ConversationHandlerAction[] actions)
        {
            this.caption = caption;
            this.actions = actions;

        }

        public void Invoke(NPC npc)
        {
            foreach(ConversationHandlerAction action in actions)
            action.Invoke(npc);
        }
    }

    public class ConversationHandlerAction
    {
        MethodInfo method;
        object[] parameters;

        public ConversationHandlerAction(string methodName, object[] parameters)
        {
            method = typeof(NPC).GetMethod(methodName);
            if(parameters != null)
                this.parameters = parameters;
        }

        public void Invoke(NPC npc)
        {
            method.Invoke(npc, parameters);
        }
    }
}
