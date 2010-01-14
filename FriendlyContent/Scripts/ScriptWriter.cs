using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace FriendlyContent
{
    [ContentTypeWriter]
    public class ScriptWriter : ContentTypeWriter<ScriptContent>
    {
        protected override void Write(ContentWriter output, ScriptContent value)
        {

            output.Write(value.Conversations.Count);
            foreach (ConversationContent c in value.Conversations)
                output.WriteObject(c);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "FriendlyEngine.ScriptReader, FriendlyEngine";
        }
    }
    [ContentTypeWriter]
    public class ConversationWriter : ContentTypeWriter<ConversationContent>
    {
        protected override void Write(ContentWriter output, ConversationContent value)
        {
            output.Write(value.Name);
            output.Write(value.Text);

            output.Write(value.Handlers.Count);
            foreach (ConversationHandlerContent c in value.Handlers)
                output.WriteObject(c);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "FriendlyEngine.ConversationReader, FriendlyEngine";
        }
    }
    [ContentTypeWriter]
    public class ConversationHandlerWriter : ContentTypeWriter<ConversationHandlerContent>
    {
        protected override void Write(ContentWriter output, ConversationHandlerContent value)
        {
            output.Write(value.Caption);
            output.Write(value.Actions.Count);
            foreach (ConversationHandlerActionContent a in value.Actions)
                output.WriteObject(a);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "FriendlyEngine.ConversationHandlerReader, FriendlyEngine";
        }
    }
    [ContentTypeWriter]
    public class ConversationHandlerActionWriter : ContentTypeWriter<ConversationHandlerActionContent>
    {
        protected override void Write(
            ContentWriter output, 
            ConversationHandlerActionContent value)
        {
            output.Write(value.MethodName);
            output.WriteObject(value.Parameters);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "FriendlyEngine.ConversationHandlerActionReader, FriendlyEngine";
        }
    }
}
