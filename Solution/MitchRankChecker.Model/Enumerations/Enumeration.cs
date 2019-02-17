using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/*
    It's arguably recommended to use
    Enumeration classes instead of the
    built-in enum constructs. Classes
    gives more flexibility and is
    more robust, and is the recommended
    approach by Microsoft docs.
    
    For more information, refer to:
      https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types
*/

namespace MitchRankChecker.Model.Enumerations
{
    /// <summary>
    /// Abstract enumeration class, providing
    /// common enumeration capabilities.
    /// </summary>
    public abstract class Enumeration : IComparable
    {
        #region Properties
        /// <summary>
        /// The name of the enumeration
        /// </summary>
        /// <value></value>
        public string Name { get; private set; }

        /// <summary>
        /// The id/value of the enumeration.
        /// </summary>
        /// <value></value>
        public int Id { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        protected Enumeration()
        { }

        /// <summary>
        /// Constructor to create a new enumeration.
        /// </summary>
        /// <param name="id">The enumeration ID</param>
        /// <param name="name">The enumeration name</param>
        protected Enumeration(int id, string name) 
        {
            Id = id; 
            Name = name; 
        }
        #endregion

        #region Overrides
        /// <inheritdoc/>
        public override string ToString() => Name;

        /// <inheritdoc/>
        public override bool Equals(object obj) 
        {
            var otherValue = obj as Enumeration; 

            if (otherValue == null) 
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        #endregion

        #region IComparable Implementation
        /// <inheritdoc/>
        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
        #endregion

        #region Utility Methods
        /// <summary>
        /// Get all available enumeration values for a particular enumeration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Enumerable list of enumerations</returns>
        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | 
                                            BindingFlags.Static | 
                                            BindingFlags.DeclaredOnly); 

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }
        
        /// <summary>
        /// Get an enumeration value based on a given ID.
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FromId<T>(int id) where T : Enumeration, new()
        {
            var matchingItem = parse<T, int>(id, "id", item => item.Id == id);
            return matchingItem;
        }

        /// <summary>
        /// Get an enumeration value based on a given name.
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FromName<T>(string name) where T : Enumeration, new()
        {
            var matchingItem = parse<T, string>(name, "name", item => item.Name == name);
            return matchingItem;
        }

        /// <summary>
        /// Gets a particular enumeration value based on a predicate.
        /// </summary>
        /// <param name="value">The value of enumeration, shown when an error occurs.</param>
        /// <param name="description">The description of enumeration, shown when an error occurs.</param>
        /// <param name="predicate">The anonymous function detailing the filter operation to perform on the enumerations.</param>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <typeparam name="K">The type of value to extract from the enumeration.</typeparam>
        /// <returns>The enumeration</returns>
        private static T parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration, new()
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
                throw new ApplicationException(message);
            }

            return matchingItem;
        }
        #endregion
    }
}