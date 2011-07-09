using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
/// <summary>
/// Store objects at absolute zero, and thaw for use as required
/// </summary>
/// <remarks>
/// Kelvin can serialize and deserialize any type of object 
/// (well, it has to be serializable to start with - I just mean it uses Generics)
/// Use Xml (String) or Binary (Byte[]) datastructures or read/write files of both types.
/// </remarks>
/// <typeparam name="T">Type/Class that you wish to serialize/deserialize/</typeparam>
/// <example>
/// Kelvin is a *static* class to provide simple, single-line, typed access to Serialization:
/// <code><![CDATA[
/// Catalog currentCatalog = new Catalog ();
/// // ... currentCatalog population ...
/// // Save to disk
/// Kelvin<Catalog>.FreezeToBinaryFile(currentCatalog, @"C:\Temp\Catalog.dat");
/// // Load from disk
/// Catalog loadedCatalog = Kelvin<Catalog>.ThawFromBinaryFile(@"C:\Temp\Catalog.dat");
/// 
/// string[] words = new string[] {"Sample", "Array", "of", "Words"};
/// Kelvin<string[]>.FreezeToXmlFile(words, @"C:\Temp\Words.xml.");
/// string[] loadedWords = Kelvin<string[]>.ThawFromXmlFile(@"C:\Temp\Words.xml");
/// ]]></code>
/// </example>
namespace ConceptDevelopment
{
   public static class Kelvin<T>
   {
      #region Static Constructor (empty)
      /// <summary>
      /// Kelvin cannot be instantiated. All methods are static. 
      /// Nested classes are not static, but are only used internally.
      /// </summary>
      static Kelvin() { }
      #endregion

      #region To/From a File
      /// <summary>
      /// Serialize object to an XML file on disk
      /// </summary>
      /// <param name="cryo">T instance to serialize</param>
      /// <param name="fileName">Full file path, including name and extension, eg @"C:\Temp\NewFile.xml"</param>
      /// <returns>true if save was successful, false if an error occured</returns>
      public static bool ToXmlFile(T cryo, string fileName)
      {
         try
         {
            XmlSerializer serializerXml = new XmlSerializer(typeof(T));
            System.IO.TextWriter writer = new System.IO.StreamWriter(fileName);
            serializerXml.Serialize(writer, cryo);
            writer.Close();
            return true;
         }
         catch (System.IO.DirectoryNotFoundException)
         {
            return false;
         }
      }
      /// <summary>
      /// Deserialize an Xml File to T object
      /// </summary>
      /// <param name="frozenObjectFileName">Full file path, including name and extension, eg @"C:\Temp\NewFile.xml"</param>
      /// <returns>T instance or default(T)</returns>
      public static T FromXmlFile(string frozenObjectFileName)
      {
         XmlSerializer serializerXml = new XmlSerializer(typeof(T));
         if (System.IO.File.Exists(frozenObjectFileName))
         {
            System.IO.Stream stream = new System.IO.FileStream(frozenObjectFileName, System.IO.FileMode.Open);
            object o = serializerXml.Deserialize(stream);
            stream.Close();
            return (T)o;
         }
         else
         {
            throw new System.IO.FileNotFoundException(frozenObjectFileName + " was not found.");
         }
      }
      #endregion

      #region To/From a String
      /// <summary>
      /// Serialize object to an Xml String for use in your code
      /// </summary>
      /// <param name="cryo">T instance to serialize</param>
      /// <returns><see cref="System.String"/> representation of T object</returns>
      public static string ToXmlString(T cryo)
      {
         XmlSerializer serializer = new XmlSerializer(typeof(T));
         System.IO.TextWriter writer = new System.IO.StringWriter();
         try
         {
            serializer.Serialize(writer, cryo);
         }
         finally
         {
            writer.Flush();
            writer.Close();
         }

         return writer.ToString();
      }
      /// <summary>
      /// Deserialize a String containing Xml to T object
      /// </summary>
      /// <param name="frozen"></param>
      /// <returns>T instance or default(T)</returns>
      public static T FromXml(string frozen)
      {
         if (frozen.Length <= 0) throw new ArgumentOutOfRangeException("frozenObject", "Cannot thaw a zero-length string");

         XmlSerializer serializer = new XmlSerializer(typeof(T));
         System.IO.TextReader reader = new System.IO.StringReader(frozen);
         object @object = default(T);    // default return value
         try
         {
            @object = serializer.Deserialize(reader);
         }
         finally
         {
            reader.Close();
         }
         return (T)@object;
      }
      #endregion

