using System.Xml.Serialization;

namespace libMBIN.EXML {

    [XmlType( "Meta" )]
    public class EXmlMeta : EXmlBase {

        [XmlAttribute( "comment" )]
        public string Comment { get; set; }

    }

}