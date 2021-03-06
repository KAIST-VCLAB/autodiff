﻿using System;

namespace AutoDiff.Compiled
{
    internal sealed class NaryFunc : TapeElement
    {
        private readonly Func<double[], double> eval;
        private readonly Func<double[], double[]> diff;

        public NaryFunc(Func<double[], double> eval, Func<double[], double[]> diff)
        {
            this.eval = eval;
            this.diff = diff;
        }

        public override void Eval()
        {
            Value = eval(GetArg());
        }

        public override void Diff()
        {
            var arg = GetArg();
            Value = eval(arg);
            var grad = diff(arg);
            for (var i = 0; i < grad.Length; ++i)
                Inputs.SetWeight(i, grad[i]);
        }

        private double[] GetArg()
        {
            var arg = new double[Inputs.Length];
            for (var i = 0; i < arg.Length; ++i)
                arg[i] = Inputs.Element(i).Value;
            return arg;
        }
    }
}
