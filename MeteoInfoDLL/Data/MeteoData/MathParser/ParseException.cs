﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Parse exception
    /// </summary>
    public class ParseException:Exception
    {
        /// <summary>Initializes a new instance of the <see cref="ParseException"/> class.</summary>
        public ParseException()
            : base()
        { }

        /// <summary>Initializes a new instance of the <see cref="ParseException"/> class.</summary>
        /// <param name="message">The message.</param>
        public ParseException(string message)
            : base(message)
        { }

        /// <summary>Initializes a new instance of the <see cref="ParseException"/> class.</summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ParseException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected ParseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
