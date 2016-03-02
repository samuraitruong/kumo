using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace System
{
    /// <summary>
    ///   Extensions for supporting xml serialization by <see cref = "XmlSerializer" />
    /// </summary>
    public static class XmlSerializerExtensions
    {
        #region Private fields
        private static readonly Dictionary<RuntimeTypeHandle, XmlSerializer> ms_serializers = new Dictionary<RuntimeTypeHandle, XmlSerializer>();
        #endregion
        #region Public methods
        /// <summary>
        ///   Serialize object to xml string by <see cref = "XmlSerializer" />
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "value"></param>
        /// <returns></returns>
        public static string ToXml<T>(this T value)
            where T : new()
        {
            var _serializer = GetValue(typeof(T));
            using (var _stream = new MemoryStream())
            {
                using (var _writer = new XmlTextWriter(_stream, new UTF8Encoding()))
                {
                    _serializer.Serialize(_writer, value);
                    return Encoding.UTF8.GetString(_stream.ToArray());
                }
            }
        }
        /// <summary>
        ///   Serialize object to stream by <see cref = "XmlSerializer" />
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "value"></param>
        /// <param name = "stream"></param>
        public static void ToXml<T>(this T value, Stream stream)
            where T : new()
        {
            var _serializer = GetValue(typeof(T));
            _serializer.Serialize(stream, value);
        }

        /// <summary>
        ///   Deserialize object from string
        /// </summary>
        /// <typeparam name = "T">Type of deserialized object</typeparam>
        /// <param name = "srcString">Xml source</param>
        /// <returns></returns>
        public static T FromXml<T>(this string srcString)
            where T : new()
        {
            var _serializer = GetValue(typeof(T));
            using (var _stringReader = new StringReader(srcString))
            {
                using (XmlReader _reader = new XmlTextReader(_stringReader))
                {
                    return (T)_serializer.Deserialize(_reader);
                }
            }
        }
        /// <summary>
        ///   Deserialize object from stream
        /// </summary>
        /// <typeparam name = "T">Type of deserialized object</typeparam>
        /// <param name = "source">Xml source</param>
        /// <returns></returns>
        public static T FromXml<T>(this Stream source)
            where T : new()
        {
            var _serializer = GetValue(typeof(T));
            return (T)_serializer.Deserialize(source);
        }
        #endregion
        #region Private methods
        private static XmlSerializer GetValue(Type type)
        {
            XmlSerializer _serializer;
            if (!ms_serializers.TryGetValue(type.TypeHandle, out _serializer))
            {
                lock (ms_serializers)
                {
                    if (!ms_serializers.TryGetValue(type.TypeHandle, out _serializer))
                    {
                        _serializer = new XmlSerializer(type);
                        ms_serializers.Add(type.TypeHandle, _serializer);
                    }
                }
            }
            return _serializer;
        }
        #endregion
    }

    public static class ObjectExtensions
    {
        public static Type TypeOfMe<T>(this T obj)
        {
            Type t;
            if (obj == null)
                t = typeof(T);
            else
                t = obj.GetType();
            return t;
        }

        public static T CopyFrom<T>(this T toObject, object fromObject)
        {
            var fromObjectType = fromObject.GetType();

            foreach (PropertyInfo toProperty in toObject.GetType().GetProperties())
            {
                PropertyInfo fromProperty = fromObjectType.GetProperty(toProperty.Name);

                if (fromProperty != null) // match found
                {
                    // check types
                    var fromType = Nullable.GetUnderlyingType(fromProperty.PropertyType) ?? fromProperty.PropertyType;
                    var toType = Nullable.GetUnderlyingType(toProperty.PropertyType) ?? toProperty.PropertyType;

                    if (toType.IsAssignableFrom(fromType))
                    {
                        try
                        {
                            toProperty.SetValue(toObject, fromProperty.GetValue(fromObject, null), null);
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                }
            }

            return toObject;
        }
    }

    public static class Extensions
    {
        public static string IsSelected(this UrlHelper urlHelper, string controller)
        {
            string result = "active";

            string controllerName = urlHelper.RequestContext.RouteData.Values["controller"].ToString();

            if (!controllerName.Equals(controller, StringComparison.OrdinalIgnoreCase))
            {
                result = null;
            }

            return result;
        }
    }
}