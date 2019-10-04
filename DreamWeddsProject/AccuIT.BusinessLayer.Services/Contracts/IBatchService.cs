
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using System.Collections.Generic;
namespace Samsung.SmartDost.BusinessLayer.Services.Contracts
{
    public interface IBatchService
    {
        /// <summary>
        /// This method will be called to sync data into DMS
        /// </summary>
        void SyncDataToDMS();

        /// <summary>
        /// Method to insert email record into database
        /// </summary>
        /// <param name="email">email entity</param>
        /// <returns>returns boolean status</returns>
        bool InsertEmailRecord(EmailServiceDTO email);

        /// <summary>
        /// Method to fetch batch emails to send email
        /// </summary>
        /// <returns>returns batch emails</returns>
        IList<EmailServiceDTO> GetBatchEmails();
    }
}
