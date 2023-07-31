using OfficeOpenXml;
using System.Reflection;

namespace IdentityApi.Extensions
{
    public static class ExcelExtensions
    {
        public class EpplusIgnore : Attribute { }

        public static bool IsNullOrEmpty(this string[] strarray)
        {
            return strarray == null || strarray.Length == 0;
        }

        public static ExcelRangeBase LoadFromCollectionFiltered<T>(this ExcelRangeBase @this, List<T> collection) where T : class
        {
            MemberInfo[] membersToInclude = typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !Attribute.IsDefined(p, typeof(EpplusIgnore)))
                .ToArray();


                return @this.LoadFromCollection<T>(collection, true,
                OfficeOpenXml.Table.TableStyles.Medium9,
                BindingFlags.Instance | BindingFlags.Public,
                membersToInclude);
        }
    }
}
