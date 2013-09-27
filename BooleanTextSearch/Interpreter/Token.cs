using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace BooleanTextSearch.Interpreter
{
    public class Token<T>
    {
        public T Type { get; set; }
        public string Value { get; set; }
        public Expression Expression { get; set; }
        public int Index { get; set; }
    }
}
