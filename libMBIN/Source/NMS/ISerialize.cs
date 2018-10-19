using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace libMBIN.NMS {

    interface ISerialize {

        bool OnSerialize( BinaryWriter writer, Type field, object fieldData, NMSAttribute settings, FieldInfo fieldInfo, ref List<Tuple<long, object>> additionalData, ref int addtDataIndex );

    }

}
