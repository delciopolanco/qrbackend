using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace qr_backend.Util
{
    public class Utilitary
    {
        public const string postGenericBroker = "PostGeneric";
        public const string getGenericBroker = "GetGeneric";

        public static Func<TSource, TTarget> CreateMapper<TSource, TTarget>()
            where TTarget : new()
            {
                var sourceProperties = typeof(TSource)
                    .GetProperties()
                    .Where(x => x.CanRead);
                var targetProperties = typeof(TTarget)
                    .GetProperties()
                    .Where(x => x.CanWrite)
                    .ToDictionary(x => x.Name, x => x, StringComparer.OrdinalIgnoreCase);

                var source = Expression.Parameter(typeof(TSource), "source");
                var target = Expression.Variable(typeof(TTarget));
                var allocate = Expression.New(typeof(TTarget));
                var assignTarget = Expression.Assign(target, allocate);

                var statements = new List<Expression>();
                statements.Add(assignTarget);

                foreach (var sourceProperty in sourceProperties)
                {
                    PropertyInfo targetProperty;
                    if (targetProperties.TryGetValue(sourceProperty.Name, out targetProperty))
                    {
                        var assignProperty = Expression.Assign(
                            Expression.Property(target, targetProperty),
                            Expression.Property(source, sourceProperty));
                        statements.Add(assignProperty);
                    }
                }

                statements.Add(target);

                var body = Expression.Block(new[] { target }, statements);

                return Expression.Lambda<Func<TSource, TTarget>>(body, source).Compile();
        }

        public static int GetProductTypeIdByDescription(string description)
        {
            if (description == "PaymeId")
                return 1;
            if (description == "Cuentas de Ahorro")
                return 2;
            if (description == "Cuentas Corrientes")
                return 3;
            if (description == "Tarjetas de Credito")
                return 4;
            if (description == "GC")
                return 5;
            return 0;
        }

        public static string GetCurrencySymbol(string currency)
        {
            if (currency == "EU")
                return "€";
            
            return "$";
        }
    }
}
