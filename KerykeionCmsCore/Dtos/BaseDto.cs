using System;

namespace KerykeionCmsCore.Dtos
{
    /// <summary>
    /// Represents an abstract base class for data transfer objects with one property 'Id' of type Guid.
    /// </summary>
    public abstract class BaseDto
    {
        /// <summary>
        /// Gets or sets the primary key.
        /// </summary>
        public Guid Id { get; set; }
    }
}
