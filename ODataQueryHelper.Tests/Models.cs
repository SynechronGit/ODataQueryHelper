using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace ODataQueryHelper.Tests
{
    public class Employee
    {
        public ObjectId _id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }

    public class Company
    {
        public Object _id { get; set; }
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }
        public int NumberOfEmployees { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public DateTime ClosureDate { get; set; }
        public string FederalTaxIdentificationNumber { get; set; }
        public bool IsActive { get; set; }
        public string CompanyGroup { get; set; }
        public string Notes { get; set; }
        public string CollabGroup { get; set; }
        public int CurrentGroupID { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// Enumeration for Available Messaging account types
    /// </summary>
    public enum MessagingAccountType
    {
        /// <summary>
        /// Messaging account is for Email communication
        /// </summary>
        Email = 0,
        /// <summary>
        /// Messaging account is for SMS communication
        /// </summary>
        Sms = 1
    }
    /// <summary>
    /// Represents Messaging account to process and send communication messages
    /// </summary>
    public class MessagingAccount
    {
        /// <summary>
        /// Key Id
        /// </summary>
        public Guid _id { get; set; }
        /// <summary>
        /// Get or set friendly name for messaging account
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set from address for communication message to send
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Get or set messaging account type
        /// </summary>
        public int MessagingAccountType { get; set; }

        /// <summary>
        /// Get or set type of messaging account
        /// </summary>
        public int MessagingProviderType { get; set; }

        /// <summary>
        /// Get or set environment name , messaging account is to be used in.
        /// </summary>
        public string EnvironmentName { get; set; }

        /// <summary>
        /// Get or set Messaging security configuration associated with Messaging account
        /// </summary>
        public Guid SecurityConfigurationId { get; set; }

        /// <summary>
        /// Get or set number of attempt messaging account can be used to process communication message
        /// </summary>
        public int RetryAttemptLimit { get; set; }

        /// <summary>
        /// Get or set retry interval in miliseconds communication message has to be wait before it can be retried
        /// </summary>
        public int RetryInterval { get; set; }
       
    }
}
