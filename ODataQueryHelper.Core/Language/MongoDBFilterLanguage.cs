using StringToExpression.GrammerDefinitions;
using StringToExpression.LanguageDefinitions;
using StringToExpression.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace ODataQueryHelper.Core.Language
{
    /// <summary>
    /// Provides the base class for parsing OData filter parameters.
    /// </summary>
    public class MongoDBFilterLanguage : ODataFilterLanguage
    {
        public MongoDBFilterLanguage() : base()
        {

        }

        /// <summary>
        /// Returns the definitions for functions used within the language.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<FunctionCallDefinition> FunctionDefinitions()
        {
            return new[]
            {
                 new FunctionCallDefinition(
                    name:"FN_STARTSWITH",
                    regex: @"startswith\(",
                    argumentTypes: new[] {typeof(string), typeof(string) },
                    expressionBuilder: (parameters) => {
                        var param0Exp = Expression.Call(instance:parameters[0], method:Type<string>.Method(x=>x.ToLower()));
                        return Expression.Call(
                            instance:param0Exp,
                            method:StringMembers.StartsWith,
                            arguments: new [] { parameters[1] });
                    }),
                new FunctionCallDefinition(
                    name:"FN_ENDSWITH",
                    regex: @"endswith\(",
                    argumentTypes: new[] {typeof(string), typeof(string) },
                    expressionBuilder: (parameters) => {
                        var param0Exp = Expression.Call(instance:parameters[0], method:Type<string>.Method(x=>x.ToLower()));
                        return Expression.Call(
                            instance:param0Exp,
                            method:StringMembers.EndsWith,
                            arguments: new [] { parameters[1] });
                    }),
                 new FunctionCallDefinition(
                    name:"FN_SUBSTRINGOF",
                    regex: @"substringof\(",
                    argumentTypes: new[] {typeof(string), typeof(string) },
                    expressionBuilder: (parameters) => {
                        return Expression.Call(
                            instance:parameters[1],
                            method:StringMembers.Contains,
                            arguments: new [] { parameters[0] });
                    }),
                new FunctionCallDefinition(
                     name:"FN_CONTAINS",
                    regex: @"contains\(",
                    argumentTypes: new[] {typeof(string), typeof(string) },
                    expressionBuilder: (parameters) => {
                        var param0Exp = Expression.Call(instance:parameters[0], method:Type<string>.Method(x=>x.ToLower()));
                        return Expression.Call(
                            instance: param0Exp,
                                    method:Type<string>.Method(x=>x.Contains(null)),
                            arguments: new [] { parameters[1] });
                    }),
                new FunctionCallDefinition(
                    name:"FN_TOLOWER",
                    regex: @"tolower\(",
                    argumentTypes: new[] {typeof(string) },
                    expressionBuilder: (parameters) => {
                        return Expression.Call(
                            instance:parameters[0],
                            method:StringMembers.ToLower);
                    }),
                new FunctionCallDefinition(
                    name:"FN_TOUPPER",
                    regex: @"toupper\(",
                    argumentTypes: new[] {typeof(string) },
                    expressionBuilder: (parameters) => {
                        return Expression.Call(
                            instance:parameters[0],
                            method:StringMembers.ToUpper);
                    }),

                 new FunctionCallDefinition(
                    name:"FN_DAY",
                    regex: @"day\(",
                    argumentTypes: new[] {typeof(DateTime) },
                    expressionBuilder: (parameters) => {
                        return Expression.MakeMemberAccess(
                            parameters[0],
                            DateTimeMembers.Day);
                    }),
                 new FunctionCallDefinition(
                    name:"FN_HOUR",
                    regex: @"hour\(",
                    argumentTypes: new[] {typeof(DateTime) },
                    expressionBuilder: (parameters) => {
                        return Expression.MakeMemberAccess(
                            parameters[0],
                            DateTimeMembers.Hour);
                    }),
                  new FunctionCallDefinition(
                    name:"FN_MINUTE",
                    regex: @"minute\(",
                    argumentTypes: new[] {typeof(DateTime) },
                    expressionBuilder: (parameters) => {
                        return Expression.MakeMemberAccess(
                            parameters[0],
                            DateTimeMembers.Minute);
                    }),
                  new FunctionCallDefinition(
                    name:"FN_MONTH",
                    regex: @"month\(",
                    argumentTypes: new[] {typeof(DateTime) },
                    expressionBuilder: (parameters) => {
                        return Expression.MakeMemberAccess(
                            parameters[0],
                            DateTimeMembers.Month);
                    }),
                new FunctionCallDefinition(
                    name:"FN_YEAR",
                    regex: @"year\(",
                    argumentTypes: new[] {typeof(DateTime) },
                    expressionBuilder: (parameters) => {
                        return Expression.MakeMemberAccess(
                            parameters[0],
                            DateTimeMembers.Year);
                    }),
                 new FunctionCallDefinition(
                    name:"FN_SECOND",
                    regex: @"second\(",
                    argumentTypes: new[] {typeof(DateTime) },
                    expressionBuilder: (parameters) => {
                        return Expression.MakeMemberAccess(
                            parameters[0],
                            DateTimeMembers.Second);
                    }),
            };
        }
    }
}
