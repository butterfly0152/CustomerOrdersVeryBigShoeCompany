using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProcessingLibrary
{
 public static class XMLDeserializer
 {
  public static BigShoeDataImport DeserializeBigShoeDataImport(Stream inputStream, ref string error)
  {
   try
   {
    XmlSerializer ser = new XmlSerializer(typeof(BigShoeDataImport));
    BigShoeDataImport dataImport = (BigShoeDataImport)ser.Deserialize(inputStream);
    return dataImport;
   }
   catch (Exception ex)
   {
    error = General.FormatInput(ex.Message);
    return null;
   }
  }

 }
}
