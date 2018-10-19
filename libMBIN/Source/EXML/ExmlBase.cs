using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace libMBIN.EXML {

    [XmlInclude( typeof( ExmlData ) )]
    [XmlInclude( typeof( ExmlProperty ) )]
    [XmlInclude( typeof( ExmlMeta ) )]
    public abstract class ExmlBase {

        protected ExmlBase() {
            Elements = new List<ExmlBase>();
        }

        [XmlAttribute( "name" )]
        public string Name { get; set; }

        [XmlElement( typeof( ExmlData ), ElementName = "Data" )]
        [XmlElement( typeof( ExmlProperty ), ElementName = "Property" )]
        [XmlElement( typeof( ExmlMeta ), ElementName = "Meta" )]
        public List<ExmlBase> Elements { get; set; }

    }

}