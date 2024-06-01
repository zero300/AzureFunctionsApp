using System.Linq.Expressions;
using System.Net;
using System.Text.Json;
using System.Threading;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Prodcuct.Entities;

namespace API.Utils;

public static class SQLUtils{
    public static IQueryable<T> ApplyLimit<T>(IQueryable<T> query, int limit) => limit == 0 ? query : query.Take(limit) ; 
    public static IQueryable<TEntity> OrderBySingle<TEntity>(this IQueryable<TEntity> query,string? orderByProperty, bool desc){
        if(orderByProperty is null) return query;
        
        var command = desc ? "OrderByDescending" : "OrderBy";
        var type = typeof(TEntity); // Type
        var property = type.GetProperty(orderByProperty) ?? 
            throw new ArgumentNullException($"This Class Doesn't Have this '{orderByProperty}' Property"); // Property
        
        var parameter = Expression.Parameter(type, "p");  // Parameter Expression
        var propertyAccess = Expression.MakeMemberAccess(parameter, property); // property.parameter
        var orderByExpression = Expression.Lambda(propertyAccess, parameter); // parameter => propertyAccess
        var resultExpression = Expression.Call(
            typeof(Queryable), 
            command, 
            new Type[] { type, property.PropertyType },
            query.Expression, 
            Expression.Quote(orderByExpression)
            );
                                        
        return query.Provider.CreateQuery<TEntity>(resultExpression);
    }
    public static IQueryable<TEntity> OrderByMultiple<TEntity>(this IQueryable<TEntity> query,string[] orderByProperties, bool[] descs){
        if (orderByProperties.Length != descs.Length) throw new Exception("排序數量需相等");
        
        var type = typeof(TEntity); // Type
        var parameter = Expression.Parameter(type, "p");  // Parameter Expression

        for (int i = 0 ; i < descs.Length ; i++){
            bool desc = descs[i];
            string orderByProperty = orderByProperties[i];

            var command = desc ? "OrderByDescending" : "OrderBy";
            var property = type.GetProperty(orderByProperty) ?? 
                throw new ArgumentNullException($"This Class Doesn't Have this '{orderByProperty}' Property"); // Property
            // Parameter Expression
            var propertyAccess = Expression.MakeMemberAccess(parameter, property); // property.parameter
            var orderByExpression = Expression.Lambda(propertyAccess, parameter); // parameter => propertyAccess
            var resultExpression = Expression.Call(
                    typeof(Queryable), 
                    command, 
                    new Type[] { type, property.PropertyType },
                    query.Expression, 
                    Expression.Quote(orderByExpression)
                );
            query.Provider.CreateQuery<TEntity>(resultExpression);
        }                               
        return query;
    }
}
