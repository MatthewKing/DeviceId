using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if NETSTANDARD
using System.Dynamic;
using Microsoft.Management.Infrastructure;
#endif
#if NETFRAMEWORK
using System.Management;
#endif
namespace DeviceId
{
    class WmiHelper
    {
        /// <summary>
        /// Get an IDictionary representation of WMI objects of a specified class
        /// </summary>
        /// <param name="wmiNamespace">The scope in which to look for objects.</param>
        /// <param name="objectClass">The WMI class name.</param>
        public static IDictionary<string, object>[] GetWMIInstances(string wmiNamespace, string objectClass)
        {
            return GetWMIInstances(wmiNamespace, objectClass, new string[0]);
        }

#if NETSTANDARD
        /// <summary>
        /// Get an IDictionary representation of WMI objects of a specified class
        /// </summary>
        /// <param name="wmiNamespace">The scope in which to look for objects.</param>
        /// <param name="objectClass">The WMI class name.</param>
        /// <param name="attributes">The list of attributes to obtain (or null for all attributes).</param>
        public static IDictionary<string, object>[] GetWMIInstances(string wmiNamespace, string objectClass, string[] attributes)
        {
                try
                {
                    string attributesSelector = (attributes != null && attributes.Length > 0) ? String.Join(",", attributes) : "*";

                    return CimSession.Create(null) // null instead of localhost which would otherwise require certain MMI services running
                            .QueryInstances(wmiNamespace, "WQL", $"SELECT {attributesSelector} FROM {objectClass}").Select((obj) => CimInstanceToExpandoObject(obj)).ToArray();
                }
                catch(CimException e)
                {
                    throw new DeviceIdComponentFailedToObtainValueException(String.Format("Failed in GetWMIInstances({0},{1})", wmiNamespace, objectClass), e);
                }
        }

        /// <summary>
        /// Get an ExpandoObject representation of a CimInstance object
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        private static ExpandoObject CimInstanceToExpandoObject(CimInstance obj)
        {
            try
            {
                ExpandoObject result = new ExpandoObject();
                var resultDictionary = (IDictionary<string, object>)result;

                foreach (var property in obj.CimInstanceProperties.Where(property => property.Value != null))
                    resultDictionary[property.Name] = property.Value is CimInstance ? CimInstanceToExpandoObject(property.Value as CimInstance) : property.Value.ToString();

                return result;
            }
            finally
            {
                obj.Dispose();
            }
        }
#endif
#if NETFRAMEWORK
        /// <summary>
        /// Get an IDictionary representation of WMI objects of a specified class
        /// </summary>
        /// <param name="wmiNamespace">The scope in which to look for objects.</param>
        /// <param name="objectClass">The WMI class name.</param>
        /// <param name="attributes">The list of attributes to obtain (or null for all attributes).</param>
        public static IDictionary<string, object>[] GetWMIInstances(string wmiNamespace, string objectClass, string[] attributes)
        {
            try
            {
                string attributesSelector = attributes.Length > 0 ? String.Join(",", attributes) : "*";
                using var managementObjectSearcher = new ManagementObjectSearcher(wmiNamespace, $"select {attributesSelector} from {objectClass}");
                using var managementObjectCollection = managementObjectSearcher.Get();

                return managementObjectCollection.Cast<ManagementBaseObject>().Select(obj => ManagementObjectToDictionary(obj)).ToArray();
            }
            catch(ManagementException e)
            {
                throw new DeviceIdComponentFailedToObtainValueException(String.Format("Failed in GetWMIInstances({0},{1})", wmiNamespace, objectClass), e);
            }
        }

        /// <summary>
        /// Get a Dictionary representation of a ManagementBaseObject object
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        private static Dictionary<string, object> ManagementObjectToDictionary(ManagementBaseObject obj)
        {
            try
            {
                return obj.Properties.Cast<PropertyData>().ToDictionary(property => property.Name, property =>
                                   property.Value is ManagementBaseObject ? ManagementObjectToDictionary(property.Value as ManagementBaseObject) as object : property.Value.ToString());
            }
            finally
            {
                obj.Dispose();
            }
        }
#endif
    }
}