      #region To/From Xml
      /// <summary>
      /// Serialize object to an XmlDocument for use in your code
      /// </summary>
      /// <param name="cryo">T instance to serialize</param>
      /// <returns><see cref="System.Xml.XmlDocument"/> representation of T object</returns>
      public static System.Xml.XmlDocument ToXmlDocument(T cryo)
      {
         XmlSerializer serializer = new XmlSerializer(typeof(T));
         System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
         using (System.IO.Stream stream = new System.IO.MemoryStream())
         {
            serializer.Serialize(stream, cryo);
            stream.Position = 0;
            xml.Load(stream);
         }
         return xml;
      }
      /// <summary>
      /// Deserialize an XmlDocument to T object
      /// </summary>
      /// <param name="frozen">XmlDocument to deserialize</param>
      /// <returns>T instance</returns>
      public static T FromXml(System.Xml.XmlDocument frozen)
      {
         XmlSerializer serializer = new XmlSerializer(typeof(T));
         System.IO.Stream stream = new System.IO.MemoryStream();
         frozen.Save(stream);
         try
         {
            stream.Position = 0;
            return (T)serializer.Deserialize(stream);
         }
         finally
         {
            stream.Close();
         }
      }
      #endregion

      #region To/From a binary File
      /// <summary>
      /// Serialize object to a Binary file on disk
      /// </summary>
      /// <param name="cryo">T instance to serialize</param>
      /// <param name="fileName">Full file path, including name and extension, eg @"C:\Temp\NewFile.dat"</param>
      /// <returns>true if save was successful, false if an error occured</returns>
      public static bool ToBinaryFile(T cryo, string fileName)
      {
         try
         {
            System.IO.Stream stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
            System.Runtime.Serialization.IFormatter formatter
                = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(stream, cryo);
            stream.Close();
            return true;
         }
         catch (System.IO.DirectoryNotFoundException)
         {
            return false;
         }
      }
      /// <summary>
      /// Deserialize a Binary File to T object
      /// </summary>
      /// <param name="frozenObjectFileName">Full file path, including name and extension, eg @"C:\Temp\NewFile.xml"</param>
      /// <returns>T instance or default(T)</returns>
      public static T FromBinaryFile(string frozenObjectFileName)
      {
         if (System.IO.File.Exists(frozenObjectFileName))
         {
            System.IO.Stream stream = new System.IO.FileStream(frozenObjectFileName, System.IO.FileMode.Open);
            System.Runtime.Serialization.IFormatter formatter
                = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            try
            {
               return (T)formatter.Deserialize(stream); // This doesn't work, SerializationException "Cannot find the assembly <random name>"
            }
            catch (System.Runtime.Serialization.SerializationException)
            {   // "Unable to find assembly 'App_Code.vj-e_8q4, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'."
               // or                       "App_Code.y4xkvcpq, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null."
               // and try the same operation with a custom Binder class that tries to resolve types locally.
               stream.Position = 0;
               formatter.Binder = new GenericBinder();
               return (T)formatter.Deserialize(stream); // // Try the custom GenericBinder to see if that helps us 'find' the assembly
            }
            finally
            {
               stream.Close();
            }
         }
         else
         {
            throw new System.IO.FileNotFoundException(frozenObjectFileName + " was not found.");
         }
      }
      #endregion

