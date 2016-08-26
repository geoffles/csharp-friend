using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Friend
{
    /// <summary>
    /// Base class friend invoker
    /// </summary>
    public abstract class Friend<T>
    {
        /// <summary>
        /// The target of invocation
        /// </summary>
        public T Target { get; private set; }

        /// <summary>
        /// Create a new instance of this base class type against given target object.
        /// </summary>
        /// <param name="target"></param>
        protected Friend(T target)
        {
            Target = target;
        }

        /// <summary>
        /// Uses the stackframe to inspect the calling type and acquire target method name to invoke
        /// against Target. Args must simply be the arguments for the target of invocation.
        /// </summary>
        /// <example>
        /// public void SomeFriendMethod(Foo foo, Bar bar, Baz baz)
        /// {
        ///     base.Invoke(foo, bar, baz);
        /// }
        /// </example>        
        protected void Invoke(params object[] args)
        {
            var proxyFrame = new StackFrame(1);
            var callerframe = new StackFrame(2);

            var proxyInfo = proxyFrame.GetMethod();

            string methodName = proxyFrame.GetMethod().Name;
            var callingType = callerframe.GetMethod().DeclaringType;

            var targetType = Target.GetType();

            var friendsList = targetType
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(method => method.Name == methodName)
                .Where(method => method
                    .GetCustomAttributes(typeof(FriendAttribute), false)
                    .OfType<FriendAttribute>()
                    .Any(attribute => attribute
                        .Friends
                        .Any(friendType => friendType.IsAssignableFrom(callingType))
                    )
                )
                .ToList();

            if (!friendsList.Any())
            {
                var message = string.Format("The calling method has no friends");

                throw new InvalidOperationException(message);
            }

            var targetMethod = friendsList.First();

            targetMethod.Invoke(Target, args);
        }
    }
}
