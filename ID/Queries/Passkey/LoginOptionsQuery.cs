﻿using Fido2NetLib;
using MediatR;

namespace ID.Queries.Passkey
{
    public class LoginOptionsQuery : IRequest<AssertionOptions>
    {
        public string Identifire { get; set; }
    }
}
