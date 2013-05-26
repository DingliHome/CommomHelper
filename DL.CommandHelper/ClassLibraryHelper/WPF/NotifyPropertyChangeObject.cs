using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;

namespace ClassLibraryHelper.WPF
{
    /// <summary>
    /// This object automatically implements notify property changed
    /// </summary>
    public class NotifyPropertyChangeObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Track changes or not.
        /// If we're working with DTOs and we fill up the DTO
        /// in the DAL we should not be tracking changes.
        /// </summary>
        private bool trackChanges;

        /// <summary>
        /// This constructor will initialize the change tracking
        /// </summary>
        public NotifyPropertyChangeObject()
        {
            // Change tracking default
            //trackChanges = true; marked by Henry
            // New change tracking dictionary
            Changes = new Dictionary<string, object>();
        }

        /// <summary>
        /// Changes to the object
        /// </summary>
        public Dictionary<string, object> Changes { get; private set; }

        /// <summary>
        /// Is the object dirty or not?
        /// </summary>
        public bool IsDirty
        {
            get { return Changes.Count > 0; }
            set { ; }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Event required for INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Reset the object to non-dirty
        /// </summary>
        public void Reset()
        {
            Changes.Clear();
            NotifyPropertyChanged("IsDirty");
        }

        /// <summary>
        /// Start tracking changes
        /// </summary>
        public void StartTracking()
        {
            trackChanges = true;
        }

        /// <summary>
        /// Stop tracking changes
        /// </summary>
        public void StopTracking()
        {
            trackChanges = false;
        }

        /// <summary>
        /// Change the property if required and throw event
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public void ApplyPropertyChange<T, F>(ref F field,
                                              Expression<Func<T, object>> property, F value)
        {
            // Only do this if the value changes
            if (field == null || !field.Equals(value))
            {
                // Get the property
                MemberExpression propertyExpression = GetMemberExpression(property);
                if (propertyExpression == null)
                    throw new InvalidOperationException("You must specify a property");
                // Property name
                string propertyName = propertyExpression.Member.Name;
                // Set the value
                field = value;
                // If change tracking is enabled, we can track the changes...
                if (trackChanges)
                {
                    // Change tracking
                    Changes[propertyName] = value;
                    // Notify change
                    NotifyPropertyChanged(propertyName);
                }
            }
        }

        /// <summary>
        /// Get member expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public MemberExpression GetMemberExpression<T>(Expression<Func<T,
                                                           object>> expression)
        {
            // Default expression
            MemberExpression memberExpression = null;
            // Convert
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression) expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
                // Member access
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }
            // Not a member access
            if (memberExpression == null)
                throw new ArgumentException("Not a member access","expression");
            // Return the member expression
            return memberExpression;
        }

        /// <summary>
        /// The property has changed
        /// </summary>
        /// <param name="propertyName"></param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Convert the changes to an XML string
        /// </summary>
        /// <returns></returns>
        public string ChangesToXml()
        {
            // Prepare base objects
            var declaration = new XDeclaration("1.0",
                                               Encoding.UTF8.HeaderName, String.Empty);
            var root = new XElement("Changes");
            // Create document
            var document = new XDocument(declaration, root);
            // Add changes to the document
            // TODO: If it's an object, maybe do some other things
            foreach (var change in Changes)
                root.Add(new XElement(change.Key, change.Value));
            // Get the XML
            return document.Document.ToString();
        }
    }
}