using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace libMBIN.EXML {

    [XmlInclude( typeof( EXmlData ) )]
    [XmlInclude( typeof( EXmlProperty ) )]
    [XmlInclude( typeof( EXmlMeta ) )]
    public abstract class EXmlBase {

        protected EXmlBase() {
            Elements = new List<EXmlBase>();
        }

        [XmlAttribute( "name" )]
        public string Name { get; set; }

        [XmlElement( typeof( EXmlData ), ElementName = "Data" )]
        [XmlElement( typeof( EXmlProperty ), ElementName = "Property" )]
        [XmlElement( typeof( EXmlMeta ), ElementName = "Meta" )]
        public List<EXmlBase> Elements { get; set; }

    }

}