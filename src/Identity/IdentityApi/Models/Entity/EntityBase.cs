namespace IdentityApi.Models.Entity
{
    public abstract class EntityBase
    {
        /// <summary>
        /// Record Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        /// Record Created By - User Id
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Date and Time of Record Creation
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Record Modified/Updated By - User Id
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Date and Time of Record Modification/Updation
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Is Record Deleted? (Default Value : '0' - False)
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Record Deleted By - User Id
        /// </summary>
        public string DeletedBy { get; set; }

        /// <summary>
        /// Date and Time of Record Deletion
        /// </summary>
        public DateTime? DeletedAt { get; set; }
        public string ExternalId { get; set; }
    }
}
