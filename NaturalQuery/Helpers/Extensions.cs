using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery
{
    public static class Extensions
    {
        public static string Pluralize(this string text)
        {
            if (Pluralize_EndingZNumbers(ref text))
            {
                return text;
            }

            if (Pluralize_es(ref text))
            {
                return text;
            }

            if (Pluralize_is(ref text))
            {
                return text;
            }

            if (Pluralize_m(ref text))
            {
                return text;
            }

            if (Pluralize_ao(ref text))
            {
                return text;
            }

            return text + "s";
        }

        public static bool IsSimpleType(this Type type)
        {
            return
                type.IsValueType ||
                type.IsPrimitive ||
                new Type[] { 
                    typeof(String),
                    typeof(Decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object;

        }

        #region Internal Methods

        /// <summary>
        /// Verifica os numeros terminados em "z"
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static bool Pluralize_EndingZNumbers(ref string text)
        {
            return (text == "dez");
        }

        /// <summary>
        /// Verifica as palavras:
        /// <list type="bullet">
        ///     <item>
        ///         <description><para>Terminadas em "r" e "z" adicionando o sufixo "es".</para></description>
        ///     </item>
        ///     <item>
        ///         <description><para>Terminadas em "n" adicionando o sufixo "s".</para></description>
        ///     </item>
        ///     <item>
        ///         <description><para>Terminadas em "s" mantendo a forma original.</para></description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static bool Pluralize_es(ref string text)
        {
            bool result = false;

            if (text.EndsWith("r", StringComparison.OrdinalIgnoreCase) || text.EndsWith("z", StringComparison.OrdinalIgnoreCase))
            {
                text = text + "es";
                result = true;
            }

            if (text.EndsWith("n", StringComparison.OrdinalIgnoreCase))
            {
                text = text + "s";
                result = true;
            }

            if (text.EndsWith("s", StringComparison.OrdinalIgnoreCase))
            {
                result = true;
            }

            return result;
        }

        private static bool Pluralize_is(ref string text)
        {
            bool result = false;

            if (text == "mal")
            {
                text = "males";
                result = true;
            }

            if (text.EndsWith("al", StringComparison.OrdinalIgnoreCase) ||
                text.EndsWith("el", StringComparison.OrdinalIgnoreCase) ||
                text.EndsWith("ol", StringComparison.OrdinalIgnoreCase) ||
                text.EndsWith("ul", StringComparison.OrdinalIgnoreCase))
            {
                text = text.Substring(0, text.LastIndexOf("l", StringComparison.OrdinalIgnoreCase)) + "is";
                result = true;
            }

            if (text.EndsWith("il", StringComparison.OrdinalIgnoreCase))
            {
                text = text.Substring(0, text.LastIndexOf("il", StringComparison.OrdinalIgnoreCase)) + "eis";
                result = true;
            }

            return result;
        }

        private static bool Pluralize_m(ref string text)
        {
            bool result = false;

            if (text.EndsWith("m", StringComparison.OrdinalIgnoreCase))
            {
                text = text.Substring(0, text.LastIndexOf("m", StringComparison.OrdinalIgnoreCase)) + "ns";
                result = true;
            }

            return result;
        }

        private static bool Pluralize_ao(ref string text)
        {
            bool result = false;

            if (result = Pluralize_ao_e(ref text))
            {
                return result;
            }

            if (result = Pluralize_ao_o(ref text))
            {
                return result;
            }

            if (!result && text.EndsWith("ão", StringComparison.OrdinalIgnoreCase))
            {
                text = text.Substring(0, text.LastIndexOf("ão", StringComparison.OrdinalIgnoreCase)) + "ões";
                return true;
            }

            return result;
        }

        private static bool Pluralize_ao_e(ref string text)
        {
            bool result = true;

            switch (text)
            {
                case "alemão":
                    text = "alemães";
                    break;
                case "cão":
                    text = "cães";
                    break;
                case "capitão":
                    text = "capitães";
                    break;
                case "charlatão":
                    text = "charlatães";
                    break;
                case "guardião":
                    text = "guardiães";
                    break;
                case "sacristão":
                    text = "sacristães";
                    break;
                case "bastião":
                    text = "bastiães";
                    break;
                case "capelão":
                    text = "capelães";
                    break;
                case "catalão":
                    text = "catalães";
                    break;
                case "escrivão":
                    text = "escrivães";
                    break;
                case "pão":
                    text = "pães";
                    break;
                case "tabelião":
                    text = "tabeliães";
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }

        private static bool Pluralize_ao_o(ref string text)
        {
            bool result = true;

            switch (text)
            {
                case "cidadão":
                    text = "cidadãos";
                    break;
                case "cristão":
                    text = "cristãos";
                    break;
                case "irmão":
                    text = "irmãos";
                    break;
                case "acórdão":
                    text = "acórdãos";
                    break;
                case "gólfão":
                    text = "gólfãos";
                    break;
                case "órgão":
                    text = "órgãos";
                    break;
                case "cortesão":
                    text = "cortesãos";
                    break;
                case "desvão":
                    text = "desvãos";
                    break;
                case "pagão":
                    text = "pagãos";
                    break;
                case "bênção":
                    text = "bênçãos";
                    break;
                case "órfão":
                    text = "órfãos";
                    break;
                case "sótão":
                    text = "sótãos";
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }

        #endregion
    }
}
