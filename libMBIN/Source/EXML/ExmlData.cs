using System.Xml.Serialization;

namespace libMBIN.EXML {

    [XmlType( "Data" )]
    public class EXmlData : EXmlBase {

        [XmlAttribute( "template" )]
        public string Template { get; set; }

    }

}