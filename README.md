
# Borrower Portal

## Overview
This project is an ASP.NET Core Web Application designedd as a web portal for borrowers to interface with their loans in NLS. 
All database queries will be made via the Nortridge Service Platform RESTful API (NSP API) through a statically typed C# .NET Standard 2.0 library (Nortridge.NlsWebApi.Client).
Authentication will be done through the Nortridge Service Platform Authentication Server (NSP Auth) which is an implementation of OpenId Connect using IdentityServer3.

---

## Getting Started

### Build
In order to build the project you will need the following:
  * [.NET Core 2.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/2.1)
  * [Node.js](https://nodejs.org/en/)
  * [Webpack](https://webpack.js.org/)

---

### Run / Debug
In order to debug the project you will need the following in addition to the build prerequisites:
  * Visual Studio 2017
    * Right click on the Nortridge.Borrower project in Visual Studio and click on the "Manage User Secrets" menu item. This will open a secrets.json file.  Copy the contents of the appSettings.json file and paste them into the secrets.json file.  The secrets.json file can be used to store configuration settings without checking them into source control.  For more information, see [here](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.1&tabs=windows#json-structure-flattening-in-visual-studio-1).
  * NSP Auth v10.0.0
    * Configured to use the Resource Owner flow (default)
  * NSP API v10.0.0

Debug Steps:
  * Run the following command in the Nortridge.BorrowerPortal project directory.
    This command will launch webpack to compile the front-end files (e.g. *.scss, *.ts, etc.) and copy the compiled files into the /wwwroot/dist folder.
    When you save one of these files, webpack will recompile automatically.
    ```bash
    npm run watch
    ```
  * Launch the Visual Studio 2017 Debugger

---

## Technology Stack

### Back End
  * **Framework**:  [ASP.NET Core Razor Pages](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-2.2&tabs=visual-studio)
  * **Authentication**:  OpenID Connect Hybrid flow via NSP Auth
  * **Data Access**: NSP API calls using the NLS Web API Client library
  * **Package Management**: [NuGet](https://www.nuget.org/)
  * **Unit Testing**
    * [xUnit.net](https://xunit.net/)
    * [Moq](https://github.com/moq/moq4)
  * **Logging**: [Log4Net](https://github.com/huorswords/Microsoft.Extensions.Logging.Log4Net.AspNetCore)

### Front End
  * **Component Libraries**
    * [Bootstrap 4](https://getbootstrap.com/)
  * **Client-side Scripts**:  [TypeScript](https://www.typescriptlang.org/)
  * **Package Management**: [npm](https://www.npmjs.com/)
  * **Build Tools**
    * [webpack](https://webpack.js.org/)

---

## Guidelines

### Code Style
Visual Studio 2017 will give code style suggestions via Quick Action light bulbs and warnings in the Error List tab.
Code style guidelines for C# are provided by the [StyleCopAnalyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers) Nuget package.

* All code style warnings should be handled before committing code.

---

### Code Analysis
Visual Studio 2017 will give code analysis suggestions via Quick Action light bulbs and warnings in the Error List tab.
Code analysis for C# is provided by the [Microsoft.CodeAnalysis.FxCopAnalyzers](https://github.com/dotnet/roslyn-analyzers#microsoftcodeanalysisfxcopanalyzers) Nuget package.
Code analysis for TypeScript is provided by the [TSLint](https://palantir.github.io/tslint/) tool.

* All code analysis warnings should be handled before committing code.

---

### Data Access
All data access will be performed by making calls to the Nortridge Service Platform RESTful API (NSP API) through a statically typed C# .NET Standard 2.0 library (Nortridge.NlsWebApi.Client).
This library is available as a NuGet package.
The access token is automatically injected into the authorization header via the AuthorizationHeaderHandler class.


```csharp
    ...
    using NlsWebApi.Client;

    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly INlsWebApiClient nlsWebApiClient;

        public IndexModel(INlsWebApiClient nlsWebApiClient)
        {
            this.nlsWebApiClient = nlsWebApiClient;
        }

        public async Task OnGet()
        {
            var contactId = 1;
            var contactResponse = await this.nlsWebApiClient.Contacts_GetAsync(contactId);
            var cashDrawers = await this.nlsWebApiClient.CashDrawers_GetAllAsync();
        }
    }
```

---

### Back End Package Management
All libraries used by the back end (i.e. .NET libraries) should be managed via [NuGet package manager](https://www.nuget.org/) which is integrated with Visual Studio 2017.

--- 

### Back End Logging
Logging for the Back End is done using a [Log4Net](https://github.com/huorswords/Microsoft.Extensions.Logging.Log4Net.AspNetCore) extension for ASP.NET Core.

* Use Log Levels.
  * For Exceptions, use the LogError function and pass in the exception object.
* Use the IStringLocalizer class when logging error messages.
* Sample
    ```csharp
        private readonly ILogger<IndexModel> logger;
        private readonly IStringLocalizer<IndexModel> localizer;

        public IndexModel(
            ILogger<IndexModel> logger,
            IStringLocalizer<IndexModel> localizer)
        {
            this.logger = logger;
            this.localizer = localizer;
        }

        public async Task OnGet()
        {
            this.logger.LogTrace(this.localizer["TRACE LEVEL TEST"]);
            this.logger.LogDebug(this.localizer["DEBUG LEVEL TEST"]);
            this.logger.LogInformation(this.localizer["INFO LEVEL TEST"]);
            this.logger.LogWarning(this.localizer["WARN LEVEL TEST"]);
            this.logger.LogError(this.localizer["ERROR LEVEL TEST"]);
            this.logger.LogCritical(this.localizer["CRITICAL LEVEL TEST"]);
        }
    ```

---

### Globalization and Localization
Develop using string literals in english (en-US) and surround them in the appropriate Localizer calls.
This gives us the ability to add additional languages later using Resource files.
For more information on Globalization and Localization in ASP.NET Core, see [documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2).

* All string literals in Razor (.cshtml) should be surrounded by a call to @Localizer.
  ```html
    <p>@Localizer["Loan"]</p>
  ```
* Use the IStringLocalizer class in Page Models when returning string literals back to the client for display (e.g. an error message).

---

### Front End Styles (CSS)
CSS should be managed using the [Sass Preprocessor](https://sass-lang.com/).
Sass (.scss) files are stored in the following location: /app/scss.
During a build, Webpack comiles the .scss files into a main.css file and moves it into the /wwwroot/dist folder.

* **Do not add inline styles** to an HTML element.  Use a CSS class instead.
* Use Sass Variables to store commonly used CSS values (e.g. colors, fonts, sizes).

#### Bootstrap Styling
Bootstrap 4 is included in the project with Webpack (see [bootstrap documentation](https://getbootstrap.com/docs/4.0/getting-started/webpack/)).
Bootstrap Sass variables can be configured in the /wwwroot/scss/_custom.css file (see [bootstrap documentation](https://getbootstrap.com/docs/4.0/getting-started/theming/).

---

### Responsive Design

* All pages and components should be built to be responsive using the [Bootstrap 4 grid system](https://getbootstrap.com/docs/4.3/layout/grid/).

---

### Client-side Package Management
All client-side libraries (i.e. libraries that are used for front-end development) should be managed via [node package manager](https://www.npmjs.com/) (npm).
See [npm documentation](https://docs.npmjs.com/) for more information.

For Development Dependencies, make sure you use the --save-dev flag when using the npm install command.

Client-side libraries should be included using Webpack with an import statement in the main.ts file.

---

### Client-side Scripts
All custom client-side scripts (i.e. scripts that run in the browser) should be written in [TypeScript](https://www.typescriptlang.org/).
During a build, Webpack transpiles the TypeScript files into JavaScript and moves them into the /wwwroot/dist folder.

#### Steps to Add a TypeScript file
  1. Create a TypeScirpt (.ts) file in one of the following locations:
     * **Shared Scripts**: /wwwroot/src/shared/module-name.ts
     * **Page Scripts**: /wwroot/src/pages/page-name.ts
     * **View Component Scripts**: /wwwroot/src/components/component-name.ts
  2. Add a reference to the transpiled JavaScript (.js) file found in the /wwwroot/dist folder
     * **Shared Scripts**: Webpack will automatically import shared scripts into the scripts dependedent on them
     * **Page Scripts**: The following code should be added to the Razor (.cshtml) file for the Page
       ```html
        @section Scripts {
            <environment include="Development">
                <script src="~/dist/pages.page-name.js"></script>
            </environment>
            <environment exclude="Development">
                <script src="~/dist/pages.page-name.js" asp-append-version="true"></script>
            </environment>
        }
       ```
     * **View Component Scripts**:  The following code should be added to the Razor (.cshtml) file for the View Component
       ```html
        <environment include="Development">
            <script src="~/dist/components.component-name.js"></script>
        </environment>
        <environment exclude="Development">
            <script src="~/dist/components.component-name.js" asp-append-version="true"></script>
        </environment>
       ```

