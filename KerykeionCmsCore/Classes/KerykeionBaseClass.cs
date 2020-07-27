using KerykeionStringExtensions;
using System;

namespace KerykeionCmsCore.Classes
{
    /// <summary>
    /// Represents an abstract base class for Entity Framework purposes with a property 'Id' of type Guid to use as a primary key.
    /// And a property DateTimeCreated to remember the date and time of entity creation.
    /// </summary>
    /// <remarks>
    /// Every added entity to the DbContext should inherit from this class.
    /// </remarks>
    public abstract class KerykeionBaseClass
    {
        /// <summary>
        /// Gets or sets the primary key for the entity named 'Id'.
        /// </summary>
        public Guid Id { get; set; }

        private DateTime? dateTimeCreated;

        /// <summary>
        /// Gets the created DateTime of the entity.
        /// </summary>
        public DateTime? DateTimeCreated
        {
            get 
            {
                return dateTimeCreated ?? DateTime.Now;
            }
            set 
            {
                if (value != null) dateTimeCreated = value;
                else dateTimeCreated = DateTime.Now;
            }
        }

        /// <summary>
        /// Gets the Unique Name Identifier for the Entity.
        /// </summary>
        public string UniqueNameIdentifier => Name?.CompleteTrimAndUpper();
        /// <summary>
        /// Gets or sets the Name for the Entity.
        /// </summary>
        public string Name { get; set; }
    }
}
