// <copyright file="InfrastructureSettings.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Infrastructure
{
    using System;
    using Nortridge.NlsWebApi.Client;

    public static class InfrastructureSettings
    {
        public static Func<Loanacct_Detail_2Dto, string> NicknameFieldMap => x => x.Userdef50;
    }
}
