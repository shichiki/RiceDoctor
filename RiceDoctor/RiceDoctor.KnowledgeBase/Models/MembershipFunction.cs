using System;

namespace RiceDoctor.KnowledgeBase.Models
{
    public enum MembershipFunctionType
    {
        TriangularShaped,
        TrapezoidalShaped
    }

    public abstract class MembershipFunction
    {
        public abstract MembershipFunctionType Type { get; }

        public abstract double Calculate(double x);
    }

    public class TriangularMembershipFunction : MembershipFunction
    {
        public override MembershipFunctionType Type => MembershipFunctionType.TriangularShaped;

        public double A { get; set; }

        public double B { get; set; }

        public double C { get; set; }

        public override double Calculate(double x)
        {
            if (x <= A) return 0;

            if (A <= x && x <= B) return (x - A)/(B - A);

            if (B <= x && x <= C) return (C - x)/(C - B);

            if (C <= x) return 0;

            throw new ArithmeticException();
        }
    }

    public class TrapezoidalMembershipFunction : MembershipFunction
    {
        public override MembershipFunctionType Type => MembershipFunctionType.TrapezoidalShaped;

        public double A { get; set; }

        public double B { get; set; }

        public double C { get; set; }

        public double D { get; set; }

        public override double Calculate(double x)
        {
            if (x <= A) return 0;

            if (A <= x && x <= B) return (x - A)/(B - A);

            if (B <= x && x <= C) return 1;

            if (C <= x && x <= D) return (D - x)/(D - C);

            if (D <= x) return 0;

            throw new ArithmeticException();
        }
    }
}