      #region To/From a Byte[]
      /// <summary>
      /// Serialize object to a Byte[] array for use in your code
      /// </summary>
      /// <param name="cryo">T instance to serialize</param>
      /// <returns>Array of <see cref="System.Byte"/> representing T object</returns>
      public static Byte[] ToBinary(T cryo)
      {
         System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter
             = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
         System.IO.Stream stream = new System.IO.MemoryStream();
         try
         {
            formatter.Serialize(stream, cryo);
            byte[] bytes = new byte[stream.Length];
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
         }
         finally
         {
            stream.Close();
         }
      }
      /// <summary>
      /// Deserialize a Byte Array to T object
      /// </summary>
      /// <param name="frozen">Array of <see cref="System.Byte"/> containing a previously serialized object</param>
      /// <returns>T instance or null</returns>
      public static T FromBinary(Byte[] frozen)
      {
         if (frozen.Length <= 0) throw new ArgumentOutOfRangeException("frozenObject", "Cannot thaw a zero-length Byte[] array");

         System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter
             = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
         System.IO.Stream stream = new System.IO.MemoryStream(frozen);
         try
         {
            return (T)formatter.Deserialize(stream);
         }
         finally
         {
            stream.Close();
         }
      }
      /// <summary>
      /// Deserialize a Byte Array to T object, using a custom Binder 
      /// to help resolve the types referred to in the serialized stream
      /// </summary>
      /// <param name="frozen">Array of <see cref="System.Byte"/> containing a previously serialized object</param>
      /// <param name="customBinder">Subclass of <see cref="System.Runtime.Serialization.SerializationBinder"/></param>
      /// <returns>T instance or null</returns>
      public static T FromBinary(Byte[] frozen, System.Runtime.Serialization.SerializationBinder customBinder)
      {
         if (frozen.Length <= 0) throw new ArgumentOutOfRangeException("frozenObject", "Cannot thaw a zero-length Byte[] array");
         if (customBinder == null) throw new ArgumentNullException("customBinder", "SerializationBinder implementation cannot be null");

         System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter
             = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
         formatter.Binder = customBinder;
         System.IO.Stream stream = new System.IO.MemoryStream(frozen);
         try
         {
            return (T)formatter.Deserialize(stream);
         }
         finally
         {
            stream.Close();
         }
      }
      #endregion



      #region Private SerializationBinder helper classes
      /// <summary>
      /// Attempt to resolve type names by looking in the assembly where T is declared
      /// </summary>
      /// <remarks>
      /// http://msdn2.microsoft.com/en-us/library/system.runtime.serialization.formatters.binary.binaryformatter.binder.aspx
      /// http://msdn2.microsoft.com/en-us/library/system.runtime.serialization.serializationbinder.aspx
      /// </remarks>
      public class GenericBinder : System.Runtime.Serialization.SerializationBinder
      {
         /// <summary>
         /// Resolve type
         /// </summary>
         /// <param name="assemblyName">eg. App_Code.y4xkvcpq, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null</param>
         /// <param name="typeName">eg. String</param>
         /// <returns>Type for the deserializer to use</returns>
         public override Type BindToType(string assemblyName, string typeName)
         {
            // We're going to ignore the assembly name, and assume it's in the same assembly 
            // that <T> is defined (it's either T or a field/return type within T anyway)
            string[] typeInfo = typeName.Split('.');
            bool isSystem = (typeInfo[0].ToString() == "System");
            string className = typeInfo[typeInfo.Length - 1];

            // noop is the default, returns what was passed in
            Type toReturn = Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));

            if (!isSystem && (toReturn == null))
            {   // don't bother if system, or if the GetType worked already (must be OK, surely?)
               System.Reflection.Assembly a = System.Reflection.Assembly.GetAssembly(typeof(T));
               if (a == null)
               {
                  throw new ArgumentException("Assembly for type '" + typeof(T).Name.ToString() + "' could not be loaded.");
               }
               else
               {
                  string assembly = a.FullName.Split(',')[0];   //FullName example: "App_Code.y4xkvcpq, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"
                  Type newtype = a.GetType(assembly + "." + className);
                  if (newtype == null)
                  {
                     throw new ArgumentException("Type '" + typeName + "' could not be loaded from assembly '" + assembly + "'.");
                  }
                  else
                  {
                     toReturn = newtype;
                  }
               }
            }
            return toReturn;
         }
      }
      #endregion
   }
}