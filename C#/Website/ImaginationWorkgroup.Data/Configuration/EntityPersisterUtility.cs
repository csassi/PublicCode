using ImaginationWorkgroup.Data.Entities;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.Configuration
{
    public class EntityPersisterUtility
    {
        public static void SetState(IEntityPersister persister, IList<object> state, string propertyName, object value)
        {
            var index = Array.IndexOf(persister.PropertyNames, propertyName);
            if (index == -1)
                return;
            state[index] = value;
        }

        public static string GetPropertyValue(IList<object> state, int index)
        {
            var propertyValue = state[index];
            if(propertyValue is ImaginationEntityBase entityBase)
            {
                return entityBase.Id.ToString();
            }
            return propertyValue == null ? string.Empty : propertyValue.ToString();
        }

        public static string GetPropertyName<TType>(Expression<Func<TType, object>> expression)
        {
            return ExpressionProcessor.FindPropertyExpression(expression.Body);
        }
    }
}
