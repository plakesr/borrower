// <copyright file="NavigationBuilder.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Infrastructure.Navigation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Nortridge.BorrowerPortal.Core.Extensions;

    public static class NavigationBuilder
    {
        public static readonly ReadOnlyCollection<string> Empty =
            new ReadOnlyCollection<string>(Array.Empty<string>());

        public static readonly SubNavigationItemModel Balances =
            new SubNavigationItemModel("Balances", "/Index", "icon-sidebar-balances", new[]
            {
                "/Balances/Details/Index"
            }.AsReadOnly());

        public static readonly SubNavigationItemModel PaymentHistory =
            new SubNavigationItemModel("Payment History", "/PaymentHistory/Index", "icon-sidebar-payment-history", Empty);

        public static readonly SubNavigationItemModel SubmitPayment =
            new SubNavigationItemModel("Make a Payment", "/SubmitPayment/Index", "icon-sidebar-submit-payment", Empty);

        public static readonly SubNavigationItemModel Statement =
            new SubNavigationItemModel(
                "View Statement",
                "/Statement/Index",
                "icon-sidebar-statement",
                new[] { "/Statement/Details/Index" }.AsReadOnly());

        /*
        public static readonly SubNavigationItemModel ACHSetup =
            new SubNavigationItemModel("ACH Setup", "/ACHSetup/Index", "icon-sidebar-ach", Empty);
        */

        public static readonly SubNavigationItemModel PendingPayments =
            new SubNavigationItemModel("Pending Payments", "/PendingPayments/Index", "icon-sidebar-pending-payments", Empty);

        public static readonly NavigationItemModel Accounts = new NavigationItemModel("Accounts", Balances.Path, new[]
        {
            Balances,
            PaymentHistory,
            SubmitPayment,
            Statement,
            /* ACHSetup, */
            PendingPayments,
        }.AsReadOnly());

        public static readonly SubNavigationItemModel ChangeAddress =
            new SubNavigationItemModel("Change of Address", "/ChangeAddress/Index", "icon-sidebar-address", Empty);

        public static readonly NavigationItemModel OtherServices = new NavigationItemModel("Other Services", ChangeAddress.Path, new[]
        {
            ChangeAddress
        }.AsReadOnly());

        public static readonly SubNavigationItemModel Nicknames =
            new SubNavigationItemModel("Loan Nicknames", "/Nicknames/Index", "icon-sidebar-nickname", Empty);

        public static readonly SubNavigationItemModel ChangePassword =
            new SubNavigationItemModel("Change Password", "/Account/ChangePassword/Index", "icon-sidebar-password", Empty);

        public static readonly SubNavigationItemModel SecuritySettings =
            new SubNavigationItemModel("Security Questions", "/SecuritySettings/Index", "icon-sidebar-security", Empty);

        public static readonly NavigationItemModel Preferences = new NavigationItemModel("Preferences", Nicknames.Path, new[]
        {
            Nicknames,
            ChangePassword,
            SecuritySettings,
        }.AsReadOnly());

        public static readonly ReadOnlyCollection<NavigationItemModel> TopNavigation = new[]
        {
            Accounts,
            OtherServices,
            Preferences
        }.AsReadOnly();

        public static readonly INavigationItem Home =
            new NavigationItemModel("Home", Balances.Path, Enumerable.Empty<SubNavigationItemModel>().ToReadOnly());

        public static NavigationItemModel ActiveNavigationItem(string path) =>
            TopNavigation.FirstOrDefault(_ =>
                _.Path == path ||
                _.Children.Any(sub => sub.Path == path || sub.RelatedPaths.Contains(path)));

        public static SubNavigationItemModel ActiveSubNavigationItem(NavigationItemModel navItem, string path) =>
            navItem.Children.First(sub => sub.Path == path || sub.RelatedPaths.Contains(path));
    }
}
