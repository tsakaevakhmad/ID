using AutoMapper;
using Fido2NetLib;
using Fido2NetLib.Objects;
using ID.Commands;
using ID.Commands.Passkey;
using ID.Commands.PassKey;
using ID.Domain.Entity;
using Microsoft.IdentityModel.Tokens;
using System.Buffers.Text;
using static ID.Commands.PassKey.RegistrationCredentialCommand;
using Base64Url = System.Buffers.Text.Base64Url;

namespace ID.MappingProfiles
{
    public class BaseProfile : Profile
    {
        public BaseProfile() 
        {
            CreateMap<RegisterCommand, User>();
            CreateMap<AttestationVerificationSuccess, FidoCredential>()
                .ForMember(dest => dest.CredentialId, opt => opt.MapFrom(src => Base64Url.EncodeToString(src.CredentialId)))
                .ForMember(dest => dest.AaGuid, opt => opt.MapFrom(src => src.Aaguid.ToString()))
                .ForMember(dest => dest.PublicKey, opt => opt.MapFrom(src => src.PublicKey))
                .ForMember(dest => dest.SignatureCounter, opt => opt.MapFrom(src => src.Counter))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Base64UrlEncoder.Decode(Base64Url.EncodeToString(src.User.Id))))
                .ForMember(x => x.User, x => x.Ignore());

            CreateMap<RegistrationCredentialCommand, AuthenticatorAttestationRawResponse>()
                .ForMember(x => x.Id, x => x.MapFrom(x => Base64Url.DecodeFromChars(x.Id)))
                .ForMember(x => x.RawId, x => x.MapFrom(x => Base64Url.DecodeFromChars(x.RawId)));
            
            CreateMap<ResponseData, Fido2NetLib.AuthenticatorAttestationRawResponse.ResponseData>()
                .ForMember(x => x.AttestationObject, x => x.MapFrom(x => Base64Url.DecodeFromChars(x.AttestationObject)))
                .ForMember(x => x.ClientDataJson, x => x.MapFrom(x => Base64Url.DecodeFromChars(x.ClientDataJson)));
        }
    }
}
