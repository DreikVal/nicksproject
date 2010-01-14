using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.Xml;

namespace FriendlyContent
{
    [ContentProcessor(DisplayName = "NPC Script Processor")]
    public class ScriptProcessor : ContentProcessor<XmlDocument, ScriptContent>
    {
        public override ScriptContent Process(
            XmlDocument input, 
            ContentProcessorContext context)
        {
            ScriptContent script = new ScriptContent();

            XmlNodeList conversations = input.GetElementsByTagName("Conversation");

            foreach (XmlNode node in conversations)
            {
                ConversationContent c = new ConversationContent();
                c.Name = node.Attributes["Name"].Value;
                c.Text = node.FirstChild.InnerText;

                foreach (XmlNode handlernode in node.LastChild.ChildNodes)
                {
                    ConversationHandlerContent h = new ConversationHandlerContent();
                    h.Caption = handlernode.Attributes["Caption"].Value;

                    string action = handlernode.Attributes["Action"].Value;

                    string[] methods = action.Split(';');

                    foreach (string m in methods)
                    {
                        string trimmedMethodName = m.Trim();

                        ConversationHandlerActionContent a =
                            new ConversationHandlerActionContent();


                        if (trimmedMethodName.Contains(":"))
                        {
                            string[] actionSplit = trimmedMethodName.Split(':');
                            a.MethodName = actionSplit[0];
                            a.Parameters = (object[])actionSplit[1].Split(',');
                        }
                        else
                        {
                            a.MethodName = trimmedMethodName;
                            a.Parameters = null;
                        }

                        h.Actions.Add(a);
                    }
                    c.Handlers.Add(h);
                }

                script.Conversations.Add(c);
            }
            return script;
        }
    }
}