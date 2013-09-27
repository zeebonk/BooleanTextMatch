using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooleanTextSearch.Interpreter.Exceptions
{
    public class UnexpectedTokenException<T> : Exception
    {
        public UnexpectedTokenException(T type, int index)
            : base(string.Format("Unexpected token '{0}' at position '{1}'", type, index))
        {
            Type = type;
            Index = index;
        }

        public T Type { get; set; }
        public int Index { get; set; }
    }
}
