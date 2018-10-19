using System.Xml.Serialization;

namespace libMBIN.EXML {

    [XmlType( "Data" )]
    public class ExmlData : ExmlBase {

        [XmlAttribute( "template" )]
        public string Template { get; set; }

    }

}