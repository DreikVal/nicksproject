using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using System.Xml;

namespace FriendlyContent
{

    [ContentImporter(".collayer", DisplayName = "Collision Layer Importer", DefaultProcessor = "CollisionLayerProcessor")]
    public class CollisionLayerImporter : ContentImporter<XmlDocument>
    {
        public override XmlDocument Import(string filename, ContentImporterContext context)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            return doc;
        }
    }
}
