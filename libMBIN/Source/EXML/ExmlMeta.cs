using System.Xml.Serialization;

namespace libMBIN.EXML {

    [XmlType( "Meta" )]
    public class ExmlMeta : ExmlBase {

        [XmlAttribute( "comment" )]
        public string Comment { get; set; }

    }

}