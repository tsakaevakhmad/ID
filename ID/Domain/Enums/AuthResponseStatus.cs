namespace ID.Domain.Enums
{
    public enum AuthResponseStatus
    {
        SendedMailConfirmationCode,
        SendedPhoneNumberConfirmationCode,
        UserAlreadyExists,
        UserNotFound,
        UserMailNotConfirmed,
        UserPhoneNotConfirmed,
        UserPhoneAlreadyExists,
        UserMailAlreadyExists,
        SendedLoginCodeToEmail,
        SendedLoginCodeToPhoneNumber,
    }
}
