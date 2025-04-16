using AutoMapper;
using Fido2NetLib.Objects;
using ID.Commands;
using ID.Commands.Passkey;
using ID.Domain.Entity;
using System.Buffers.Text;

namespace ID.MappingProfiles
{
    public class BaseProfile : Profile
    {
        public BaseProfile() 
        {
            CreateMap<RegisterCommand, User>();
            CreateMap<AttestationVerificationSuccess, FidoCredential>()
                .ForMember(dest => dest.CredentialId, opt => opt.MapFrom(src => Base64Url.EncodeToString(src.CredentialId)))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Base64Url.EncodeToString(src.User.Id)))
                .ForMember(dest => dest.AaGuid, opt => opt.MapFrom(src => src.Aaguid.ToString()))
                .ForMember(dest => dest.SignatureCounter, opt => opt.MapFrom(src => src.Counter))
                .ForMember(dest => dest.CredType, opt => opt.MapFrom(src => src.CredType));
                /*.ForMember(dest => dest.AuthenticatorDescription, opt => opt.MapFrom(src => src.PublicKey?.AttestationCertificateDescription)) // или null, если нет
                .ForMember(dest => dest.Transports, opt => opt.MapFrom(src => string.Join(",", src.Transports ?? [])));*/
        }
    }
}
