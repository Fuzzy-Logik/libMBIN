using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace libMBIN.NMS {

    interface IDeserialize {

        object OnDeserialize( BinaryReader reader, Type field, NMSAttribute settings, long templatePosition, FieldInfo fieldInfo );

    }

}
