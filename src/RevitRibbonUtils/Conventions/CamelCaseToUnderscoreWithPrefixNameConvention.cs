using System;
using System.Globalization;
using System.Linq;

namespace AI.RevitReinforcementDimensioner.RevitRibbonUtils.Conventions
{
    public class CamelCaseToUnderscoreWithPrefixNameConvention : RibbonItemNameConvention
    {
        private readonly string prefix;

        public CamelCaseToUnderscoreWithPrefixNameConvention(string prefix)
        {
            this.prefix = prefix;
        }

        public override string GetRibbonItemName(Type type)
        {
            var name = string.Concat(prefix.Replace(" ", ""), type.Name);

            return string.Concat(name.Select((x, i) => ConvertCharacter(x, i == 0))).ToUpper();
        }

        private static string ConvertCharacter(char x, bool isFirst)
        {
            return !isFirst && char.IsUpper(x)
                ? $"_{x}"
                : x.ToString(CultureInfo.InvariantCulture);
        }
    }
}