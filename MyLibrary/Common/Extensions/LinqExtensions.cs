using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Extensions
{
    /// <summary>
    /// 自定义Lambda扩展
    /// </summary>
    public static class LinqExtensions
    {

        private static string _name = "p";
        #region 公共方法
        
        /// <summary>
        /// 创建lambda表达式：p=>true
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            List<string> aa = new List<string>();

            return p => true;
        }
       
        /// <summary>
        /// 创建lambda表达式：p=>false
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return p => false;
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <typeparam name="TKey">参数类型</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <returns></returns>
        public static Expression<Func<T, TKey>> GetOrderExpression<T, TKey>(string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), _name);
            return Expression.Lambda<Func<T, TKey>>(Expression.Property(parameter, propertyName), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName == propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateEqual<T>(string propertyName, object propertyValue)
        {
            Type typeValue = propertyValue.GetType();
            return CreateEqual<T>(propertyName, propertyValue, typeValue);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName == propertyValue
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <param name="propertyValue">数据值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateEqual<T>(string propertyName, object propertyValue, Type typeValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), _name);//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, typeValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(member, constant), parameter);
        }
        
        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName != propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateNotEqual<T>(string propertyName, object propertyValue)
        { 
            Type typeValue = propertyValue.GetType();
            return CreateNotEqual<T>(propertyName, propertyValue, typeValue);
        }


        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName != propertyValue
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <param name="propertyValue">数据值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateNotEqual<T>(string propertyName, object propertyValue, Type typeValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), _name);//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, typeValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.NotEqual(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName > propertyValue
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <param name="propertyValue">数据值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateGreaterThan<T>(string propertyName, object propertyValue, Type typeValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), _name);//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, typeValue);//创建常数

            MethodInfo methodInfo = typeValue.GetMethod("CompareTo", new Type[] { typeValue });//因为CompareTo有重载，所以这里指定了下参数的类型，否则会报反射异常  
            BinaryExpression body =
                                Expression.GreaterThan(
                                    Expression.Call(
                                        member,
                                        methodInfo,
                                        constant
                                            ),
                                    Expression.Constant(0, typeof(Int32)) //比较String.CompareTo的返回结果和0，来实现>=的效果  
                                    );

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName小于propertyValue 
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <param name="propertyValue">数据值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateLessThan<T>(string propertyName, object propertyValue, Type typeValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), _name);//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, typeValue);//创建常数
            MethodInfo methodInfo = typeValue.GetMethod("CompareTo", new Type[] { typeValue });//因为CompareTo有重载，所以这里指定了下参数的类型，否则会报反射异常  
            BinaryExpression body =
                                Expression.LessThan(
                                    Expression.Call(
                                        member,
                                        methodInfo,
                                        constant
                                            ),
                                    Expression.Constant(0, typeof(Int32)) //比较String.CompareTo的返回结果和0，来实现>=的效果  
                                    );

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName >= propertyValue
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <param name="propertyValue">数据值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateGreaterThanOrEqual<T>(string propertyName, object propertyValue, Type typeValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), _name);//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, typeValue);//创建常数
            MethodInfo methodInfo = typeValue.GetMethod("CompareTo", new Type[] { typeValue });//因为CompareTo有重载，所以这里指定了下参数的类型，否则会报反射异常  
            BinaryExpression body =
                                Expression.GreaterThanOrEqual(
                                    Expression.Call(
                                        member,
                                        methodInfo,
                                        constant
                                            ),
                                    Expression.Constant(0, typeof(Int32)) //比较String.CompareTo的返回结果和0，来实现>=的效果  
                                    );
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=> p.propertyName <= propertyValue 
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <param name="propertyValue">数据值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateLessThanOrEqual<T>(string propertyName, object propertyValue, Type typeValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), _name);//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, typeValue);//创建常数
            MethodInfo methodInfo = typeValue.GetMethod("CompareTo", new Type[] { typeValue });//因为CompareTo有重载，所以这里指定了下参数的类型，否则会报反射异常  
            BinaryExpression body =
                                Expression.LessThanOrEqual(
                                    Expression.Call(
                                        member,
                                        methodInfo,
                                        constant
                                            ),
                                    Expression.Constant(0, typeof(Int32)) //比较String.CompareTo的返回结果和0，来实现>=的效果  
                                    );
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName.Contains(propertyValue)
        /// 等价于SQL中的like
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <param name="propertyValue">数据值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetContains<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), _name);
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Call(member, method, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：!(p=>p.propertyName.Contains(propertyValue))
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <param name="propertyValue">数据值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetNotContains<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), _name);
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Not(Expression.Call(member, method, constant)), parameter);
        }

        /// <summary>
        /// 拼接Or
        /// </summary>
        /// <param name="expression1">expression1</param>
        /// <param name="expression2">expression2</param>
        /// <returns>返回值</returns>
        public static Expression<Func<T, bool>> Or<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            return Compose(expression1, expression2, Expression.OrElse);
        }

        /// <summary>
        /// 拼接And
        /// </summary>
        /// <param name="expression1">expression1</param>
        /// <param name="expression2">expression2</param>
        /// <returns>返回值</returns>
        public static Expression<Func<T, bool>> And<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            return Compose(expression1, expression2, Expression.AndAlso);
        }

        /// <summary>
        /// 合并2个表达式
        /// </summary>
        /// <param name="first">first</param>
        /// <param name="second">second</param>
        /// <param name="merge">merge</param>
        /// <returns>返回值</returns>
        public static Expression<T> Compose<T>(Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        #endregion

        #region 私有方法

        private class ParameterRebinder : ExpressionVisitor
        {
            readonly Dictionary<ParameterExpression, ParameterExpression> map;
            /// <summary>
            /// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
            /// </summary>
            /// <param name="map">The map.</param>
            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }
            /// <summary>
            /// Replaces the parameters.
            /// </summary>
            /// <param name="map">The map.</param>
            /// <param name="exp">The exp.</param>
            /// <returns>Expression</returns>
            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }
            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;

                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }
                return base.VisitParameter(p);
            }
        }
        
        #endregion
        
    }
}
