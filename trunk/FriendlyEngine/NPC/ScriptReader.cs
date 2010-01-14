using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FriendlyEngine
{
    public class ScriptReader : ContentTypeReader<Script>
    {
        protected override Script Read(
            ContentReader input, 
            Script existingInstance)
        {
            int conversationCount = input.ReadInt32();
            Conversation[] conversations = new Conversation[conversationCount];
            for (int i = 0; i < conversationCount; i++)
                conversations[i] = input.ReadObject<Conversation>();

            return new Script(conversations);
        }
    }
    public class ConversationReader : ContentTypeReader<Conversation>
    {
        protected override Conversation Read(
            ContentReader input,
            Conversation existingInstance)
        {
            string name = input.ReadString();
            string text = input.ReadString();

            int handlerCount = input.ReadInt32();
            ConversationHandler[] handlers = new ConversationHandler[handlerCount];
            for (int i = 0; i < handlerCount; i++)
                handlers[i] = input.ReadObject<ConversationHandler>();

            return new Conversation(name, text, handlers);
        }
    }
    public class ConversationHandlerReader : ContentTypeReader<ConversationHandler>
    {
        protected override ConversationHandler Read(
            ContentReader input,
            ConversationHandler existingInstance)
        {
            string caption = input.ReadString();

            int actionCount = input.ReadInt32();
            ConversationHandlerAction[] actions = new ConversationHandlerAction[actionCount];
            for (int i = 0; i < actionCount; i++)
                actions[i] = input.ReadObject<ConversationHandlerAction>();

            return new ConversationHandler(caption, actions);
        }
    }

    public class ConversationHandlerActionReader : ContentTypeReader<ConversationHandlerAction>
    {
        protected override ConversationHandlerAction Read(
            ContentReader input,
            ConversationHandlerAction existingInstance)
        {
            string methodName = input.ReadString();
            object[] parameters = input.ReadObject<object[]>();

            return new ConversationHandlerAction(methodName, parameters);
        }
    }
}
