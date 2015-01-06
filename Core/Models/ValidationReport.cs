namespace MobileApp.Core.Models
{
    using System;
    using System.Collections.Generic;

    public class ValidationReport
    {
        private readonly bool isPinOk;

        private readonly bool isInstanceOk;

        private readonly bool isUserOk;

        /// <summary>
        /// List of Validation messages
        /// </summary>
        public List<string> Messages;

        /// <summary>
        /// Create new validation report
        /// </summary>
        /// <param name="isInstanceOk">validation of Instance</param>
        /// <param name="isUserOk">validation of User Id</param>
        /// <param name="isPinOk">validation of Pin</param>
        public ValidationReport(bool isInstanceOk, bool isUserOk, bool isPinOk)
        {
            this.isInstanceOk = isInstanceOk;
            this.isUserOk = isUserOk;
            this.isPinOk = isPinOk;
        }

        /// <summary>
        /// Create new validation report
        /// </summary>
        public ValidationReport(bool[] validation)
        {
            if (validation == null)
            {
                throw new ArgumentNullException("validation");
            }

            this.isInstanceOk = validation[0];
            this.isUserOk = validation[1];
            this.isPinOk = validation[2];
        }

        public bool IsUserOk
        {
            get
            {
                return this.isUserOk;
            }
        }

        public bool IsInstanceOk
        {
            get
            {
                return this.isInstanceOk;
            }
        }

        public bool IsPinOk
        {
            get
            {
                return this.isPinOk;
            }
        }
    }
}
