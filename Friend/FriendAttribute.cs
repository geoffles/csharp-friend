using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friend
{
    /// <summary>
    /// Marks a method as accessible by the types defined.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class FriendAttribute : Attribute
    {
        /// <summary>
        /// Create a new instance of Friend Attribute with a list of friend types.
        /// </summary>
        /// <param name="friends">Each type in this list will be granted access via a <see cref="Friend"/> invoker</param>
        public FriendAttribute(params Type[] friends)
        {
            Friends = friends;
        }

        /// <summary>
        /// Current list of associated friends. This cannot be changed at runtime.
        /// </summary>
        public Type[] Friends { get; private set; }
    }
}
