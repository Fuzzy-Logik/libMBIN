using System.Xml.Serialization;

namespace libMBIN.EXML {

    [XmlType( "Property" )]
    public class ExmlProperty : ExmlBase {

        [XmlAttribute( "value" )]
        public string Value { get; set; }

    }

}