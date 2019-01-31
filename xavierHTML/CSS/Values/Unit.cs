using System;
using System.ComponentModel;
using System.Linq;
using Sprache;
using xavierHTML.Parsers.CSS;

namespace xavierHTML.CSS.Values
{
    public abstract class Unit
    {
        public string Name { get; protected set; }
        public string Notation { get; protected set; }
        
        public static readonly Percentage Percentage = new Percentage();
        public static readonly Ems Ems = new Ems();
        public static readonly Rems Rems = new Rems();
        public static readonly Points Points = new Points();
        public static readonly Pixels Pixels = new Pixels();
        public static readonly Degrees Degrees = new Degrees();
        public static readonly Radians Radians = new Radians();

        private static readonly Unit[] Units =
        {
            Percentage, Ems, Rems, Points, Pixels, Degrees, Radians
        };

        public static readonly Parser<Unit> Parse =
            Units.Select(u => Sprache.Parse.String(u.Notation).Text().Optional())
                .Aggregate((unit1, unit2) => unit1.Or(unit2)) // OR together all the Unit subclasses
                .Or(Tokens.Identifier.Optional()) // If none of those matched, eat the bad unit in the code
                .Select(option =>
                {
                    if (!option.IsDefined) return new Unitless();
                    var matchedUnit = Units.FirstOrDefault(unit => unit.Notation == option.Get());
                    // TODO: Handle this case with better introspection?
                    return matchedUnit ?? new Unitless();
                });

        public override string ToString()
        {
            return $"{Name} ({Notation})";
        }
    }

    public class Unitless : Unit
    {
        public Unitless()
        {
            Notation = "";
            Name = "Unitless";
        }
    }

    public class Percentage : Unit
    {
        public Percentage()
        {
            Notation = "%";
            Name = "Percentage";
        }
    }

    public class Ems : Unit
    {
        public Ems()
        {
            Notation = "em";
            Name = "Ems";
        }
    }
    
    public class Rems : Unit
    {
        public Rems()
        {
            Notation = "rem";
            Name = "Rems";
        }
    }

    public class Points : Unit
    {
        public Points()
        {
            Notation = "pt";
            Name = "Points";
        }
    }

    public class Pixels : Unit
    {
        public Pixels()
        {
            Notation = "px";
            Name = "Pixels";
        }
    }

    public class Degrees : Unit
    {
        public Degrees()
        {
            Notation = "deg";
            Name = "Degrees";
        }
    }

    public class Radians : Unit
    {
        public Radians()
        {
            Notation = "rad";
            Name = "Radians";
        }
    }
}
