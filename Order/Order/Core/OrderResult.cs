using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Core
{
    public class OrderResult
    {
        public class OrderResults
        {
            /// <summary>
            /// Execution result
            /// </summary>
            public string Status { get; set; }

            /// <summary>
            /// Execution message
            /// </summary>
            public string Message { get; set; }
        }

        /// <summary>
        /// Execution result container
        /// </summary>
        /// <typeparam name="T">Execution return data</typeparam>
        /// <seealso cref="WsResult" />
        public class OrderResults<T> : OrderResult
        {
            /// <summary>
            /// Execution return data
            /// </summary>
            public T Payload { get; set; }
        }
    }
}
