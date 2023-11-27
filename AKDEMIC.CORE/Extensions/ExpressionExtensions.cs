using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace AKDEMIC.CORE.Extensions
{
    public static class ExpressionExtensions
    {
        public static List<MemberInfo> GetExpressionMembers(this Expression expression, int depth = 0)
        {
            var expressionMembers = new List<MemberInfo>();
            var memberInitExpression = (MemberInitExpression)expression;
            var bodyBindings = memberInitExpression.Bindings;
            var processedDepth = depth;

            if (depth > 0)
            {
                processedDepth--;
            }

            for (var i = 0; i < bodyBindings.Count; i++)
            {
                var bodyBinding = (MemberAssignment)bodyBindings[i];
                var bindingExpressionNodeType = bodyBinding.Expression.NodeType;
                var expressionMember = bodyBinding.Member;

                if (bindingExpressionNodeType == ExpressionType.MemberAccess)
                {
                    expressionMembers.Add(expressionMember);
                }
                else if (depth != 0 && bindingExpressionNodeType == ExpressionType.MemberInit)
                {
                    var extraExpressionMembers = GetExpressionMembers(bodyBinding.Expression, processedDepth);

                    expressionMembers.AddRange(extraExpressionMembers);
                }
            }

            return expressionMembers;
        }

        public static List<string> GetMemberNames(this Expression expression, int depth = 3, string category = null)
        {
            var memberNames = new List<string>();
            var categorySeparator = false;
            var processedDepth = depth;

            if (category != null && category != "")
            {
                categorySeparator = true;
            }

            if (depth > 0)
            {
                processedDepth--;
            }

            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    var memberExpression = (MemberExpression)expression;
                    var expressionMemberName = $"{category}{(categorySeparator ? "." : "")}{memberExpression.Member.Name}";
                    
                    memberNames.Add(expressionMemberName);

                    break;
                case ExpressionType.MemberInit:
                    var memberInitExpression = (MemberInitExpression)expression;
                    var expressionBindings = memberInitExpression.Bindings;

                    for (var i = 0; i < expressionBindings.Count; i++)
                    {
                        var expressionBinding = (MemberAssignment)expressionBindings[i];
                        var bindingExpression = expressionBinding.Expression;
                        var bindingMemberName = $"{category}{(categorySeparator ? "." : "")}{expressionBinding.Member.Name}";

                        switch (bindingExpression.NodeType)
                        {
                            case ExpressionType.MemberAccess:
                                memberNames.Add(bindingMemberName);

                                break;
                            case ExpressionType.MemberInit:
                            case ExpressionType.New:
                                if (depth != 0)
                                {
                                    var extraExpressionMemberNames = bindingExpression.GetMemberNames(processedDepth, bindingMemberName);

                                    memberNames.AddRange(extraExpressionMemberNames);
                                }

                                break;
                            default:
                                break;
                        }
                    }

                    break;
                case ExpressionType.New:
                    var newExpression = (NewExpression)expression;
                    var expressionArguments = newExpression.Arguments;
                    var expressionMembers = newExpression.Members;

                    for (var i = 0; i < expressionMembers.Count; i++)
                    {
                        var expressionArgument = expressionArguments[i];
                        var expressionMember = expressionMembers[i];
                        var memberName = $"{category}{(categorySeparator ? "." : "")}{expressionMember.Name}";

                        switch (expressionArgument.NodeType)
                        {
                            case ExpressionType.MemberAccess:
                                memberNames.Add(memberName);

                                break;
                            case ExpressionType.MemberInit:
                            case ExpressionType.New:
                                if (depth != 0)
                                {
                                    var extraExpressionMemberNames = expressionArgument.GetMemberNames(processedDepth, memberName);

                                    memberNames.AddRange(extraExpressionMemberNames);
                                }

                                break;
                            default:
                                break;
                        }
                    }

                    break;
            }

            return memberNames;
        }
        
        public static MemberExpression GetMemberExpression(this Expression expression, string[] propertyNames)
        {
            MemberExpression memberExpression = null;

            for (var i = 0; i < propertyNames.Length; i++)
            {
                if (memberExpression != null)
                {
                    expression = memberExpression;
                }

                try
                {
                    memberExpression = Expression.Property(expression, expression.Type, propertyNames[i]);
                }
                catch (Exception _)
                {
                    continue;
                }
            }

            return memberExpression;
        }
    }
}
