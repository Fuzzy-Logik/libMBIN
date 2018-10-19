using System.Xml.Serialization;

namespace libMBIN.EXML {

    [XmlType( "Property" )]
    public class EXmlProperty : EXmlBase {

        [XmlAttribute( "value" )]
        public string Value { get; set; }

    }

}