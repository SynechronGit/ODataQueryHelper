﻿using StringToExpression.GrammerDefinitions;
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
        /// Returns the definitions for types used within the language.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<GrammerDefinition> TypeDefinitions()
        {
            return new[]
            {
                new OperandDefinition(
                    name:"GUID",
                    regex: @"guid'[0-9A-Fa-f]{8}\-[0-9A-Fa-f]{4}\-[0-9A-Fa-f]{4}\-[0-9A-Fa-f]{4}\-[0-9A-Fa-f]{12}'",
                    expressionBuilder: x => Expression.Constant(Guid.Parse(x.Substring("guid".Length).Trim('\'')))),

                new OperandDefinition(
                    name:"STRING",
                    regex: @"'(?:\\.|[^'])*'",
                    expressionBuilder: x => Expression.Constant(x.Trim('\'')
                        .Replace("\\'", "'")
                        .Replace("\\r", "\r")
                        .Replace("\\f", "\f")
                        .Replace("\\n", "\n")
                        .Replace("\\\\", "\\")
                        .Replace("\\b", "\b")
                        .Replace("\\t", "\t"))),
                new OperandDefinition(
                    name:"BYTE",
                    regex: @"0x[0-9A-Fa-f]{1,2}",
                    expressionBuilder: x => Expression.Constant(byte.Parse(x.Substring("0x".Length), NumberStyles.HexNumber | NumberStyles.AllowHexSpecifier))),
                new OperandDefinition(
                    name:"NULL",
                    regex: @"null",
                    expressionBuilder: x => Expression.Constant(null)),
                new OperandDefinition(
                    name:"BOOL",
                    regex: @"true|false",
                    expressionBuilder: x => Expression.Constant(bool.Parse(x))),
                 new OperandDefinition(
                    name:"DATETIME",
                    regex: @"[Dd][Aa][Tt][Ee][Tt][Ii][Mm][Ee]'[^']+'",
                    expressionBuilder: x => Expression.Constant(DateTime.Parse(x.Substring("datetime".Length).Trim('\'')))),
                new OperandDefinition(
                    name:"DATETIMEOFFSET",
                    regex: @"datetimeoffset'[^']+'",
                    expressionBuilder: x => Expression.Constant(DateTimeOffset.Parse(x.Substring("datetimeoffset".Length).Trim('\'')))),

                new OperandDefinition(
                    name:"FLOAT",
                    regex: @"\-?\d+?\.\d*f",
                    expressionBuilder: x => Expression.Constant(float.Parse(x.TrimEnd('f')))),
                new OperandDefinition(
                    name:"DOUBLE",
                    regex: @"\-?\d+\.?\d*d",
                    expressionBuilder: x => Expression.Constant(double.Parse(x.TrimEnd('d')))),
                new OperandDefinition(
                    name:"DECIMAL_EXPLICIT",
                    regex: @"\-?\d+\.?\d*[m|M]",
                    expressionBuilder: x => Expression.Constant(decimal.Parse(x.TrimEnd('m','M')))),
                new OperandDefinition(
                    name:"DECIMAL",
                    regex: @"\-?\d+\.\d+",
                    expressionBuilder: x => Expression.Constant(decimal.Parse(x))),

                new OperandDefinition(
                    name:"LONG",
                    regex: @"\-?\d+L",
                    expressionBuilder: x => Expression.Constant(long.Parse(x.TrimEnd('L')))),
                new OperandDefinition(
                    name:"INTEGER",
                    regex: @"\-?\d+",
                    expressionBuilder: x => Expression.Constant(int.Parse(x))),
            };
        }

        /// <summary>
        /// Returns the definitions for logic operators used within the language.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<GrammerDefinition> LogicalOperatorDefinitions()
        {
            return new GrammerDefinition[]
            {
                new BinaryOperatorDefinition(
                    name:"EQ",
                    regex: @"eq",
                    orderOfPrecedence:11,
                    expressionBuilder: ConvertEnumsIfRequired((left,right) => Expression.Equal(left, right))),
                new BinaryOperatorDefinition(
                    name:"NE",
                    regex: @"ne",
                    orderOfPrecedence:12,
                    expressionBuilder: ConvertEnumsIfRequired((left,right) => Expression.NotEqual(left, right))),

                new BinaryOperatorDefinition(
                    name:"GT",
                    regex: @"gt",
                    orderOfPrecedence:13,
                    expressionBuilder: (left,right) => Expression.GreaterThan(left, right)),
                new BinaryOperatorDefinition(
                    name:"GE",
                    regex: @"ge",
                    orderOfPrecedence:14,
                    expressionBuilder: (left,right) => Expression.GreaterThanOrEqual(left, right)),

                new BinaryOperatorDefinition(
                    name:"LT",
                    regex: @"lt",
                    orderOfPrecedence:15,
                    expressionBuilder: (left,right) => Expression.LessThan(left, right)),
                new BinaryOperatorDefinition(
                    name:"LE",
                    regex: @"le",
                    orderOfPrecedence:16,
                    expressionBuilder: (left,right) => Expression.LessThanOrEqual(left, right)),

                new BinaryOperatorDefinition(
                    name:"AND",
                    regex: @"and",
                    orderOfPrecedence:17,
                    expressionBuilder: (left,right) => Expression.AndAlso(left, right)),
                new BinaryOperatorDefinition(
                    name:"OR",
                    regex: @"or",
                    orderOfPrecedence:18,
                    expressionBuilder: (left,right) => Expression.OrElse(left, right)),

                new UnaryOperatorDefinition(
                    name:"NOT",
                    regex: @"not",
                    orderOfPrecedence:19,
                    operandPosition: RelativePosition.Right,
                    expressionBuilder: (arg) => {
                        ExpressionConversions.TryBoolean(ref arg);
                        return Expression.Not(arg);
                    })
            };
        }

        /// <summary>
        /// Returns the definitions for arithmetic operators used within the language.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<GrammerDefinition> ArithmeticOperatorDefinitions()
        {
            return new[]
            {
                 new BinaryOperatorDefinition(
                    name:"ADD",
                    regex: @"add",
                    orderOfPrecedence: 2,
                    expressionBuilder: (left,right) => Expression.Add(left, right)),
                new BinaryOperatorDefinition(
                    name:"SUB",
                    regex: @"sub",
                    orderOfPrecedence: 2,
                    expressionBuilder: (left,right) => Expression.Subtract(left, right)),
                new BinaryOperatorDefinition(
                    name:"MUL",
                    regex: @"mul",
                    orderOfPrecedence: 1,
                    expressionBuilder: (left,right) => Expression.Multiply(left, right)),
                new BinaryOperatorDefinition(
                    name:"DIV",
                    regex: @"div",
                    orderOfPrecedence: 1,
                    expressionBuilder: (left,right) => Expression.Divide(left, right)),
                new BinaryOperatorDefinition(
                    name:"MOD",
                    regex: @"mod",
                    orderOfPrecedence: 1,
                    expressionBuilder: (left,right) => Expression.Modulo(left, right)),
            };
        }

        /// <summary>
        /// Returns the definitions for brackets used within the language.
        /// </summary>
        /// <param name="functionCalls">The function calls in the language. (used as opening brackets)</param>
        /// <returns></returns>
        protected override IEnumerable<GrammerDefinition> BracketDefinitions(IEnumerable<FunctionCallDefinition> functionCalls)
        {
            BracketOpenDefinition openBracket;
            ListDelimiterDefinition delimeter;
            return new GrammerDefinition[] {
                openBracket = new BracketOpenDefinition(
                    name: "OPEN_BRACKET",
                    regex: @"\("),
                delimeter = new ListDelimiterDefinition(
                    name: "COMMA",
                    regex: ","),
                new BracketCloseDefinition(
                    name: "CLOSE_BRACKET",
                    regex: @"\)",
                    bracketOpenDefinitions: new[] { openBracket }.Concat(functionCalls),
                    listDelimeterDefinition: delimeter)
            };
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
                            instance: param0Exp,
                                    method:Type<string>.Method(x=>x.StartsWith(null)),
                            arguments: new [] { parameters[1] });
                    }),
                new FunctionCallDefinition(
                    name:"FN_ENDSWITH",
                    regex: @"endswith\(",
                    argumentTypes: new[] {typeof(string), typeof(string) },
                    expressionBuilder: (parameters) => {
                         var param0Exp = Expression.Call(instance:parameters[0], method:Type<string>.Method(x=>x.ToLower()));
                        return Expression.Call(
                            instance: param0Exp,
                                    method:Type<string>.Method(x=>x.EndsWith(null)),
                            arguments: new [] { parameters[1] });
                    }),
                 new FunctionCallDefinition(
                    name:"FN_SUBSTRINGOF",
                    regex: @"substringof\(",
                    argumentTypes: new[] {typeof(string), typeof(string) },
                    expressionBuilder: (parameters) => {
                        return Expression.Call(
                            instance:parameters[1],
                            method:Type<string>.Method(x=>x.Contains(null)),
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
                            method:Type<string>.Method(x=>x.ToLower()));
                    }),
                new FunctionCallDefinition(
                    name:"FN_TOUPPER",
                    regex: @"toupper\(",
                    argumentTypes: new[] {typeof(string) },
                    expressionBuilder: (parameters) => {
                        return Expression.Call(
                            instance:parameters[0],
                            method:Type<string>.Method(x=>x.ToUpper()));
                    }),

                 new FunctionCallDefinition(
                    name:"FN_DAY",
                    regex: @"day\(",
                    argumentTypes: new[] {typeof(DateTime) },
                    expressionBuilder: (parameters) => {
                        return Expression.MakeMemberAccess(
                            parameters[0],
                            Type<DateTime>.Member(x=>x.Day));
                    }),
                 new FunctionCallDefinition(
                    name:"FN_HOUR",
                    regex: @"hour\(",
                    argumentTypes: new[] {typeof(DateTime) },
                    expressionBuilder: (parameters) => {
                        return Expression.MakeMemberAccess(
                            parameters[0],
                            Type<DateTime>.Member(x=>x.Hour));
                    }),
                  new FunctionCallDefinition(
                    name:"FN_MINUTE",
                    regex: @"minute\(",
                    argumentTypes: new[] {typeof(DateTime) },
                    expressionBuilder: (parameters) => {
                        return Expression.MakeMemberAccess(
                            parameters[0],
                            Type<DateTime>.Member(x=>x.Minute));
                    }),
                  new FunctionCallDefinition(
                    name:"FN_MONTH",
                    regex: @"month\(",
                    argumentTypes: new[] {typeof(DateTime) },
                    expressionBuilder: (parameters) => {
                        return Expression.MakeMemberAccess(
                            parameters[0],
                            Type<DateTime>.Member(x=>x.Month));
                    }),
                new FunctionCallDefinition(
                    name:"FN_YEAR",
                    regex: @"year\(",
                    argumentTypes: new[] {typeof(DateTime) },
                    expressionBuilder: (parameters) => {
                        return Expression.MakeMemberAccess(
                            parameters[0],
                            Type<DateTime>.Member(x=>x.Year));
                    }),
                 new FunctionCallDefinition(
                    name:"FN_SECOND",
                    regex: @"second\(",
                    argumentTypes: new[] {typeof(DateTime) },
                    expressionBuilder: (parameters) => {
                        return Expression.MakeMemberAccess(
                            parameters[0],
                            Type<DateTime>.Member(x=>x.Second));
                    })

            };
        }
    }
}
