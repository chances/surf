using System;
using System.ComponentModel;
using Sprache;

namespace xavierHTML.CSS.Values
{
    public enum Unit
    {
        Unitless,
        [Description("%")]
        Percentage,
        [Description("em")]
        Em,
        [Description("pt")]
        Pt,
        [Description("px")]
        Px,
        [Description("deg")]
        Deg,
        [Description("rad")]
        Rad
    }

    public class Units
    {
        private static readonly Parser<Unit> Percentage =
            from _ in Parse.String(GetDescription(Unit.Percentage))
            select Unit.Percentage;

        private static readonly Parser<Unit> Em =
            from _ in Parse.String(GetDescription(Unit.Em))
            select Unit.Em;

        private static readonly Parser<Unit> Pt =
            from _ in Parse.String(GetDescription(Unit.Pt))
            select Unit.Pt;

        private static readonly Parser<Unit> Px =
            from _ in Parse.String(GetDescription(Unit.Px))
            select Unit.Px;

        private static readonly Parser<Unit> Deg =
            from _ in Parse.String(GetDescription(Unit.Deg))
            select Unit.Deg;

        private static readonly Parser<Unit> Rad =
            from _ in Parse.String(GetDescription(Unit.Rad))
            select Unit.Rad;
        
        public static readonly Parser<Unit> Parser =
            Percentage.Or(Em).Or(Pt).Or(Px).Or(Deg).Or(Rad).Or(
                from _ in Parse.AnyChar.Preview()
                select Unit.Unitless
            );

        private static string GetDescription(Unit obj)
        {
            try
            {
                var fieldInfo = 
                    obj.GetType().GetField( obj.ToString() );

                var attribArray = fieldInfo.GetCustomAttributes( false );

                if (attribArray.Length <= 0) return obj.ToString();
                if( attribArray[0] is DescriptionAttribute attrib  )
                    return attrib.Description;
                return obj.ToString();
            }
            catch(Exception ex)
            {
                return "";
            }
        }
    }
}
