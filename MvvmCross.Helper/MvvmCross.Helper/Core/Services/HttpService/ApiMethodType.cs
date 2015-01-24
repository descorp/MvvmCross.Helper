namespace MvvmCross.Helper.Core.Services.HttpService
{
    /// <summary>
    /// The request type.
    /// </summary>
    public enum ApiMethodType
    {
        /// <summary>
        /// default value
        /// </summary>
        Default,

        /// <summary>
        /// The contact list.
        /// </summary>
        GetContacts,

        /// <summary>
        /// The voicemail.
        /// </summary>
        GetVoicemails,

        /// <summary>
        /// The media.
        /// Path:
        /// account/voicemail/{MEDIAID}/raw
        /// Parameters:
        /// MEDIAID - string
        /// </summary>
        GetVoicemailMedia,

        /// <summary>
        /// THe Queues
        /// </summary>
        GetQueues,

        /// <summary>
        /// Get a list of devices
        /// </summary>
        GetDevices,

        /// <summary>
        /// Get External Numbers
        /// </summary>
        GetNumbers,

        /// <summary>
        /// The login.
        /// Path:
        /// auth?instance={INSTANCE} userId={USERID} pin={PIN}
        /// Parameters:
        /// INSTANCE - string
        /// USERID   - string
        /// PIN      - string
        /// </summary>
        /// <example>
        /// new Dictionary( <see cref="string" />, <see cref="object" />) { {"INSTANCE", ...}, {"PIN", ...}, {"USERID", ...} }
        /// </example>
        GetAuth,

        /// <summary>
        /// The logout.
        /// </summary>
        GetAuthOut,

        /// <summary>
        /// Make transfer
        /// Path:
        /// account/contacts/{USERID}/transfer
        /// Parameters:
        /// USERID - This attribute contains a URI that identifies the user whose dialog information is reported in the remainder
        /// of the document.  This user is referred to as the "observed user".
        /// </summary>
        GetTransfer,

        /// <summary>
        /// Set device's state
        /// Path:
        /// account/devices/{ID}/state?state={ENABLE}
        /// Parameters:
        /// ID - string
        /// ENABLE - Boolean
        /// </summary>
        UpdateDeviceState,

        /// <summary>
        /// Update voicemail
        /// Path:
        /// account/voicemail
        /// Parameters:
        /// Voicemails - VmcContact
        /// </summary>
        PutVoicemails,

        /// <summary>
        /// Save Settings
        /// Path:
        /// account
        /// Parameters:
        /// ACCOUNT - VmcContact
        /// </summary>
        PutAccount,

        /// <summary>
        /// The change status.
        /// Path:
        /// account/status?status={STATUS}
        /// Parameters:
        /// STATUS - VmcContactStatus
        /// </summary>
        PutAccountStatus,

        /// <summary>
        /// The number change status.
        /// Path:
        /// account/numbers
        /// Parameters:
        /// NUMBERS - Array of VmcNumbers
        /// </summary>
        PutNumbers,

        /// <summary>
        /// Post request to upload service
        /// Path:
        /// monitor/report/{user}
        /// Parameters:
        /// USER - user's name
        /// STREAM - stream with report
        /// </summary>
        PostReport,

        /// <summary>
        /// Get list of registered devices
        /// Path:
        /// account/contacts/devices
        /// </summary>
        GetRegisteredDevices,

        /// <summary>
        /// Get current list of  channels
        /// Path:
        /// account/contacts/channels
        /// Parameters:
        /// </summary>
        GetChannels
    }
}