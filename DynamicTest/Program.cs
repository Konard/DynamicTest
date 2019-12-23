using System;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Westwind.Utilities;

namespace DynamicTest
{
    class A : Expando
    {
        public static dynamic Prototype = new A(false);

        public dynamic a = 5;

        public A(dynamic bl)
        {
            if (System.Convert.ToBoolean(bl))
            {
                dynamic self = this;
                self.b = 4;
            }
        }

        public bool HasMember(string name)
        {
            dynamic self = this;
            object result = null;
            try
            {
                var binder = Binder.GetMember(CSharpBinderFlags.None, name, this.GetType(),
                new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });
                var callsite = CallSite<Func<CallSite, object, object>>.Create(binder);
                result = callsite.Target(callsite, this);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!base.TryGetMember(binder, out result))
            {
                if (this == Prototype)
                {
                    return false;
                }
                return Prototype.TryGetMember(binder, out result);
            }
            return true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("enter 0 or 1:");
            dynamic a = int.Parse(Console.ReadLine());
            if (System.Convert.ToBoolean(a))
            {
                a = new A(a);
                a.c = 3;
            }
            else
            {
                A.Prototype.d = 4;
                a = new A(a);
            }
            if (a.HasMember("c"))
            {
                Console.WriteLine(a.c);
                Console.WriteLine(a.b);
            }
            if (a.HasMember("d")) Console.WriteLine(a.d);
        }
    }
}
